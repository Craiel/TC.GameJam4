namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using UnityEngine;

    public class Character : MonoBehaviour
    {
        [SerializeField]
        public IList<IWeapon> weapons;

        private void Update()
        {
        }
    }
}
