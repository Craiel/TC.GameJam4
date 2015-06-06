namespace Assets.Scripts.Logic
{
    using UnityEngine;

    public class CombatResolve
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CombatResolve(CombatInfo info)
        {
            this.Info = info;
            this.Result = new CombatResult { Info = info };
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public GameObject Source { get; set; }

        public GameObject Target { get; set; }

        public CombatInfo Info { get; private set; }

        public CombatResult Result { get; private set; }
    }
}
