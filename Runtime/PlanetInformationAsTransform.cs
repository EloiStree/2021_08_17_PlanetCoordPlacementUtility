using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInformationAsTransform
    : MonoBehaviour, IPlanetInformation
{
    public Transform m_root;
    public Transform m_heightPointOfGround;
    public LayerMask m_groundLayerMaskToUse;
    public void GetHighestHeight(out float maxGroundHeigh)
    {
        maxGroundHeigh = Vector3.Distance(m_root.position, m_heightPointOfGround.position);
    }

    public void GetRootCenterTransform(out Transform root)
    {
        root = m_root;
    }
    public void GetGroundLayerMask(out LayerMask groundLayerMask)
    {
        groundLayerMask = m_groundLayerMaskToUse;

    }
}
