namespace Assets.Scripts.InputHandling
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class KeyboardInputDeviceMappingEntry
    {
        public PlayerControl Control { get; set; }

        public bool IsPositive { get; set; }
    }

    public class KeyboardInputDeviceMapping : BaseInputDeviceMapping
    {
        private readonly IDictionary<string, IList<KeyboardInputDeviceMappingEntry>> axisMapping;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public KeyboardInputDeviceMapping()
        {
            this.axisMapping = new Dictionary<string, IList<KeyboardInputDeviceMappingEntry>>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void SetAxis(string axis, PlayerControl target, bool isPositive = true)
        {
            System.Diagnostics.Trace.Assert(!this.axisMapping.ContainsKey(axis));

            var entry = new KeyboardInputDeviceMappingEntry { Control = target, IsPositive = isPositive };
            if (!this.axisMapping.ContainsKey(axis))
            {
                this.axisMapping.Add(axis, new List<KeyboardInputDeviceMappingEntry>());
            }

            this.axisMapping[axis].Add(entry);
        }

        public override void Update()
        {
            base.Update();

            foreach (string axis in this.axisMapping.Keys)
            {
                foreach (KeyboardInputDeviceMappingEntry entry in this.axisMapping[axis])
                {
                    this.UpdateState(Input.GetAxis(axis), Input.GetButtonDown(axis), entry.Control, entry.IsPositive);
                }
            }
        }
    }
}
