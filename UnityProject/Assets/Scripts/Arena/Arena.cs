﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
 
﻿using JetBrains.Annotations;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;
﻿using Assets.Scripts.Logic.Enums;

namespace Assets.Scripts.Arena
{
    public class Arena : MonoBehaviour
    {
        private static Arena instance;
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static Arena Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject existing = GameObject.Find(typeof(Arena).Name);
                    if (existing == null)
                    {
                        existing = new GameObject(typeof(Arena).Name);
                        instance = existing.AddComponent<Arena>();
                    }
                    else
                    {
                        instance = existing.GetComponent<Arena>();
                    }
                }

                return instance;
            }
        }

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Arena()
        {
            this.UnclaimedGear = new List<GearView>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public List<Color> tileColorKeys;

        [SerializeField]
        public List<GameObject> tilePrefabs;

        [SerializeField]
        public GameObject gearViewPrefab;

        [SerializeField]
        public GameplayManager gameplayManager;

        public List<ArenaTile> Tiles { get; private set; }
        public List<GearView> UnclaimedGear { get; private set; }

        public ArenaData Data { get; private set; }

        public void Initialize(ArenaData newData)
        {
            // Clear out the map contents
            this.Uninitialize();
            this.Data = newData;

            for (var i = 0; i < newData.TileIndizes.Count; i++)
            {
                this.BuildTile(newData.TileIndizes[i], newData.Positions[i]);
            }
        }

        public void Uninitialize()
        {
            foreach (ArenaTile tile in this.Tiles)
            {
                Destroy(tile.TileView);
            }

            this.Tiles.Clear();

            foreach (GearView view in this.UnclaimedGear)
            {
                Destroy(view);
            }

            this.UnclaimedGear.Clear();
            this.Data = null;
        }

        public void PlaceStarterGear()
        {
            int characterCount = this.gameplayManager.Characters.Count;
            int totalStarterGear = StaticSettings.NumGearDropsPerCharacterAtStart * characterCount;

            for (int i = 0; i < totalStarterGear; ++i)
            {
                this.PlaceGear(GearGeneration.GenerateRandomGear());
            }
        }

        public void ClaimGear(IGear gear)
        {
            GearView gearView = this.UnclaimedGear.Find(g => g.Gear == gear);
            if (gearView != null)
            {
                this.UnclaimedGear.Remove(gearView);
            }
            Destroy(gearView.gameObject);
        }

        public void PlaceGear(IGear gear)
        {
            const float minGearDropProximity = 0.7f;

            List<ArenaTile> availableTiles = new List<ArenaTile>();
            foreach (ArenaTile tile in this.Tiles)
            {
                if (tile.CurrentType != ArenaTileType.Ground)
                {
                    continue;
                }

                Vector3 tilePosition = tile.TileView.transform.position;

                bool isValid = true;

                foreach (GearView gearView in this.UnclaimedGear)
                {
                    if ((gearView.transform.position - tilePosition).magnitude < minGearDropProximity)
                    {
                        isValid = false;
                        break;
                    }
                }

                foreach (PlayerCharacterBehavior characterView in this.gameplayManager.CharacterViews)
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

            if (availableTiles.Count == 0)
            {
                return;
            }

            GameObject newGear = Instantiate(this.gearViewPrefab);
            newGear.transform.SetParent(this.transform);
            newGear.transform.position = availableTiles[Random.Range(0, availableTiles.Count)].TileView.transform.position;

            GearView newGearView = newGear.GetComponent<GearView>();
            newGearView.Init(gear);
            this.UnclaimedGear.Add(newGearView);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Awake()
        {
            this.Tiles = new List<ArenaTile>();
        }

        [UsedImplicitly]
        private void Update()
        {
        }

        private ArenaTileType GetTileType(int index)
        {
            switch (index)
            {
                case 1: return ArenaTileType.Indestructible;
                case 2: return ArenaTileType.Destructible;
                case 3: return ArenaTileType.HalfWall;
                case 4: return ArenaTileType.Spawn;
            }

            return ArenaTileType.Ground;
        }

        private void BuildTile(int tileIndex, Vector3 position)
        {
            ArenaTileType type = this.GetTileType(tileIndex);
            GameObject tile = Instantiate(this.tilePrefabs[tileIndex]);
            this.Tiles.Add(new ArenaTile { CurrentType = type, TileView = tile });

            tile.transform.SetParent(this.transform);
            tile.transform.localPosition = position;
        }
    }
}