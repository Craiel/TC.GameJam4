using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic;
using Assets.Scripts.Contracts;
using System.Collections.Generic;
using Assets.Scripts.Logic.Enums;
using System;

public class GearGenerationTest : MonoBehaviour
{
    [SerializeField]
    private int numGearToGenerate;

    private Dictionary<GearType, int> gearOutputs = new Dictionary<GearType, int>();

    private void Start()
    {
        foreach(GearType gearType in Enum.GetValues(typeof(GearType)))
        {
            gearOutputs.Add(gearType, 0);
        }

        for(int i=0; i<numGearToGenerate; ++i)
        {
            IGear gear = GearGeneration.GenerateRandomGear();
            gearOutputs[gear.Type]++;
        }

        Debug.Log("\n\n=====================");

        foreach (GearType gearType in Enum.GetValues(typeof(GearType)))
        {
            Debug.Log(gearType + ": " + gearOutputs[gearType]);
        }
    }
}