namespace Assets.Scripts
{
    using System;

    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Controls;
    using Assets.Scripts.Logic;
    
    using JetBrains.Annotations;

    using UnityEngine;

    public class PlayerCharacterBehavior : MonoBehaviour
    {
        private readonly IList<ProjectileBehavior> projectiles;
        
        private IMovementController movementController;

        private GameObject projectileParent;

        private bool currentMove;

        private bool nextMove;

        private float changeTime;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public PlayerCharacterBehavior()
        {
            this.projectiles = new List<ProjectileBehavior>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public bool useFixedAxisController = true;

        [SerializeField]
        public Animator mechController;

        // Mostly needed for inspection
        public ICharacter Character { get; set; }
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void InitializeMovementController()
        {
            if (StaticSettings.EnableInControl)
            {
                this.movementController = new InControlPlayerController(this.gameObject);
            }
            else
            {
                if (this.useFixedAxisController)
                {
                    this.movementController = new FixedAxisPlayerController(this.gameObject);
                }
                else
                {
                    this.movementController = new FreeFormPlayerController(this.gameObject);
                }
            }

            this.movementController.InputDevice = this.Character.InputDevice;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (this.Character == null)
            {
                return;
            }

            this.UpdateBehaviorState();

            this.Character.Update(this.gameObject);

            float fireLeft;
            float fireRight;
            if (StaticSettings.EnableInControl && this.Character.InputDevice != null)
            {
                fireLeft = this.Character.InputDevice.LeftTrigger.Value;
                fireRight = this.Character.InputDevice.RightTrigger.Value;
            }
            else
            {
                fireLeft = Input.GetAxis("Fire1");
                fireRight = Input.GetAxis("Fire2");
            }
            
            float currentTime = Time.time;

            if (Math.Abs(fireLeft) > float.Epsilon)
            {
                this.FireWeapon(this.Character.GetGear(GearType.LeftWeapon) as IWeapon);
            }

            if (Math.Abs(fireRight) > float.Epsilon)
            {
                this.FireWeapon(this.Character.GetGear(GearType.RightWeapon) as IWeapon);
            }

            this.movementController.Velocity = this.Character.GetStat(StatType.Velocity);
            this.movementController.RotationSpeed = this.Character.GetStat(StatType.RotationSpeed);

            bool didUpdate = this.movementController.Update();
            this.UpdateMoveAnimation(didUpdate);

            this.UpdateProjectileLifespan(currentTime);
        }

        private void UpdateBehaviorState()
        {
            if (this.movementController == null)
            {
                this.InitializeMovementController();
            }

            if (this.movementController.InputDevice != this.Character.InputDevice)
            {
                this.movementController.InputDevice = this.Character.InputDevice;
            }

            if (this.projectileParent == null)
            {
                this.projectileParent = new GameObject(this.Character.Name + "_Bullets");
            }
        }

        private void FireWeapon(IWeapon weapon)
        {
            if (weapon == null || !weapon.CanFire())
            {
                return;
            }

            IList<ProjectileBehavior> newProjectiles = weapon.Fire(this.gameObject, this.Character);
            if (newProjectiles == null)
            {
                return;
            }

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
            this.nextMove = didUpdate;

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
            {
                this.changeTime = Time.time;
            }
        }
    }
}
