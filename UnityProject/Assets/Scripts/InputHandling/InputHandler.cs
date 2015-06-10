namespace Assets.Scripts.InputHandling
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using InControl;

    public static class InputHandler
    {
        private static readonly IDictionary<ICharacter, IInputDeviceMapping> PlayerDeviceMapping;
        private static readonly IDictionary<IInputDeviceMapping, ICharacter> DeviceToPlayerMapping;

        private static readonly IDictionary<InputDevice, IInputDeviceMapping> ControllerMappings;

        private static readonly IList<IInputDeviceMapping> DeviceMappings;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static InputHandler()
        {
            PlayerDeviceMapping = new Dictionary<ICharacter, IInputDeviceMapping>();
            DeviceToPlayerMapping = new Dictionary<IInputDeviceMapping, ICharacter>();

            ControllerMappings = new Dictionary<InputDevice, IInputDeviceMapping>();

            DeviceMappings = new List<IInputDeviceMapping>();

            InputManager.OnDeviceDetached += OnDeviceDetached;

            CreateKeyboardMappings("K1");
            CreateKeyboardMappings("K2");
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static void RegisterPlayer(ICharacter player)
        {
            PlayerDeviceMapping.Add(player, null);
        }

        public static void Update()
        {
            UpdateControllerState();

            foreach (IInputDeviceMapping mapping in DeviceMappings)
            {
                // Update the mapping
                mapping.Update();

                if (DeviceToPlayerMapping.ContainsKey(mapping))
                {
                    // The mapping is already assigned to a player
                    continue;
                }

                // Check if the mapping is asking to be assigned to a player
                if (mapping.GetState(PlayerControl.Confirm).IsPressed)
                {
                    IList<ICharacter> players = new List<ICharacter>(PlayerDeviceMapping.Keys);
                    foreach (ICharacter character in players)
                    {
                        if (PlayerDeviceMapping[character] != null)
                        {
                            continue;
                        }

                        // We have a player without a mapping so connect this one
                        PlayerDeviceMapping[character] = mapping;
                        DeviceToPlayerMapping.Add(mapping, character);
                        character.InputDevice = mapping;
                        UnityEngine.Debug.Log("Assigned Mapping to Player " + character.Name);
                        break;
                    }
                }
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private static void UpdateControllerState()
        {
            if (InputManager.Devices == null)
            {
                return;
            }

            foreach (InputDevice device in InputManager.Devices)
            {
                if (ControllerMappings.ContainsKey(device))
                {
                    // The controller is already registerd
                    continue;
                }

                // We have a new or reconnected controller present, add a new mapping for it
                var mapping = new ControllerInputDeviceMapping { Device = device };
                ControllerMappings.Add(device, mapping);
                DeviceMappings.Add(mapping);
                UnityEngine.Debug.Log("Controller Registered!");
            }
        }

        private static void OnDeviceDetached(InputDevice controller)
        {
            if (!ControllerMappings.ContainsKey(controller))
            {
                return;
            }
            
            // Get the mapping and if its exists remove
            IInputDeviceMapping mapping = ControllerMappings[controller];
            ControllerMappings.Remove(controller);
            DeviceMappings.Remove(mapping);

            // Check if the mapping was already assigned to a player
            if (!DeviceToPlayerMapping.ContainsKey(mapping))
            {
                return;
            }

            // Unregister the device and it's mappings
            ICharacter player = DeviceToPlayerMapping[mapping];
            player.InputDevice = null;
            PlayerDeviceMapping[player] = null;
            DeviceToPlayerMapping.Remove(mapping);

            UnityEngine.Debug.LogWarning("Controller was detached!");
        }

        private static void CreateKeyboardMappings(string prefix)
        {
            var mapping = new KeyboardInputDeviceMapping();
            mapping.SetAxis("Submit", PlayerControl.Start);
            mapping.SetAxis(prefix + "Move", PlayerControl.MoveForward);
            mapping.SetAxis(prefix + "Move", PlayerControl.MoveBackwards, false);
            mapping.SetAxis(prefix + "Rotate", PlayerControl.MoveRotateLeft, false);
            mapping.SetAxis(prefix + "Rotate", PlayerControl.MoveRotateRight);
            mapping.SetAxis(prefix + "Fire1", PlayerControl.Fire);
            mapping.SetAxis(prefix + "Fire1", PlayerControl.Confirm);
            mapping.SetAxis(prefix + "Fire2", PlayerControl.Fire2);
            mapping.SetAxis(prefix + "Fire2", PlayerControl.Exit);
            mapping.SetAxis(prefix + "Fire3", PlayerControl.DropGear);
            mapping.SetAxis(prefix + "CycleItems", PlayerControl.CycleItemsLeft);
            mapping.SetAxis(prefix + "CycleItems", PlayerControl.CycleItemsRight, false);

            DeviceMappings.Add(mapping);
        }

        public static void ReleaseMapping(ICharacter player)
        {
            if (!PlayerDeviceMapping.ContainsKey(player))
            {
                return;
            }

            IInputDeviceMapping device = PlayerDeviceMapping[player];
            if (device == null)
            {
                return;
            }

            // If a controller was assigned we need to also unregister it's device
            var controllerBinding = device as ControllerInputDeviceMapping;
            if (controllerBinding != null)
            {
                ControllerMappings.Remove(controllerBinding.Device);
            }

            DeviceToPlayerMapping.Remove(device);
            PlayerDeviceMapping[player] = null;
            player.InputDevice = null;
        }
    }
}
