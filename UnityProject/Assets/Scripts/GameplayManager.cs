using System.Collections.Generic;

using Assets.Scripts.Contracts;

using UnityEngine;

namespace Assets.Scripts
{
    using System;

    using Assets.Scripts.Arena;
    using Assets.Scripts.Logic.Enums;

    using JetBrains.Annotations;

    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;
    using Assets.Scripts.Logic;

    public class GameplayManager : MonoBehaviour
    {
        private readonly IDictionary<ICharacter, GameObject> activePlayers;
        
        private readonly IList<ArenaData> arenaData; 

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public GameplayManager()
        {
            this.activePlayers = new Dictionary<ICharacter, GameObject>();

            this.arenaData = new List<ArenaData>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public List<Object> textArenas;

        [SerializeField]
        public List<Texture2D> imageArenas;

        [SerializeField]
        public Arena.Arena arena;

        [SerializeField]
        public GameObject mechPrefab;

        private float timeUntilSpawn;

        public bool IsPlaying { get; private set; }

        public IList<ICharacter> Characters { get; private set; }

        public List<PlayerCharacterBehavior> CharacterViews { get; private set; }

        public void SetupMatch(IList<ICharacter> characters)
        {
            if (this.IsPlaying)
            {
                throw new InvalidOperationException("Game is underway");
            }

            this.Characters = characters;
            this.InitializeRandomArena();
            this.SpawnMechs(characters);
            this.arena.PlaceStarterGear();

            this.timeUntilSpawn = 30f;
            this.IsPlaying = true;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Awake()
        {
            this.CharacterViews = new List<PlayerCharacterBehavior>();

            foreach (Texture2D texture in this.imageArenas)
            {
                var data = new ArenaData { Name = texture.name };
                data.InitFromTexture(texture, this.arena.tileColorKeys);
                if (data.IsValid)
                {
                    this.arenaData.Add(data);
                }
            }

            foreach (Object textFile in this.textArenas)
            {
                var data = new ArenaData { Name = textFile.name };
                data.InitFromText(textFile.name);
                if (data.IsValid)
                {
                    this.arenaData.Add(data);
                }
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!this.IsPlaying)
            {
                return;
            }

            int aliveCharacters = this.activePlayers.Count;
            foreach (ICharacter character in this.activePlayers.Keys)
            {
                if (character.IsDead)
                {
                    if (!this.KillCharacter(character))
                    {
                        aliveCharacters--;
                    }
                }
            }

            if (aliveCharacters <= 1)
            {
                this.EndGame();
            }
            else
            {
                this.timeUntilSpawn -= Time.deltaTime;
                if(this.timeUntilSpawn < 0)
                {
                    for(int i=0; i<aliveCharacters; ++i)
                    {
                        arena.PlaceGear(GearGeneration.GenerateRandomGear());
                    }
                }
                this.timeUntilSpawn = 30f;
            }
        }

        private void EndGame()
        {
            // Mark that we have stopped playing first to disable all updates
            this.IsPlaying = false;

            // Clear out the arena
            this.arena.Uninitialize();

            // Destroy all players
            foreach (ICharacter player in this.activePlayers.Keys)
            {
                Destroy(this.activePlayers[player]);
            }

            this.activePlayers.Clear();
            this.Characters.Clear();
        }

        private bool KillCharacter(ICharacter character)
        {
            float levels = character.GetCurrentStat(StatType.Level);
            if (levels < 1)
            {
                // Character is dead for good
                return false;
            }

            character.ResetCurrentStats();
            character.SetStat(StatType.Level, levels - 1);
            this.RespawnMech(character);
            return true;
        }

        private void ChangeArena()
        {
            this.InitializeRandomArena();

            foreach (ICharacter player in this.activePlayers.Keys)
            {
                this.RespawnMech(player);
            }
        }

        private void InitializeRandomArena()
        {
            int index = Random.Range(0, this.arenaData.Count);
            this.arena.Initialize(this.arenaData[index]);
        }

        private void SpawnMechs(IList<ICharacter> characters)
        {
            foreach(ICharacter character in characters)
            {
                if(character.InputDevice == null)
                {
                    continue;
                }

                GameObject newMech = Instantiate(this.mechPrefab);
                newMech.GetComponent<PlayerCharacterBehavior>().Character = character;
                newMech.transform.SetParent(this.arena.transform);
                this.activePlayers.Add(character, newMech);
                this.CharacterViews.Add(newMech.GetComponent<PlayerCharacterBehavior>());

                // Spawn it
                this.RespawnMech(character);
            }
        }
        
        private void RespawnMech(ICharacter character)
        {
            // Find a valid spawn point
            Vector3 spawnPoint = this.arena.Data.SpawnPoints[Random.Range(0, this.arena.Data.SpawnPoints.Count)];

            // Set the player to the spawn point and reset it's rotation
            this.activePlayers[character].transform.localPosition = spawnPoint;
            this.activePlayers[character].transform.rotation = Quaternion.identity;

            // Re-activate the character
            this.activePlayers[character].SetActive(true);
        }
    }
}
