namespace Assets.Scripts
{
    using System.Collections.Generic;

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
        }
        
        [UsedImplicitly]
        private void Update()
        {
            foreach (InputDevice device in InputManager.Devices)
            {
                //device.Action1
            }
        }

        private void OnDeviceDetached(InputDevice device)
        {
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
