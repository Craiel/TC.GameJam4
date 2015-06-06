using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Contracts;

public class UIManager : MonoBehaviour 
{
    [SerializeField]
    private GameplayManager gameplayManager;

    [SerializeField]
    private GameObject playerSelect;

    public List<UIPlayerManager> players;
    private IList<ICharacter> characters;

    public void Init(IList<ICharacter> characters)
    {   
        for (var i = 0; i < characters.Count; i++)
        {
            players[i].Init(characters[i], this);
        }
    }

    private void Update()
    {
        if(!gameplayManager.IsPlaying)
        {
            List<ICharacter> characters = new List<ICharacter>();
            foreach(UIPlayerManager playerManager in players)
            {
                if(playerManager.CurrentState == UIPlayerManager.UIState.Joined)
                {
                    return;
                }
                if(playerManager.CurrentState == UIPlayerManager.UIState.Ready)
                {
                    characters.Add(playerManager.Character);
                }
            }

            //TODO: Make this actually 2, dummy
            if(characters.Count > 0)
            {
                gameplayManager.SetupMatch(characters);
                playerSelect.SetActive(false);
            }
        }
    }
}