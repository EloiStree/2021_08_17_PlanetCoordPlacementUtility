using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractAstronomicDistanceMono : MonoBehaviour, AstronomicDistanceConverter
{
    public abstract void GetAstronomicFromUnityValue(double unityValue, out double asAstronomic);
    public abstract void GetKilometerFromUnityValue(double unityValue, out double asKilometer);
    public abstract void GetLightYearFromUnityValue(double unityValue, out double asLightYear);
    public abstract void GetUnityValueFrom(SpaceDistance spaceDistance, out double unityvalue);
    public abstract void GetUnityValueFromAstronomicValue(double asAstronomic, out double unityValue);
    public abstract void GetUnityValueFromKilometer(double asKilometer, out double unityValue);
    public abstract void GetUnityValueFromLightYear(double asLightYear, out double unityValue);
}

public class UnityAstronomicDistanceMono : AbstractAstronomicDistanceMono, AstronomicDistanceConverter
{
    public const double m_lightYearInKilometer = 9.46e12;
    public const double m_astronomicalInKilometer = 149597870.700;
    public double m_unityUnityAsKilometer=6370;

    [Header("Unit Debug")]
    public double m_oneUnityValueAsLightYear ;
    public double m_oneUnityValueAsAstronomicUnit ;

    [Header("Code Check")]
    // 10km
    public double m_kmTest=10;
    public double m_kmUnity;
    public double m_kmTestBack;
    //6.6846e-8
    public double m_auTest= 6.684587122E-8;
    public double m_auUnity;
    public double m_auTestBack;
    //1.057e-13
    public double m_lyTest= 1.057001e-12;
    public double m_lyUnity;
    public double m_lyTestBack;

    private void OnValidate()
    {

        m_oneUnityValueAsLightYear = m_unityUnityAsKilometer / m_lightYearInKilometer;
        m_oneUnityValueAsAstronomicUnit = m_unityUnityAsKilometer / m_astronomicalInKilometer;


        GetUnityValueFromKilometer(m_kmTest, out m_kmUnity);
        GetUnityValueFromLightYear(m_lyTest, out m_lyUnity);
        GetUnityValueFromAstronomicValue(m_auTest, out m_auUnity);
        GetKilometerFromUnityValue(m_kmUnity, out m_kmTestBack);
        GetLightYearFromUnityValue(m_lyUnity, out m_lyTestBack);
        GetAstronomicFromUnityValue(m_auUnity, out m_auTestBack);
    }


    public override void GetUnityValueFromKilometer(double asKilometer, out double unityValue)
    {
        unityValue = asKilometer / m_unityUnityAsKilometer;
    }
    public override void GetUnityValueFromLightYear(double asLightYear, out double unityValue)
    {
        double asKilometer = asLightYear * m_lightYearInKilometer;
        unityValue = asKilometer / m_unityUnityAsKilometer;
    }
    public override void GetUnityValueFromAstronomicValue(double asAstronomic, out double unityValue)
    {

        double asKilometer = asAstronomic * m_astronomicalInKilometer;
        unityValue = asKilometer / m_unityUnityAsKilometer;
    }
    public override void GetKilometerFromUnityValue(double unityValue , out double asKilometer)
    {
        asKilometer = unityValue * m_unityUnityAsKilometer;
    }
    public override void GetLightYearFromUnityValue(double unityValue , out double asLightYear)
    {
        double asKilometer = unityValue * m_unityUnityAsKilometer;
        asLightYear = asKilometer / m_lightYearInKilometer;
    }
    public override void GetAstronomicFromUnityValue(double unityValue , out double asAstronomic)
    {

        double asKilometer = unityValue * m_unityUnityAsKilometer;
        asAstronomic = asKilometer / m_astronomicalInKilometer;
    }

    public override void GetUnityValueFrom(SpaceDistance spaceDistance, out double unityvalue)
    {
        if (spaceDistance.m_unitType == UnitType.Kilometer)
            GetUnityValueFromKilometer(spaceDistance.m_value, out unityvalue);
        else if (spaceDistance.m_unitType == UnitType.LightYear)
            GetUnityValueFromLightYear(spaceDistance.m_value, out unityvalue);
        else if (spaceDistance.m_unitType == UnitType.AstronomicUnit)
            GetUnityValueFromAstronomicValue(spaceDistance.m_value, out unityvalue);
        else unityvalue = spaceDistance.m_value;
    }

   
}


public interface AstronomicDistanceConverter {

    public void GetUnityValueFromKilometer(double asKilometer, out double unityValue);
    public void GetUnityValueFromLightYear(double asLightYear, out double unityValue);
    public void GetUnityValueFromAstronomicValue(double asAstronomic, out double unityValue);
    public void GetKilometerFromUnityValue(double unityValue, out double asKilometer);
    public void GetLightYearFromUnityValue(double unityValue, out double asLightYear);
    public void GetAstronomicFromUnityValue(double unityValue, out double asAstronomic);
    public void GetUnityValueFrom(SpaceDistance spaceDistance, out double unityvalue);


}
