using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaneteryPlacement {

    void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSCoordinate coordinate, GPSCoordinate direction);
    void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalGroundPositioning coordinate);
    void TryToPlaceAround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalInSpacePositioning positioning);
    void TryToPlaceAround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalInSpaceLookAtPositioning positioning);

}

public interface IPlanetInformation {

    void GetRootCenterTransform(out Transform root);
    void GetHighestHeight( out float maxGroundHeigh);
    void GetGroundLayerMask(out LayerMask groundLayerMask);
}


public class PlaneteryPlacementUtilityMono : MonoBehaviour  , IPlaneteryPlacement
{
    public AbstractAstronomicDistanceMono m_astronomicDistance;
    public IPlanetaryHeightScanner m_scannerUsed = new DefaultPlanetaryHeightScanner();




    public void GetHeighestPointOfPlanet(Transform planetCenter, LayerMask layerCounted, float maxRadius, out float minHeight, out float maxHeight) {

        m_scannerUsed.GetHeightOfPlanetary(planetCenter, layerCounted, maxRadius, out minHeight, out maxHeight);
    }

   

    public void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSCoordinate coordinate, GPSCoordinate direction)
    {
        planet.GetRootCenterTransform(out Transform root);
        planet.GetHighestHeight(out float height);
        planet.GetGroundLayerMask(out LayerMask mask);
        m_scannerUsed.GetHeightOfPlanetary(root, mask, height + 0.02f, coordinate, out float heightFound);
        if (heightFound > 0f) {
            GetGlobalPositionOf(planet,new GPSHorizontalGroundPositioning(coordinate, direction), heightFound, out GlobalUnityPosition newPosition);

            objectToMove.transform.position = newPosition.m_where;
            objectToMove.transform.rotation = newPosition.m_orientation;

        }
    }

    public void TryToPlaceHorizontalInSpace(GameObject objectToMove, IPlanetInformation planet, SpaceGPSCoordinate coordinate, SpaceGPSCoordinate direction, Vector3 additionalEulerRotation)
    {
        GetGlobalPositionOf(planet, new GPSHorizontalInSpacePositioning(coordinate, direction, additionalEulerRotation), out GlobalUnityPosition newPosition);

        objectToMove.transform.position = newPosition.m_where;
        objectToMove.transform.rotation = newPosition.m_orientation;

    }

    public void TryToPlaceLookingAtObjectInSpaceFromHorizontalPlane(GameObject objectToMove, IPlanetInformation planet, SpaceGPSCoordinate coordinate, SpaceGPSCoordinate direction, Vector3 additionalEulerRotation)
    {
        GetGlobalPositionOf(planet, new GPSHorizontalInSpaceLookAtPositioning(coordinate, direction, additionalEulerRotation), out GlobalUnityPosition newPosition);

        objectToMove.transform.position = newPosition.m_where;
        objectToMove.transform.rotation = newPosition.m_orientation;
    }

    public void GetLocalPositionOf(GPSHorizontalInSpaceLookAtPositioning coordinate, float highestGroundHeight, out LocalUnityPosition position)
    {
        GetLocalPositionOf(coordinate.m_wheteToPosition, highestGroundHeight, out Vector3 directionOfObjectOnSurface);
        GetLocalPositionOf(coordinate.m_lookingAtPosition, highestGroundHeight, out Vector3 destinationOfObjectOnSurface);
        Quaternion rotation = Quaternion.LookRotation(destinationOfObjectOnSurface- directionOfObjectOnSurface, directionOfObjectOnSurface);
        Quaternion localOrientationFinal = rotation * Quaternion.Euler(coordinate.m_additionalEulerRotation);
        // GetLocalDirectionOf(to, out Quaternion directionOfDestinationOnSurface);
        position = new LocalUnityPosition(directionOfObjectOnSurface, rotation);
    }


    public void GetGlobalPositionOf(IPlanetInformation planet, GPSHorizontalInSpaceLookAtPositioning coordinate, out GlobalUnityPosition position)
    {
        planet.GetHighestHeight(out float height);
        planet.GetRootCenterTransform(out Transform root);
        GetLocalPositionOf(coordinate, height, out LocalUnityPosition localPosition);
        GetGlobalPositionFrom(root, localPosition, out position);
    }
    public void GetGlobalPositionOf(IPlanetInformation planet, GPSHorizontalInSpacePositioning coordinate, out GlobalUnityPosition position)
    {
        planet.GetHighestHeight(out float height);
        planet.GetRootCenterTransform(out Transform root);
        GetLocalPositionOf(coordinate, height, out LocalUnityPosition localPosition);
        GetGlobalPositionFrom(root, localPosition, out position);
    }



    /// <summary>
    /// Convert a local point to a point in Unity based on the origine reference of a transform
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="localPosition"></param>
    /// <param name="globalPosition"></param>
    public void GetGlobalPositionFrom(Transform reference, Vector3 localPosition, out Vector3 globalPosition) {

        localPosition = reference.rotation * localPosition;
        globalPosition = localPosition + reference.position;

    }
    /// <summary>
    /// Convert a local position(V3 and Quat) to a position in Unity from a transform reference point.
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="localPosition"></param>
    /// <param name="globalPosition"></param>
    public void GetGlobalPositionFrom(Transform reference, LocalUnityPosition localPosition, out GlobalUnityPosition globalPosition) {

        Vector3 localPositionTemp = reference.rotation * localPosition.m_where;
        localPositionTemp = localPositionTemp + reference.position;
        Quaternion newQ = reference.rotation * localPosition.m_orientation;
        globalPosition = new GlobalUnityPosition(localPositionTemp, newQ);

    }

    public void GetLocalDirectionOf(GPSCoordinate coordinate, out Vector3 localDirection)
    {
        PlanetaryQuaterionUtility.GetLocalDirectionOf(coordinate, out localDirection);
    }
    public void GetLocalPositionOf(GPSCoordinate coordinate, float groundHeight, out Vector3 localPosition)
    {

        PlanetaryQuaterionUtility.GetLocalDirectionOf(coordinate, out Vector3 direction);
        localPosition = direction * groundHeight;
    }
    public void GetLocalPositionOf(SpaceGPSCoordinate coordinate, float highestGroundHeight, out Vector3 localPosition)
    {
        PlanetaryQuaterionUtility.GetLocalDirectionOf(coordinate, out Vector3 direction);
        SpaceDistance sd = coordinate.GetAltitudeDistance();
        m_astronomicDistance.GetUnityValueFrom(sd, out double altitude);
        localPosition = direction * (highestGroundHeight + (float) altitude) ;
    }
   
    public void GetLocalPositionOf(GPSHorizontalInSpacePositioning coordinate, float highestGroundHeight, out LocalUnityPosition position)
    {
        
        SpaceGPSCoordinate start = coordinate.m_wheteToPosition ;
        GetLocalPositionOf(start, highestGroundHeight, out Vector3 positionOfStart);
        PlanetaryQuaterionUtility.GetSurfaceDirection( coordinate.m_wheteToPosition, coordinate.m_lookingTowardPosition, out Quaternion surfaceOrientation);
        Quaternion localOrientationFinal = surfaceOrientation * Quaternion.Euler(coordinate.m_additionalEulerRotation);
        position = new LocalUnityPosition(positionOfStart, localOrientationFinal);
    }

    public void GetGlobalPositionOf(IPlanetInformation planet, GPSHorizontalGroundPositioning coordinate, float groundHeightForHorizonStart, out GlobalUnityPosition position) {

        planet.GetRootCenterTransform(out Transform root);
        GetLocalPositionOf(coordinate, groundHeightForHorizonStart, out LocalUnityPosition newPosition);
        GetGlobalPositionFrom(root, newPosition, out position);
    }
    public void GetLocalPositionOf(GPSHorizontalGroundPositioning coordinate, float groundHeightForHorizonStart, out LocalUnityPosition position)
    {
            GetLocalPositionOf(coordinate.m_wheteToPosition, groundHeightForHorizonStart, out Vector3 gpsPosition);
            PlanetaryQuaterionUtility.GetSurfaceDirection( coordinate.m_wheteToPosition,coordinate.m_lookingTowardPosition, out Quaternion surfaceOrientation);
            position = new LocalUnityPosition(gpsPosition, surfaceOrientation);
    }

    

    public void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalGroundPositioning coordinate)
    {
        TryToPlaceOnGround(objectToMove, planet, coordinate.m_wheteToPosition, coordinate.m_lookingTowardPosition);
    }

    public void TryToPlaceAround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalInSpacePositioning positioning)
    {
        TryToPlaceHorizontalInSpace(objectToMove, planet, positioning.m_wheteToPosition, positioning.m_lookingTowardPosition, positioning.m_additionalEulerRotation);
    }
    public void TryToPlaceAround(GameObject objectToMove, IPlanetInformation planet, GPSHorizontalInSpaceLookAtPositioning positioning)
    {
        TryToPlaceLookingAtObjectInSpaceFromHorizontalPlane(objectToMove, planet, positioning.m_wheteToPosition, positioning.m_lookingAtPosition, positioning.m_additionalEulerRotation);
    }


}

public interface IPlanetaryHeightScanner {
    void GetHeightOfPlanetary(Transform planetCenter, LayerMask layerCounted, float maxRadiusScan, out float minHeight, out float maxHeight);
    void GetHeightOfPlanetary(Transform planetCenter, LayerMask layerCounted, float maxRadiusScan, GPSCoordinate coordinate, out float height);
}



public class DefaultPlanetaryHeightScanner : IPlanetaryHeightScanner
{

    public void GetHeightOfPlanetary(Transform planetCenter, LayerMask layerCounted, float maxRadiusScan, out float minHeight, out float maxHeight)
    {
        GPSCoordinate coordinate = new GPSCoordinate();
        minHeight = float.MaxValue;
        maxHeight = 0;
        for (int i = -180; i < 180; i += 10)
        {
            coordinate.SetLatitude(i);
            for (int j = -90; j < 90; j+=10)
            {
                coordinate.SetLongitude(j);
                GetHeightOfPlanetary(planetCenter, layerCounted, maxRadiusScan, coordinate, out float h);
                if (h > maxHeight)
                    maxHeight = h;
                if (h < minHeight)
                    minHeight = h;
            }
        }

    }
    public void GetHeightOfPlanetary(Transform planetCenter, LayerMask layerCounted, float maxRadiusScan, GPSCoordinate coordinate, out float height)
    {
        PlanetaryQuaterionUtility.GetLocalDirectionOf(coordinate, planetCenter, out Vector3 direction);
        Vector3 origine = planetCenter.position, end = planetCenter.position + direction* maxRadiusScan;
     
        height = 0;
       
            Debug.DrawLine(origine, end);
            if (Physics.Raycast(end, 
                direction*-1
                , out RaycastHit hit
                , maxRadiusScan
                , layerCounted))
            {
                height = Vector3.Distance(planetCenter.position, hit.point);
                 Debug.DrawLine(origine, hit.point, Color.red);
            }
           


        
    }

}

    

    public class PlanetaryQuaterionUtility
{
    public static void GetLocalDirectionOf(GPSCoordinate coordinate, out Vector3 direction) {

        GetLocalDirectionOf(coordinate, out Quaternion orientation);
        direction = orientation*Vector3.forward;
    }
    public static void GetLocalDirectionOf(GPSCoordinate coordinate, out Quaternion orientation) { 
    
        orientation= Quaternion.Euler(-(float)coordinate.GetLongitude(), -(float)coordinate.GetLatitude(), 0); 
    }

    public static void GetLocalDirectionOf(GPSCoordinate coordinate, Transform refPoint, out Vector3 direction)
    {
        if (refPoint == null) { 
            GetLocalDirectionOf(coordinate, out direction);
        }
        else { 
            GetLocalDirectionOf(coordinate, out Quaternion orientation);
            direction = (refPoint.rotation * orientation) * Vector3.forward;
        }
    }

    public static void GetSurfaceDirection(Transform root, GPSCoordinate from, GPSCoordinate to, out Quaternion surfaceOrientation)
    {
        GetLocalDirectionOf(from, root, out Vector3 directionOfObjectOnSurface);
        GetLocalDirectionOf(to, root, out Vector3 destinationOfObjectOnSurface);
        Vector3 whereToLook = Vector3.ProjectOnPlane(destinationOfObjectOnSurface, directionOfObjectOnSurface);
        surfaceOrientation = Quaternion.LookRotation(whereToLook, directionOfObjectOnSurface);
        // GetLocalDirectionOf(to, out Quaternion directionOfDestinationOnSurface);
    }
    public static void GetSurfaceDirection(GPSCoordinate from, GPSCoordinate to, out Quaternion surfaceOrientation)
    {
        GetLocalDirectionOf(from, out Vector3 directionOfObjectOnSurface);
        GetLocalDirectionOf(to, out Vector3 destinationOfObjectOnSurface);
        Vector3 whereToLook = Vector3.ProjectOnPlane(destinationOfObjectOnSurface, directionOfObjectOnSurface);
        surfaceOrientation = Quaternion.LookRotation(whereToLook, directionOfObjectOnSurface);
        // GetLocalDirectionOf(to, out Quaternion directionOfDestinationOnSurface);
    }
   
}

