﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

﻿using Assets.Scripts;
﻿using Assets.Scripts.Arena;

﻿using JetBrains.Annotations;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;

public class Arena : MonoBehaviour
{
    private const string TagIndestructible = "x";
    private const string TagDestructible = "d";
    private const string TagHalf = "h";
    private const string TagSpawn = "s";

    private int currentLineToProcess;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public Arena()
    {
        UnclaimedGear = new List<GearView>();
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    [SerializeField]
    private List<Color> tileColorKeys;

    [SerializeField]
    private List<GameObject> tiles;

    [SerializeField]
    private GameObject gearViewPrefab;

    [SerializeField]
    private GameplayManager gameplayManager;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public List<Vector3> SpawnPoints { get; private set; }
    public List<ArenaTile> Tiles {get; private set;}
    public List<GearView> UnclaimedGear { get; private set; }
    
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

    public void PlaceStarterGear()
    {
        int characterCount = gameplayManager.Characters.Count;
        int totalStarterGear = StaticSettings.NumGearDropsPerCharacterAtStart * characterCount;

        for (int i = 0; i < characterCount; ++i)
        {   
            PlaceGear(GearGeneration.GenerateRandomGear(GearType.RightWeapon));
        }

        for (int i = 0; i < totalStarterGear - characterCount; ++i)
        {
            PlaceGear(GearGeneration.GenerateRandomGear());
        }
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

    [UsedImplicitly]
    private void Update()
    {

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

    private void PlaceGear(IGear gear)
    {
        const float minGearDropProximity = 0.7f;
        
        List<ArenaTile> availableTiles = new List<ArenaTile>();
        foreach (ArenaTile tile in Tiles)
        {
            if(tile.CurrentType != ArenaTileType.Ground)
            {
                continue;
            }

            Vector3 tilePosition = tile.TileView.transform.position;

            bool isValid = true;

            foreach (GearView gearView in UnclaimedGear)
            {
                if ((gearView.transform.position - tilePosition).magnitude < minGearDropProximity)
                {
                    isValid = false;
                    break;
                }
            }

            foreach(PlayerCharacterBehavior characterView in gameplayManager.CharacterViews)
            {
                if ((characterView.transform.position - tilePosition).magnitude < minGearDropProximity)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                availableTiles.Add(tile);
            }
        }

        if(availableTiles.Count == 0)
        {
            return;
        }

        GameObject newGear = Instantiate(gearViewPrefab) as GameObject;
        newGear.transform.SetParent(this.transform);
        newGear.transform.position = availableTiles[Random.Range(0, availableTiles.Count)].TileView.transform.position;

        GearView newGearView = newGear.GetComponent<GearView>();
        newGearView.Init(gear);
        UnclaimedGear.Add(newGearView);
    }
}