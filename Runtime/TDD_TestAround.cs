using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_TestAround : MonoBehaviour
{

    public GPSCoordinate m_coordinate;
    public GPSCoordinate m_coordinateOrientation;
    public Transform m_testTarget;
    public LayerMask m_masks ;
    public float m_maxRadius=1000;
    public float m_gpsHeight;
    public float m_maxHeight;
    public float m_minHeight;
    public Renderer m_meshTarget;

    public IPlanetaryHeightScanner m_scanner = new DefaultPlanetaryHeightScanner() {};


    public PlaneteryPlacementUtilityMono m_placement;
    public GameObject m_objectToPlace;
    public GameObject m_objectToPlaceDestination;
    public PlanetInformationAsMesh m_planetInformation;

    private void Start()
    {
    }

    void Update()
    {
        m_placement.TryToPlaceOnGround(m_objectToPlace, m_planetInformation, m_coordinate, m_coordinateOrientation);
        m_placement.TryToPlaceOnGround(m_objectToPlaceDestination, m_planetInformation, m_coordinateOrientation, m_coordinate);

        //PlanetaryQuaterionUtility.GetLocalDirectionOf(m_coordinate, m_testTarget, out Vector3 direction);
        //Debug.DrawLine(m_testTarget.position, m_testTarget.position+ direction, Color.red) ;
        //float h = m_meshTarget.bounds.size.magnitude / 2f ;
        //m_scanner.GetHeightOfPlanetary(m_testTarget, m_masks, h, m_coordinate, out m_gpsHeight);
        //m_scanner.GetHeightOfPlanetary(m_testTarget, m_masks, h, out m_minHeight, out m_maxHeight);
    }
}
