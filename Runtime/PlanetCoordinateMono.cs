using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCoordinateMono : MonoBehaviour
{

    public Transform m_orientationRef;
    //Not the best solution but for quick proto;
    public SphereCollider m_planetSphereCollider;
    public UnityAstronomicDistanceMono m_unityScale;

    public void Glue(GameObject objectToGlue, GPSCoordinate coordinate)
    {

        float longitude = (float)coordinate.GetLongitude();
        float latitude = (float)coordinate.GetLatitude();
        PutTheObjectOnThePlanetGroundWithGoodRotation(objectToGlue, longitude, latitude);




        //objectToGlue.transform.parent = m_orientationRef;
        //Quaternion objectOrientaiton = Quaternion.Euler((float)coordinate.GetLongitude(), (float)coordinate.GetLatitude(), 0);
        //objectToGlue.transform.localRotation = objectOrientaiton;
        //objectToGlue.transform.position += objectOrientaiton * Vector3.forward * GetHighestHeight() * 0.51f;
    }

    private void PutTheObjectOnThePlanetGroundWithGoodRotation(GameObject objectToGlue, float longitude, float latitude)
    {
        GPSCoordinate coord = new GPSCoordinate(longitude, latitude);
        objectToGlue.transform.position = m_orientationRef.position;
        Quaternion planetOrientation = m_orientationRef.rotation;
        PlanetaryQuaterionUtility.
            GetLocalDirectionOf(coord, out Quaternion objectOrientaiton);

        objectOrientaiton = planetOrientation * objectOrientaiton;
        //objectOrientaiton = objectOrientaiton* planetOrientation  ;

        //Debug.DrawLine(m_orientationRef.transform.position + Vector3.zero, m_orientationRef.transform.position + (objectOrientaiton * Vector3.forward * GetHighestHeight()), Color.red, 2);
        objectToGlue.transform.rotation = objectOrientaiton;
        objectToGlue.transform.position += objectOrientaiton * Vector3.forward * GetHighestHeight();
        objectToGlue.transform.Rotate(90, 0, 0, Space.Self);
    }

    private float GetHighestHeight()
    {
        return m_planetSphereCollider.radius;
    }

    public void Glue(GameObject objectToGlue, SpaceGPSCoordinate coordinate) {


        float longitude = (float)coordinate.GetLongitude();
        float latitude = (float)coordinate.GetLatitude();
        PutTheObjectOnThePlanetGroundWithGoodRotation(objectToGlue, longitude, latitude);

        // Use AstronomicUnityConverter To convert wanted height to unity value;
        m_unityScale.GetUnityValueFrom(coordinate.GetAltitudeDistance(), out double unityvalue);
        float heightInUnityValue= (float)unityvalue;


        objectToGlue.transform.Translate(
            new Vector3(0, heightInUnityValue, 0), Space.Self);

    }
    public void Glue(GameObject objectToGlue, MovingGPSCoordinate coordinate) { 
    
    }


}



[System.Serializable]
public class GPSCoordinate
{
    public Longitude m_longitude= new Longitude();
    public Latitude m_latitude= new Latitude();
    public GPSCoordinate()
    {    }
    public GPSCoordinate(double longitude, double latitude)
    {
        m_longitude =new Longitude ( longitude );
        m_latitude = new Latitude ( latitude );
    }
    public GPSCoordinate(Longitude longitude, Latitude latitude)
    {
        m_longitude = longitude;
        m_latitude = latitude;
    }

    public double GetLatitude()
    {
        m_latitude.GetAngleClampAs180To180(out double value);
        return value;


    }

    public double GetLongitude()
    {
        m_longitude.GetAngleClampAs90To90(out double value);
        return value;
    }

    public void SetLatitude(double value)
    {
        m_latitude.SetAngle(value);
    }

    public void SetLongitude(double value)
    {
        m_longitude.SetAngle(value);
    }
}
[System.Serializable]
public class SpaceGPSCoordinate : GPSCoordinate
{
    public SpaceDistance m_heightFromTopMountain;

    public SpaceDistance GetAltitudeDistance() { return m_heightFromTopMountain; }
   
}

[System.Serializable]
public class MovingGPSCoordinate 
{
    public SpaceGPSCoordinate m_initialGPSCoordinate;
    public GPSCoordinate m_direction;
    public Angle m_angluarSpeed;
}

[System.Serializable]
public class SolarCoordinate
{
    public SpaceDistance m_x;
    public SpaceDistance m_y;
    public SpaceDistance m_z;
}
public enum UnitType { LightYear, Kilometer, AstronomicUnit , UnityValue}

[System.Serializable]
public struct SpaceDistance {
    public UnitType m_unitType;
    public double m_value;

}

public class AstronomicUnityConverter {

    public static void Convert(double kilometerValueOf1Unity, double givenValue, UnitType givenType, UnitType wantedType, out double convertValue) {
        //TODO LATER WHEN I AM NOT SLEEPY
        throw new NotImplementedException();
    }

}

[System.Serializable]
public class  Angle
{
    [SerializeField] protected double m_angle;
    public void GetAngle(out double angle) {
        angle = m_angle;
    }
    internal void SetAngle(double value)
    {
        m_angle = value;
    }
}
[System.Serializable]
public class Longitude : Angle
{
    public Longitude()
    {
        this.m_angle = 0;
    }
    public Longitude(double longitude)
    {
        this.m_angle = longitude;
    }

    public void GetAngleClampAs90To90(out double value90To90)
    {

        if (m_angle < -90.0) value90To90 = -90.0;
        else if (m_angle > 90.0) value90To90 = 90.0;
        else value90To90 = m_angle;
    }
}
[System.Serializable]
public class Latitude : Angle
{
    public Latitude()
    {
        this.m_angle = 0;
    }
    public Latitude(double latitude)
    {
        this.m_angle = latitude;
    }

    public void GetAngleClampAs180To180(out double value180To180) {

        if (m_angle < -180.0) value180To180= -180.0;
        else if (m_angle > 180.0) value180To180= 180.0;
        else value180To180 = m_angle;
    }

   
}