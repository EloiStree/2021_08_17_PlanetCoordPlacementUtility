using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_GlueSomeStuffs : MonoBehaviour
{
    public PlanetCoordinateMono m_planet;
    public ToGlueOnGround m_toGlueBuilding;
    public ToGlueSatellite m_toGlueSatellite;

    [System.Serializable]
    public class ToGlueOnGround
    {
        public GPSCoordinate m_coordinate;
        public GameObject m_instance;
    }
    [System.Serializable]
    public class ToGlueSatellite
    {
        public SpaceGPSCoordinate m_coordinate;
        public GameObject m_instance;
    }


    private void OnValidate()
    {
        if (m_planet != null && m_toGlueBuilding.m_instance != null)
            m_planet.Glue(m_toGlueBuilding.m_instance, m_toGlueBuilding.m_coordinate);

        if (m_planet != null && m_toGlueSatellite.m_instance != null)
            m_planet.Glue(m_toGlueSatellite.m_instance, m_toGlueSatellite.m_coordinate);
    }

}
