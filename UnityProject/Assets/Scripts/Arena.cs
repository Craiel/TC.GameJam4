﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class Arena : MonoBehaviour 
{
    [SerializeField]
    private List<Color> tileColorKeys;

    [SerializeField]
    private List<GameObject> tiles;

    public void InitFromText(string fileName, out List<Vector3> spawnPoints)
    {
        List<Vector3> foundSpawns = new List<Vector3>();
        StreamReader streamReader = new StreamReader(Application.dataPath + "/Resources/Maps/" + fileName + ".txt");

        string indestructible = "x";
        string empty = "o";
        string destructible = "d";
        string half = "h";
        string spawn = "s";

        int i = 0;

        string line;
        using (streamReader)
        {
            do
            {
                line = streamReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    for(int j=0; j<line.Length; ++j)
                    {
                        string character = line.Substring(j, 1);
                        int tileIndex = 0;
                        if(character == indestructible)
                        {
                            tileIndex = 1;
                        }
                        else if(character == destructible)
                        {
                            tileIndex = 2;
                        }
                        else if(character == half)
                        {
                            tileIndex = 3;
                        }
                        else if(character == spawn)
                        {
                            foundSpawns.Add(new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f));
                        }

                        GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;

                        tile.transform.SetParent(this.transform);
                        tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);
                    }

                    i++;
                }

            }
            while (line != null);
        }

        spawnPoints = foundSpawns;
    }
    
    public void InitFromTexture(Texture2D texture, out List<Vector3> spawnPoints)
    {
        List<Vector3> foundSpawns = new List<Vector3>();

        Vector2 cellDimensions = new Vector2(35, 35);
        Vector2 mapDimensions = new Vector2(texture.width, texture.height);

        int currentIndex = 0;
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < mapDimensions.x; i++)
        {
            for(int j = 0; j < mapDimensions.y; j++)
            {
                int tileIndex = tileColorKeys.IndexOf(colors[currentIndex]);
                GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;

                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);

                currentIndex++;
            }
        }

        spawnPoints = foundSpawns;
    }
}
