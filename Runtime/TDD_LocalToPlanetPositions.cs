using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_LocalToPlanetPositions : MonoBehaviour
{

    public PlaneteryPlacementUtilityMono m_placementManager;

    public PlanetInformationAsTransform m_planetTarget;

    [Space(5)]
    public Vector3 m_localPointToTarget;

    [Space(5)] 
    public Transform m_localTransform;
    public Transform m_gizmoRelocationLocalToPlanet;
  



    void Update()
    {
        m_planetTarget.GetRootCenterTransform(out Transform root);
        Vector3 planetPosition = root.position;
        m_placementManager.GetGlobalPositionFrom(root, m_localPointToTarget, out Vector3 newPositionLocalPoint);

        Debug.DrawLine(Vector3.zero, m_localPointToTarget, Color.red);
        Debug.DrawLine(planetPosition, newPositionLocalPoint, Color.red);

        m_placementManager.GetGlobalPositionFrom(root, new LocalUnityPosition(m_localTransform.position, m_localTransform.rotation), out GlobalUnityPosition newPosition);
        m_gizmoRelocationLocalToPlanet.position = newPosition.m_where;
        m_gizmoRelocationLocalToPlanet.rotation = newPosition.m_orientation;


    }
}
