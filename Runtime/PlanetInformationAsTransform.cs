using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInformationAsTransform
    : AbstractPlanetInformation, IPlanetInformation
{
    public Transform m_root;
    public Transform m_heightPointOfGround;
    public LayerMask m_groundLayerMaskToUse;
    public override void GetHighestHeight(out float maxGroundHeigh)
    {
        maxGroundHeigh = Vector3.Distance(m_root.position, m_heightPointOfGround.position);
    }

    public override void GetRootCenterTransform(out Transform root)
    {
        root = m_root;
    }
    public override void GetGroundLayerMask(out LayerMask groundLayerMask)
    {
        groundLayerMask = m_groundLayerMaskToUse;

    }
}
