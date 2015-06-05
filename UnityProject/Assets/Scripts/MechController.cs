namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public class MechController : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public float speed;

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private CharacterController characterController;

        private void Start()
        {
            this.characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
        }
    }
}
