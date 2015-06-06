namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Controls;
    using Assets.Scripts.Logic;

    using JetBrains.Annotations;

    using UnityEngine;

    public class CharacterBehavior : MonoBehaviour
    {
        private IActor character;

        private IMovementController movementController;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public bool useFixedAxisController = true;

        public IActor Character
        {
            get
            {
                return this.character;
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Start()
        {
            this.character = new PlayerCharacter();

            // Create the movement controller
            if (this.useFixedAxisController)
            {
                this.movementController = new FixedAxisPlayerController(this.gameObject);
            }
            else
            {
                this.movementController = new FreeFormPlayerController(this.gameObject);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            float fireIn = Input.GetAxis("Fire1");

            this.movementController.Velocity = this.character.GetStat(StatType.Velocity);
            this.movementController.RotationSpeed = this.character.GetStat(StatType.RotationSpeed);
            this.movementController.Update();
        }
    }
}
