using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Contracts;

public class UIManager : MonoBehaviour {
    private enum State { NotReady, Ready}
    private State currentState = State.NotReady;
    public List<UIPlayerManager> players;
    private IList<ICharacter> characters;
    // Use this for initialization
    void Start () 
    {
        characters = InputManagerBehavior.Instance.GetCharacters();
        for (var i = 0; i < characters.Count; i++)
        {
            players[i].character = characters[i];
        }
    }
    
    // Update is called once per frame
    void Update () 
    {

    }
}
