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
    }
}
