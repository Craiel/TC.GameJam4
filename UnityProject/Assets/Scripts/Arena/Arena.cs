﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private List<Color> tileColorKeys;

    [SerializeField]
    private List<GameObject> tiles;

    public List<Vector3> SpawnPoints { get; private set; }
    public List<Tile> Tiles {get; private set;}

    private void Awake() 
    {
        Tiles = new List<Tile>();
        SpawnPoints = new List<Vector3>();
    }

    public void InitFromText(string fileName)
    {   
        StreamReader streamReader = new StreamReader(Application.dataPath + "/Resources/Maps/" + fileName + ".txt");

        string indestructible = "x";
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
                    for (int j = 0; j < line.Length; ++j)
                    {
                        string character = line.Substring(j, 1);
                        int tileIndex = 0;
                        Tile.Type type = Tile.Type.Ground;
                        if (character == indestructible)
                        {
                            tileIndex = 1;
                            type = Tile.Type.Indestructible;
                        }
                        else if (character == destructible)
                        {
                            tileIndex = 2;
                            type = Tile.Type.Destructible;
                        }
                        else if (character == half)
                        {
                            tileIndex = 3;
                            type = Tile.Type.HalfWall;
                        }
                        else if (character == spawn)
                        {
                            SpawnPoints.Add(new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f));
                            type = Tile.Type.Spawn;
                        }

                        GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;
                        Tiles.Add(new Tile{CurrentType = type, TileView = tile});

                        tile.transform.SetParent(this.transform);
                        tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);
                    }

                    i++;
                }

            }
            while (line != null);
        }
    }

    public void InitFromTexture(Texture2D texture)
    {   
        Vector2 mapDimensions = new Vector2(texture.width, texture.height);

        int currentIndex = 0;
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < mapDimensions.x; i++)
        {
            for (int j = 0; j < mapDimensions.y; j++)
            {
                int tileIndex = tileColorKeys.IndexOf(colors[currentIndex]);
                GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;

                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);

                currentIndex++;
            }
        }
        //TODO: Recognize spawns
    }

    public class Tile
    {
        public enum Type
        {
            Indestructible,
            Destructible,
            HalfWall,
            Spawn,
            Ground
        }

        public Type CurrentType { get; set; }
        public GameObject TileView { get; set; }
    }
}
