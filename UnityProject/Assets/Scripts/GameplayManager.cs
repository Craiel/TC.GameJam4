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

    private void Start()
    {
        ChooseArena();
    }

    public void ChooseArena()
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
}
