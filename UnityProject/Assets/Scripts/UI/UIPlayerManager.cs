using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Contracts;

public class UIPlayerManager : MonoBehaviour {
    private enum UIState { NotJoined, Joined, Ready }
    private UIState currentState =UIState.NotJoined;
    public ICharacter character;
    public GameObject mechPreview;
    public GameObject mechStats;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switch (currentState)
            {
                case UIState.NotJoined:
                    currentState = UIState.Joined;
                    break;
                case UIState.Joined:
                    currentState = UIState.Ready;
                    break;
                case UIState.Ready:
                    break;
            }
        }
        //Dpad controls for Player 1
        if (Input.GetKeyDown(KeyCode.Q) && currentState == UIState.Joined)
        {
            mechPreview.GetComponent<Image>().color = new Color(0f, 0f, 0f);
        }
    
    }
}
