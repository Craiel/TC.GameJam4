namespace Assets.Scripts
{
    using System.Collections.Generic;

    using InControl;

    using JetBrains.Annotations;

    using UnityEngine;

    public class InputManagerBehavior : MonoBehaviour
    {
        private readonly IDictionary<PlayerBehavior, InputDevice> charactersToInputDevices;
        private readonly IDictionary<InputDevice, PlayerBehavior> inputDevicesToCharacters;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public InputManagerBehavior()
        {
            this.charactersToInputDevices = new Dictionary<PlayerBehavior, InputDevice>();
            this.inputDevicesToCharacters = new Dictionary<InputDevice, PlayerBehavior>();
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
            // Todo: for testing only
            if(InputManager.ActiveDevice.Command.Value > 0)
            {
                foreach (PlayerBehavior behavior in this.charactersToInputDevices.Keys)
                {
                    if (behavior.Character.InputDevice != null)
                    {
                        behavior.TempTransitionToGameMode();
                    }
                }
                this.charactersToInputDevices.Clear();
                this.inputDevicesToCharacters.Clear();
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
                    foreach (PlayerBehavior behavior in this.charactersToInputDevices.Keys)
                    {
                        if (this.charactersToInputDevices[behavior] == null)
                        {
                            this.charactersToInputDevices[behavior] = device;
                            this.inputDevicesToCharacters.Add(device, behavior);
                            behavior.Character.InputDevice = device;
                            Debug.Log("Assigned Controller " + device.Name + " to player " + behavior.Character.Name);
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
                PlayerBehavior assignedCharacter = this.inputDevicesToCharacters[device];
                assignedCharacter.Character.InputDevice = null;
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

            var behavior = player.GetComponent<PlayerBehavior>();
            if (behavior == null)
            {
                Debug.LogWarning("Player has no Player behavior set!");
                return;
            }

            this.charactersToInputDevices.Add(behavior, null);
        }
    }
}
