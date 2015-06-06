﻿using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Contracts;

public class GameplayManager : MonoBehaviour 
{
    [SerializeField]
    private List<Object> textArenas;

    [SerializeField]
    private List<Texture2D> imageArenas;

    [SerializeField]
    private Arena arena;

    [SerializeField]
    private GameObject mechPrefab;

    public bool IsPlaying { get; private set; }

    public void SetupMatch(IList<ICharacter> characters)
    {
        IsPlaying = true;
        ChooseArena();
        SpawnMechs(characters);
    }

    private void ChooseArena()
    {
        int chosenIndex = Random.Range(0, textArenas.Count + imageArenas.Count);

        if (chosenIndex < textArenas.Count)
        {
            arena.InitFromText(textArenas[chosenIndex].name);
        }
        else
        {
            arena.InitFromTexture(imageArenas[chosenIndex - textArenas.Count]);
        }
    }

    private void SpawnMechs(IList<ICharacter> characters)
    {
        foreach(ICharacter character in characters)
        {
            if(character.InputDevice == null)
            {
                continue;
            }

            GameObject newMech = Instantiate(mechPrefab) as GameObject;
            newMech.GetComponent<PlayerCharacterBehavior>().Character = character;
            newMech.transform.SetParent(arena.transform);

            if (arena.SpawnPoints.Count > 0)
            {
                newMech.transform.localPosition = arena.SpawnPoints[Random.Range(0, arena.SpawnPoints.Count)];
            }
        }
    }
}
