using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Contracts;
using UnityEngine.UI;

public class PlayerGamePanel : MonoBehaviour
{
    [SerializeField]
    private List<GearSlot> gearSlots;

    [SerializeField]
    private Text rangedAccuracy;

    [SerializeField]
    private Text meleeAccuracy;

    [SerializeField]
    private Text cooling;

    [SerializeField]
    private Text armor;

    [SerializeField]
    private Text shield;

    [SerializeField]
    private Text hull;

    private ICharacter character;

    private bool isInitialized;

    public void Init(ICharacter character)
    {
        this.character = character;
        foreach (GearSlot gearSlot in gearSlots)
        {
            gearSlot.Init(character);
        }
        isInitialized = true;
    }

    private void Update()
    {
        if(isInitialized)
        {
            foreach(GearSlot gearSlot in gearSlots)
            {
                gearSlot.UpdateUI();
            }
        }

        rangedAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.RangedAccuracy)*100) + "%";
        meleeAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.MeleeAccuracy) * 100) + "%";
        cooling.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.HeatCoolingRate) + "/s";
        armor.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Armor)).ToString();
        shield.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Shield)).ToString();
        hull.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Health) + "/" + (int)character.GetMaxStat(Assets.Scripts.Logic.Enums.StatType.Health);
    }
}