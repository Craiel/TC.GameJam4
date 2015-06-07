﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic.Enums;

public class GearSlot : MonoBehaviour 
{
    [SerializeField]
    private Text hull;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image heat;

    [SerializeField]
    private Image selectionBox;

    [SerializeField]
    private GearType gearType;

    private bool isInitialized;
    private ICharacter character;

    IGear currentGear;

    public void Init(ICharacter character)
    {
        this.character = character;
        isInitialized = true;
        SetSelected(false);
        Update();
    }

    public void SetSelected(bool isSelected)
    {
        selectionBox.enabled = isSelected;
    }

    private void Update()
    {
        if(isInitialized)
        {
            IGear gear = character.GetGear(gearType);

            if(currentGear != gear)
            {
                currentGear = gear;

                bool isGearAssigned = (currentGear != null);
                icon.enabled = isGearAssigned;
                hull.enabled = isGearAssigned;
                heat.enabled = isGearAssigned;
                selectionBox.enabled = isGearAssigned;
            }

            if(currentGear != null)
            {
                hull.text = (int)gear.GetCurrentStat(StatType.Health) + "/" + (int)gear.GetMaxStat(StatType.Health);

                if(!Mathf.Approximately(gear.GetMaxStat(StatType.Heat), 0f))
                {
                    heat.fillAmount = gear.GetCurrentStat(StatType.Heat) / gear.GetMaxStat(StatType.Heat);
                    
                    if(currentGear.IsOverheated)
                    {   
                        heat.GetComponent<Animator>().enabled = true;
                    }
                    else
                    {
                        heat.GetComponent<Animator>().enabled = false;
                        heat.GetComponent<CanvasGroup>().alpha = 1f;
                    }
                }
                else
                {
                    heat.fillAmount = 0;
                }
            }
        }
    }
}