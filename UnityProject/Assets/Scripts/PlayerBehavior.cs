namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using JetBrains.Annotations;

    using UnityEngine;

    public class PlayerBehavior : MonoBehaviour
    {
        private ICharacter character;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public string characterName = "player";

        public ICharacter Character
        {
            get
            {
                return this.character;
            }
        }

        public void TempTransitionToGameMode()
        {
            var gameBehavior = this.gameObject.AddComponent<PlayerCharacterBehavior>();
            gameBehavior.Character = this.character;
            gameBehavior.mechController = this.gameObject.GetComponent<Animator>();

            Destroy(this);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Start()
        {
            this.character = new Character { Name = "Player" };
        }
    }
}
