﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

﻿using Assets.Scripts;
﻿using Assets.Scripts.Arena;

﻿using JetBrains.Annotations;

public class Arena : MonoBehaviour
{
    private const string TagIndestructible = "x";
    private const string TagDestructible = "d";
    private const string TagHalf = "h";
    private const string TagSpawn = "s";

    private int currentLineToProcess;

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    [SerializeField]
    private List<Color> tileColorKeys;

    [SerializeField]
    private List<GameObject> tiles;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public List<Vector3> SpawnPoints { get; private set; }
    public List<ArenaTile> Tiles {get; private set;}
    
    public void InitFromText(string fileName)
    {   
        StreamReader streamReader = new StreamReader(Application.dataPath + string.Format(StaticSettings.MapFileFilter, fileName));

        this.currentLineToProcess = 0;
        using (streamReader)
        {
            string line;
            do
            {
                line = streamReader.ReadLine();
                this.ProcessLine(line);
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
                int tileIndex = this.tileColorKeys.IndexOf(colors[currentIndex]);
                GameObject tile = Instantiate(this.tiles[tileIndex]);

                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);

                currentIndex++;
            }
        }
        //TODO: Recognize spawns
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    [UsedImplicitly]
    private void Awake()
    {
        this.Tiles = new List<ArenaTile>();
        this.SpawnPoints = new List<Vector3>();
    }

    private void ProcessLine(string line)
    {
        if (!string.IsNullOrEmpty(line))
        {
            for (int j = 0; j < line.Length; ++j)
            {
                string character = line.Substring(j, 1);
                int tileIndex = 0;
                ArenaTileType type = ArenaTileType.Ground;
                if (character == TagIndestructible)
                {
                    tileIndex = 1;
                    type = ArenaTileType.Indestructible;
                }
                else if (character == TagDestructible)
                {
                    tileIndex = 2;
                    type = ArenaTileType.Destructible;
                }
                else if (character == TagHalf)
                {
                    tileIndex = 3;
                    type = ArenaTileType.HalfWall;
                }
                else if (character == TagSpawn)
                {
                    this.SpawnPoints.Add(new Vector3(-5.25f + j * 0.35f, 5.25f - this.currentLineToProcess * 0.35f, 0f));
                    type = ArenaTileType.Spawn;
                }

                GameObject tile = Instantiate(this.tiles[tileIndex]);
                this.Tiles.Add(new ArenaTile { CurrentType = type, TileView = tile });

                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - this.currentLineToProcess * 0.35f, 0f);
            }

            this.currentLineToProcess++;
        }
    }
}
