namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;

    using InControl;

    using JetBrains.Annotations;

    using UnityEngine;

    public class GameManagerBehavior : MonoBehaviour
    {
        private IDictionary<CharacterBehavior, InputDevice> charactersToInputDevices;
        private IDictionary<InputDevice, CharacterBehavior> inputDevicesToCharacters;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public GameManagerBehavior()
        {
            this.charactersToInputDevices = new Dictionary<CharacterBehavior, InputDevice>();
            this.inputDevicesToCharacters = new Dictionary<InputDevice, CharacterBehavior>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public bool enableInControl;

        [SerializeField]
        public GameObject player1;

        [SerializeField]
        public GameObject player2;

        [SerializeField]
        public GameObject player3;

        [SerializeField]
        public GameObject player4;

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Start()
        {
            InputManager.OnDeviceDetached += this.OnDeviceDetached;

            this.RegisterPlayer(this.player1);
            this.RegisterPlayer(this.player2);
            this.RegisterPlayer(this.player3);
            this.RegisterPlayer(this.player4);

            // Override the static InControl setting
            StaticSettings.EnableInControl = this.enableInControl;
        }
        
        [UsedImplicitly]
        private void Update()
        {
            foreach (InputDevice device in InputManager.Devices)
            {
                if (this.inputDevicesToCharacters.ContainsKey(device))
                {
                    // This device is already assigned, skip it
                    continue;
                }

                if (device.GetControl(InputControlType.Action1))
                {
                    foreach (CharacterBehavior behavior in this.charactersToInputDevices.Keys)
                    {
                        if (this.charactersToInputDevices[behavior] != null)
                        {
                            this.charactersToInputDevices[behavior] = device;
                            this.inputDevicesToCharacters.Add(device, behavior);
                            behavior.InputDevice = device;
                            Debug.Log("Assigned Controller " + device.Name + " to player " + behavior.name);
                            break;
                        }
                    }
                }
            }
        }

        private void OnDeviceDetached(InputDevice device)
        {
            if (this.inputDevicesToCharacters.ContainsKey(device))
            {
                CharacterBehavior assignedCharacter = this.inputDevicesToCharacters[device];
                assignedCharacter.InputDevice = null;
                this.charactersToInputDevices[assignedCharacter] = null;
                this.inputDevicesToCharacters.Remove(device);
            }
        }

        private void RegisterPlayer(GameObject player)
        {
            if (player == null)
            {
                return;
            }

            var behavior = player.GetComponent<CharacterBehavior>();
            if (behavior == null)
            {
                Debug.LogWarning("Player has no Character behavior set!");
                return;
            }

            this.charactersToInputDevices.Add(behavior, null);
        }
    }
}
