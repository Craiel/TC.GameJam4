using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour 
{
    [SerializeField]
    private List<Object> textArenas;

    [SerializeField]
    private List<Texture2D> imageArenas;

    [SerializeField]
    private Arena arena;

    private List<Vector3> currentSpawnPoints;

    private void Start()
    {
        ChooseArena();
    }

    public void ChooseArena()
    {
        int chosenIndex = Random.Range(0, textArenas.Count + imageArenas.Count);

        if (chosenIndex < textArenas.Count)
        {
            arena.InitFromText(textArenas[chosenIndex].name, out currentSpawnPoints);
        }
        else
        {
            arena.InitFromTexture(imageArenas[chosenIndex - textArenas.Count], out currentSpawnPoints);
        }

        Debug.Log("Spawn Points Count: " + currentSpawnPoints.Count);
    }
}
