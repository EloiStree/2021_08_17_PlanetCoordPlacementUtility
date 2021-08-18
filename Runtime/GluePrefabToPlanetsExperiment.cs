using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GluePrefabToPlanetsExperiment : MonoBehaviour
{
    public List<string> m_planets= new List<string>();
    public PlaneteryPlacementUtilityMono m_placementUtility;
    // Start is called before the first frame update
    void Start()
    {
       CheckForPlanets(); 
    }
    public void CheckForPlanets() {
        PlanetTagName.GetListOfPlanetsNames( out m_planets);
    }

    public bool DoesPlanetExists(string planetName) {
        planetName = planetName.ToLower();
        return m_planets.Where(k => k.ToLower() == planetName).Count() > 0;
    }
    public void TryToGlue(GameObject objectToGlue, GPSHorizontalGroundPositioning coordinate, string planetName)
    {
        GetPlanetInfoOf(planetName, out bool found, out PlanetRootMono planet);
        if (found)
        {

            planet.m_glueZone.GlueToThisPlanet(objectToGlue.transform);
            m_placementUtility.TryToPlaceOnGround(objectToGlue, planet.m_planetInformation, coordinate);
        }
    }
    public void TryToGlue(GameObject objectToGlue, GPSHorizontalInSpacePositioning coordinate, string planetName)
    {
        GetPlanetInfoOf(planetName, out bool found, out PlanetRootMono planet);
        if (found)
        {

            planet.m_glueZone.GlueToThisPlanet(objectToGlue.transform);
            m_placementUtility.TryToPlaceAround(objectToGlue, planet.m_planetInformation, coordinate);
        }
    }
    public void TryToGlue(GameObject objectToGlue, GPSHorizontalInSpaceLookAtPositioning coordinate, string planetName)
    {
        GetPlanetInfoOf(planetName, out bool found, out PlanetRootMono planet);
        if (found)
        {
            planet.m_glueZone.GlueToThisPlanet(objectToGlue.transform);
            m_placementUtility.TryToPlaceAround(objectToGlue, planet.m_planetInformation, coordinate);
        }
    }
    public void GetPlanetTagOf(string planetName, out bool found, out PlanetTagName planettag)
    {
        PlanetTagName.Contains(planetName, out found, out planettag);
    }
    public void GetPlanetInfoOf(string planetName, out bool found, out PlanetRootMono planet)
    {
        found = false;
        planet = null;
        GetPlanetTagOf(planetName, out bool foundTag, out PlanetTagName tag);
        if (foundTag) {
            SearchForPlanetInfoNearTag(tag, out found, out planet);
        }

    }

    private void SearchForPlanetInfoNearTag(PlanetTagName tag, out bool found, out PlanetRootMono planet)
    {
        planet = tag.gameObject.GetComponent<PlanetRootMono>();
        if (planet == null)
            planet = tag.gameObject.GetComponentInChildren<PlanetRootMono>();
        found = planet != null;

    }

    public void Glue(GameObject objectToGlue, Transform parent) {
        objectToGlue.transform.parent = parent;
    }
    
}
