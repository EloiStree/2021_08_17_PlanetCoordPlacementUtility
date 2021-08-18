using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInformationAsMesh : MonoBehaviour, IPlanetInformation
{
    public Transform m_root;
    public LayerMask m_groundLayer;
    public Renderer m_renderer;
    public float m_maxHeight;
    public float m_minHeight;

    private void Reset()
    {
        int layerMask = 1 << this.gameObject.layer;
        CheckHeight();
    }
    [ContextMenu("Check Heights")]
    private void CheckHeight()
    {
        DefaultPlanetaryHeightScanner scanner = new DefaultPlanetaryHeightScanner();
        float h = m_renderer.bounds.size.magnitude / 2f;
        scanner.GetHeightOfPlanetary(m_root, m_groundLayer, h, out m_minHeight, out m_maxHeight);
    }

    public void GetHighestHeight(out float maxGroundHeigh)
    {
        maxGroundHeigh = m_maxHeight;
    }

    public void GetRootCenterTransform(out Transform root)
    {
        root = m_root;
    }

    public void GetGroundLayerMask(out LayerMask groundLayerMask)
    {
        groundLayerMask = m_groundLayer;

    }
}
