using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Contracts;

public class UIPlayerManager : MonoBehaviour
{
    public enum UIState 
    { 
        NotJoined, 
        Joined, 
        Ready 
    }

    private UIState currentState =UIState.NotJoined;
    
    public GameObject mechPreview;
    public GameObject mechStats;

    private ICharacter character;

    private UIManager UIManager;

    private bool isInitialized;

    public UIState CurrentState { get { return currentState; } }
    public ICharacter Character { get { return character; } }
    
    public void Init(ICharacter character, UIManager UIManager)
    {
        this.character = character;
        this.UIManager = UIManager;
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
                //TODO: Move right in selection menu
            }
            else if(character.InputDevice.DPadLeft.WasPressed)
            {
                //TODO: Move left in selection menu
            }
            else if(character.InputDevice.Action2.WasPressed)
            {
                character.InputDevice = null;
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
                break;
            case UIState.Joined:
                break;
            case UIState.Ready:
                break;
        }
    }
}
