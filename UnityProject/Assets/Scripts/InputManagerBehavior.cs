namespace Assets.Scripts
{
    using System.Collections.Generic;

    using InControl;

    using JetBrains.Annotations;

    using UnityEngine;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    public class InputManagerBehavior : MonoBehaviour
    {
        private static InputManagerBehavior instance;

        private readonly IDictionary<ICharacter, InputDevice> charactersToInputDevices;
        private readonly IDictionary<InputDevice, ICharacter> inputDevicesToCharacters;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public InputManagerBehavior()
        {
            this.charactersToInputDevices = new Dictionary<ICharacter, InputDevice>();
            this.inputDevicesToCharacters = new Dictionary<InputDevice, ICharacter>();
        }

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
            return new List<ICharacter>(charactersToInputDevices.Keys);
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
            InputManager.OnDeviceDetached += this.OnDeviceDetached;

            for (int i = 0; i < StaticSettings.MaxPlayerCount; ++i )
            {   
                var newPlayer = new Character();
                this.charactersToInputDevices.Add(newPlayer, null);
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
            if(InputManager.Devices == null)
            {
                return;
            }

            foreach (InputDevice device in InputManager.Devices)
            {
                if (this.inputDevicesToCharacters.ContainsKey(device))
                {
                    // This device is already assigned, skip it
                    continue;
                }

                if (device.GetControl(InputControlType.Action1))
                {
                    ICharacter target = this.GetPlayerForNewController();
                    if (target != null)
                    {
                        this.charactersToInputDevices[target] = device;
                        this.inputDevicesToCharacters.Add(device, target);
                        target.InputDevice = device;
                        Debug.Log("Assigned Controller " + device.Name + " to player " + target.Name);
                    }
                }
            }
        }

        private ICharacter GetPlayerForNewController()
        {
            foreach (ICharacter character in this.charactersToInputDevices.Keys)
            {
                if (character.InputDevice == null)
                {
                    return character;
                }
            }

            return null;
        }

        private void OnDeviceDetached(InputDevice device)
        {
            if (this.inputDevicesToCharacters.ContainsKey(device))
            {
                ICharacter assignedCharacter = this.inputDevicesToCharacters[device];
                assignedCharacter.InputDevice = null;
                this.charactersToInputDevices[assignedCharacter] = null;
                this.inputDevicesToCharacters.Remove(device);
            }
        }
    }
}
