namespace Assets.Scripts
{
    using System;

    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Controls;
    using Assets.Scripts.Logic;

    using JetBrains.Annotations;

    using UnityEngine;

    public class CharacterBehavior : MonoBehaviour
    {
        private readonly IList<ProjectileBehavior> projectiles;
 
        private ICharacter character;

        private IMovementController movementController;

        private GameObject projectileParent;

        private bool currentMove;

        private bool nextMove;

        private float changeTime;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CharacterBehavior()
        {
            this.projectiles = new List<ProjectileBehavior>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public string characterName = "player";

        [SerializeField]
        public bool useFixedAxisController = true;

        [SerializeField]
        public Animator mechController;

        public ICharacter Character
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
            this.character = new PlayerCharacter { Name = this.characterName };
            this.projectileParent = new GameObject(this.character.Name + "_Bullets");

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
            float fireLeft = Input.GetAxis("Fire1");
            float fireRight = Input.GetAxis("Fire2");

            float currentTime = Time.time;

            if (Math.Abs(fireLeft) > float.Epsilon)
            {
                this.FireWeapon(this.character.LeftWeapon);
            }

            if (Math.Abs(fireRight) > float.Epsilon)
            {
                this.FireWeapon(this.character.RightWeapon);
            }

            this.movementController.Velocity = this.character.GetStat(StatType.Velocity);
            this.movementController.RotationSpeed = this.character.GetStat(StatType.RotationSpeed);

            bool didUpdate = this.movementController.Update();
            this.UpdateMoveAnimation(didUpdate);

            this.UpdateProjectileLifespan(currentTime);
        }

        private void FireWeapon(IWeapon weapon)
        {
            if (weapon == null || !weapon.CanFire())
            {
                return;
            }

            IList<ProjectileBehavior> newProjectiles = weapon.Fire(this.gameObject, this.character);
            foreach (ProjectileBehavior projectile in newProjectiles)
            {
                projectile.transform.SetParent(this.projectileParent.transform);
                this.projectiles.Add(projectile);
            }
        }

        private void UpdateProjectileLifespan(float currentTime)
        {
            IList<ProjectileBehavior> projectileList = new List<ProjectileBehavior>(this.projectiles);
            foreach (ProjectileBehavior projectile in projectileList)
            {
                if (currentTime > projectile.LifeSpan)
                {
                    this.projectiles.Remove(projectile);

                    if(projectile.IsAlive)
                    {
                        projectile.Dispose();
                    }
                }
            }
        }

        private void UpdateMoveAnimation(bool didUpdate)
        {
            //If there is movement our next move is true
            if (didUpdate)
                this.nextMove = true;
            else
                this.nextMove = false;

            if (this.nextMove != this.currentMove && this.changeTime + 1 >= Time.time)
            {
                if (this.nextMove)
                {
                    this.mechController.SetTrigger("WalkUp");
                    this.currentMove = true;
                }
                else
                {
                    this.mechController.SetTrigger("Idle");
                    this.currentMove = false;
                }
                this.changeTime = Time.time;
            }
            else
                this.changeTime = Time.time;
        }
    }
}
