using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Contracts;
using Assets.Scripts;

public class UIPlayerManager : MonoBehaviour
{
    public enum UIState 
    { 
        NotJoined, 
        Joined, 
        Ready 
    }

    [SerializeField]
    private Text mechName;

    [SerializeField]
    private Text health;

    [SerializeField]
    private Text cooling;

    [SerializeField]
    private Text speed;

    [SerializeField]
    private Text rotationSpeed;

    [SerializeField]
    private Text armor;

    [SerializeField]
    private Text shield;

    [SerializeField]
    private Text rangedAccuracy;

    [SerializeField]
    private Text meleeAccuracy;
    
    [SerializeField]
    private Text targetingDistance;

    [SerializeField]
    private Text targetingLockTime;

    [SerializeField]
    private GameObject joinScreen;

    [SerializeField]
    private GameObject mechSelectionScreen;

    private UIState currentState =UIState.NotJoined;

    private ICharacter character;

    private UIManager UIManager;

    private bool isInitialized;

    private int currentLoadoutIndex;

    public UIState CurrentState { get { return currentState; } }
    public ICharacter Character { get { return character; } }
    
    public void Init(ICharacter character, UIManager UIManager)
    {
        this.character = character;
        this.UIManager = UIManager;
        currentLoadoutIndex = 0;

        UpdateUI();
        UpdateMechSelection();
        isInitialized = true;
    }
    
    // Update is called once per frame
    void Update () 
    {
        if(!isInitialized)
        {
            return;
        }

        UIState previousState = currentState;

        if(currentState == UIState.NotJoined)
        {
            if(character.InputDevice != null)
            {
                currentState = UIState.Joined;
            }
        }
        else if(currentState == UIState.Joined)
        {
            if (character.InputDevice == null)
            {
                currentState = UIState.NotJoined;
            }

            if(character.InputDevice.Action1.WasPressed)
            {
                currentState = UIState.Ready;
            }
            else if(character.InputDevice.DPadRight.WasPressed)
            {
                currentLoadoutIndex++;
                if(currentLoadoutIndex >= MechLoadouts.Loadouts.Count)
                {
                    currentLoadoutIndex = 0;
                }
                UpdateMechSelection();
            }
            else if(character.InputDevice.DPadLeft.WasPressed)
            {
                currentLoadoutIndex--;
                if(currentLoadoutIndex < 0)
                {
                    currentLoadoutIndex = MechLoadouts.Loadouts.Count - 1;
                }
                UpdateMechSelection();
            }
            else if(character.InputDevice.Action2.WasPressed)
            {
                InputManagerBehavior.Instance.DetachDevice(character.InputDevice);
                currentState = UIState.NotJoined;
            }
        }
        else if(currentState == UIState.Ready)
        {
            if(character.InputDevice.Action2.WasPressed)
            {
                currentState = UIState.Joined;
            }
        }

        if(previousState != currentState)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //TODO Update visuals to match state
        switch(currentState)
        {
            case UIState.NotJoined:
                joinScreen.SetActive(true);
                mechSelectionScreen.SetActive(false);
                break;
            case UIState.Joined:
                joinScreen.SetActive(false);
                mechSelectionScreen.SetActive(true);
                break;
            case UIState.Ready:
                joinScreen.SetActive(false);
                mechSelectionScreen.SetActive(false);
                break;
        }
    }

    private void UpdateMechSelection()
    {
        MechLoadouts.MechLoadout loadout = MechLoadouts.Loadouts[currentLoadoutIndex];
        
        mechName.text = loadout.Name;
        health.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.Health].ToString();
        cooling.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.HeatCoolingRate].ToString();
        speed.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.Velocity].ToString();
        rotationSpeed.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.RotationSpeed].ToString();
        armor.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.Armor].ToString();
        shield.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.Shield].ToString();
        rangedAccuracy.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.RangedAccuracy].ToString();
        meleeAccuracy.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.MeleeAccuracy].ToString();
        targetingDistance.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.TargetingDistance].ToString();
        targetingLockTime.text = loadout.BasicStats[Assets.Scripts.Logic.StatType.TargetingLockTime].ToString();
    }
}
