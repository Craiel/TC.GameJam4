using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

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

    private void Start()
    {
        SetupMatch();
    }

    public void SetupMatch()
    {
        ChooseArena();
        SpawnMechs();
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

    private void SpawnMechs()
    {
        GameObject newMech = Instantiate(mechPrefab) as GameObject;
        //TODO: Initialize mech with ICharacter data

        newMech.transform.SetParent(arena.transform);
        if(arena.SpawnPoints.Count > 0)
        {   
            newMech.transform.localPosition = arena.SpawnPoints[Random.Range(0, arena.SpawnPoints.Count)];
        }
    }
}