using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetTagName : MonoBehaviour
{
    public static Dictionary<string, PlanetTagName> m_planetInScene = new Dictionary<string, PlanetTagName>();

    public string m_associatedName;

    private void OnValidate()
    {
        m_associatedName = m_associatedName.ToLower();
    }

    private void Awake()
    {
        
        AddPlanetToRegister();
    }

    private void OnEnable()
    {
        AddPlanetToRegister();
    }

    private void AddPlanetToRegister()
    {
        m_associatedName = m_associatedName.ToLower();
        if (m_planetInScene.ContainsKey(m_associatedName))
        {
            m_planetInScene[m_associatedName] = this;
        }
        else
        {
            m_planetInScene.Add(m_associatedName, this);
        }
    }

    private void OnDisable()
    {
        RemovePlanetFromRegister();
    }

    private void RemovePlanetFromRegister()
    {
        if (m_planetInScene.ContainsKey(m_associatedName))
        {
            m_planetInScene.Remove(m_associatedName);
        }
    }

    public static void GetListOfPlanetsNames(out List<string> nameOfPlanest)
    {
        nameOfPlanest = m_planetInScene.Keys.ToList();
    }

    public static void Contains(string planetName, out bool found, out PlanetTagName planettag)
    {
        planetName = planetName.ToLower();
        if (found = m_planetInScene.ContainsKey(planetName))
        {
            planettag = m_planetInScene[planetName];
        }
        else planettag = null;

    }

    public static void GetListOfPlanets(out List<PlanetTagName> nameOfPlanest)
    {
        nameOfPlanest = m_planetInScene.Values.ToList();
    }
}
