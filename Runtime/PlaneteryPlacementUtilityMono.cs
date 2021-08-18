using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaneteryPlacement {

    void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSCoordinate coordinate, GPSCoordinate direction);
    void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, SpaceGPSCoordinate coordinate, GPSCoordinate direction);

}

public interface IPlanetInformation {

    void GetRootCenterTransform(out Transform root);
    void GetHighestHeight( out float maxGroundHeigh);
    void GetGroundLayerMask(out LayerMask groundLayerMask);
}


public class PlaneteryPlacementUtilityMono : MonoBehaviour  , IPlaneteryPlacement
{

    public IPlanetaryHeightScanner scannerUsed = new DefaultPlanetaryHeightScanner();




    public void GetHeighestPointOfPlanet(Transform planetCenter, LayerMask layerCounted, float maxRadius, out float minHeight, out float maxHeight) {

        scannerUsed.GetHeightOfPlanetary(planetCenter, layerCounted, maxRadius, out minHeight, out maxHeight);
    }



    public void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, GPSCoordinate coordinate, GPSCoordinate direction)
    {
        planet.GetRootCenterTransform(out Transform root);
        planet.GetHighestHeight(out float height);
        planet.GetGroundLayerMask(out LayerMask mask);
        scannerUsed.GetHeightOfPlanetary(root, mask, height + 0.02f, coordinate, out float heightFound);
        if (heightFound > 0f) {

            PlanetaryQuaterionUtility.GetLocalDirectionOf(coordinate, root, out Vector3 gpsDirection);
            Vector3 position = root.position + gpsDirection * heightFound;
            objectToMove.transform.position = position;
            PlanetaryQuaterionUtility.GetSurfaceDirection(root,  coordinate,  direction, out Quaternion surfaceOrientation);
            objectToMove.transform.rotation = surfaceOrientation;

        }
    }

    public void TryToPlaceOnGround(GameObject objectToMove, IPlanetInformation planet, SpaceGPSCoordinate coordinate, GPSCoordinate direction)
    {
        throw new NotImplementedException();
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

    public static void GetSurfaceDirection(Transform root,  GPSCoordinate from, GPSCoordinate to, out Quaternion surfaceOrientation)
    {
        GetLocalDirectionOf(from, root, out Vector3 directionOfObjectOnSurface);
        GetLocalDirectionOf(to, root, out Vector3 destinationOfObjectOnSurface);
        Vector3 whereToLook=  Vector3.ProjectOnPlane(destinationOfObjectOnSurface, directionOfObjectOnSurface) ;
        surfaceOrientation = Quaternion.LookRotation(whereToLook, directionOfObjectOnSurface);
       // GetLocalDirectionOf(to, out Quaternion directionOfDestinationOnSurface);
    }
}

