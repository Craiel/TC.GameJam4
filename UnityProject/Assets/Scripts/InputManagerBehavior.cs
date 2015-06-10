namespace Assets.Scripts
{
    using System.Collections.Generic;

    using InControl;

    using JetBrains.Annotations;

    using UnityEngine;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.InputHandling;
    using Assets.Scripts.Logic;

    public class InputManagerBehavior : MonoBehaviour
    {
        private static InputManagerBehavior instance;

        private static readonly IList<ICharacter> characters = new List<ICharacter>(); 
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static InputManagerBehavior Instance
        {
            get
            {
                if(instance == null) {
                    GameObject existing = GameObject.Find(typeof(InputManagerBehavior).Name);
                    if(existing == null) {
                        existing = new GameObject(typeof(InputManagerBehavior).Name);
                        instance = existing.AddComponent<InputManagerBehavior>();
                    } else {
                        instance = existing.GetComponent<InputManagerBehavior>();
                    }
                }

                return instance;
            }
        }

        [SerializeField]
        public bool enableInControl;

        [SerializeField]
        private GameplayManager gameplayManager;

        [SerializeField]
        private UIManager UIManager;

        public IList<ICharacter> GetCharacters()
        {
            return characters;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this);
        }

        [UsedImplicitly]
        private void Start()
        {
            for (int i = 0; i < StaticSettings.MaxPlayerCount; ++i )
            {
                var newPlayer = new Character { Color = StaticSettings.PlayerColors[i] };
                InputHandler.RegisterPlayer(newPlayer);
                characters.Add(newPlayer);
            }

            if (this.UIManager != null)
            {
                this.UIManager.Init(this.GetCharacters());    
            }
            
            // Override the static InControl setting
            StaticSettings.EnableInControl = this.enableInControl;
        }
        
        [UsedImplicitly]
        private void Update()
        {
            InputHandler.Update();
        }
    }
}
