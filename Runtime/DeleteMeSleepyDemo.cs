using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMeSleepyDemo : MonoBehaviour
{
    public GluePrefabToPlanetsExperiment m_glueToPlanet;

    public string m_targetPlanet="earth";
    public GameObject m_basePrefab;
    public GPSHorizontalGroundPositioning m_ground;

    public GameObject m_statelitPrefab;
    public GPSHorizontalInSpacePositioning m_horizontalStatelit;
    public GPSHorizontalInSpaceLookAtPositioning m_lookAtStatelit;


    void Start()
    {
        if(m_glueToPlanet.DoesPlanetExists(m_targetPlanet))
        { 
            GameObject b = GameObject.Instantiate(m_basePrefab);
            m_glueToPlanet.TryToGlue(b, m_ground, m_targetPlanet);

            b = GameObject.Instantiate(m_statelitPrefab);
            m_glueToPlanet.TryToGlue(b, m_horizontalStatelit, m_targetPlanet);

            b = GameObject.Instantiate(m_statelitPrefab);
            m_glueToPlanet.TryToGlue(b, m_lookAtStatelit, m_targetPlanet);
        }



    }

}
