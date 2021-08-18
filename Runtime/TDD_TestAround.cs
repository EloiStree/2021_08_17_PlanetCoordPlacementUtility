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



    public GPSHorizontalInSpacePositioning m_coordinateSatelite;
    public GameObject m_sateliteToPlace;
    public GameObject m_sateliteToPlaceDestination;

    public GPSHorizontalInSpaceLookAtPositioning m_coordinateSateliteLooktAt;
    public GameObject m_sateliteToPlaceLootAt;
    public GameObject m_sateliteToPlaceDestinationLootAt;



    void Update()
    {
        m_placement.TryToPlaceOnGround(m_objectToPlace, m_planetInformation, m_coordinate, m_coordinateOrientation);
        m_placement.TryToPlaceOnGround(m_objectToPlaceDestination, m_planetInformation, m_coordinateOrientation, m_coordinate);

        GPSHorizontalInSpacePositioning iTmp = new GPSHorizontalInSpacePositioning(m_coordinateSatelite.m_lookingTowardPosition, m_coordinateSatelite.m_wheteToPosition);
        m_placement.TryToPlaceAround(m_sateliteToPlace, m_planetInformation, m_coordinateSatelite);
        m_placement.TryToPlaceAround(m_sateliteToPlaceDestination, m_planetInformation, iTmp);

        GPSHorizontalInSpaceLookAtPositioning iiTmp = new GPSHorizontalInSpaceLookAtPositioning(m_coordinateSateliteLooktAt.m_lookingAtPosition, m_coordinateSateliteLooktAt.m_wheteToPosition);
        m_placement.TryToPlaceAround(m_sateliteToPlaceLootAt, m_planetInformation, m_coordinateSateliteLooktAt);
        m_placement.TryToPlaceAround(m_sateliteToPlaceDestinationLootAt, m_planetInformation, iiTmp);



        //m_placement.TryToPlaceHorizontalInSpace(m_sateliteToPlace, m_planetInformation,  m_coordinateSatelite.m_lookingTowardPosition, m_coordinateSatelite.m_wheteToPosition);

        //PlanetaryQuaterionUtility.GetLocalDirectionOf(m_coordinate, m_testTarget, out Vector3 direction);
        //Debug.DrawLine(m_testTarget.position, m_testTarget.position+ direction, Color.red) ;
        //float h = m_meshTarget.bounds.size.magnitude / 2f ;
        //m_scanner.GetHeightOfPlanetary(m_testTarget, m_masks, h, m_coordinate, out m_gpsHeight);
        //m_scanner.GetHeightOfPlanetary(m_testTarget, m_masks, h, out m_minHeight, out m_maxHeight);
    }
}
