﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;

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
            Dictionary<ICharacter, MechLoadouts.MechLoadout> characters = new Dictionary<ICharacter, MechLoadouts.MechLoadout>();
            foreach(UIPlayerManager playerManager in players)
            {
                if(playerManager.CurrentState == UIPlayerManager.UIState.Joined)
                {
                    return;
                }
                if(playerManager.CurrentState == UIPlayerManager.UIState.Ready)
                {
                    characters.Add(playerManager.Character, playerManager.Loadout);
                }
            }

            //TODO: Make this actually 2, dummy
            if(characters.Count > 0)
            {
                foreach(ICharacter character in characters.Keys)
                {
                    character.SetBaseStats(characters[character].BasicStats);
                }

                gameplayManager.SetupMatch(new List<ICharacter>(characters.Keys));
                playerSelect.SetActive(false);
            }
        }
    }
}