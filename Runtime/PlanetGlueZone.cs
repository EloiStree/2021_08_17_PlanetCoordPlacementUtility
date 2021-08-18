using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGlueZone : MonoBehaviour
{
    [SerializeField] Transform m_transformUseToGlueObject;

    public void GetTransformToLinkTo(out Transform transformTargeted) {
       transformTargeted = m_transformUseToGlueObject;
    }
    public void GlueToThisPlanet(Transform givenTransform) {
        givenTransform.parent = m_transformUseToGlueObject;
    }

     void Reset()
    {
        m_transformUseToGlueObject = this.transform;
    }
}
