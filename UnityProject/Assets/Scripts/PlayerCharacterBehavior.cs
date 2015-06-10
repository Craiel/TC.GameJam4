namespace Assets.Scripts
{
    using System;
    
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Controls;
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;
    using Assets.Scripts.Weapons;

    using JetBrains.Annotations;

    using UnityEngine;

    public class PlayerCharacterBehavior : MonoBehaviour
    {
        private IMovementController movementController;

        private GameObject projectileParent;

        private bool currentMove;

        private bool nextMove;

        private bool startupGearGenerated;

        private float changeTime;
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public bool useFixedAxisController = true;

        [SerializeField]
        public bool createCharacter = false;

        [SerializeField]
        public bool generateRandomStartupGear = false;

        [SerializeField]
        public Animator mechController;

        // Mostly needed for inspection
        public ICharacter Character { get; set; }
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void InitializeMovementController()
        {
            this.movementController = new InControlPlayerController(this.gameObject)
                                          {
                                              InputDevice = this.Character.InputDevice
                                          };

        }

        [UsedImplicitly]
        private void Update()
        {
            if (this.Character == null)
            {
                if (this.createCharacter)
                {
                    this.Character = new Character();
                }
                else
                {
                    return;
                }
            }

            if (this.generateRandomStartupGear && !this.startupGearGenerated)
            {
                /*foreach (GearType gearType in EnumLists.GearTypes)
                {
                    this.Character.SetGear(gearType, GearGeneration.GenerateRandomGear(gearType));
                }*/

                // Todo: Remove these when we are done debugging
                //this.Character.SetGear(GearType.LeftWeapon, GearGeneration.GenerateRandomWeapon(GearType.LeftWeapon, typeof(WeaponColumn)));
                this.Character.SetGear(GearType.RightWeapon, GearGeneration.GenerateRandomWeapon(GearType.RightWeapon, typeof(WeaponRanged)));
                
                this.startupGearGenerated = true;
            }

            this.UpdateBehaviorState();

            this.Character.Update(this.gameObject);

            float fireLeft = this.Character.InputDevice.GetState(PlayerControl.Fire).Value;
            float fireRight = this.Character.InputDevice.GetState(PlayerControl.Fire2).Value;
            
            if (Math.Abs(fireLeft) > float.Epsilon)
            {
                this.FireWeapon(this.Character.GetGear(GearType.LeftWeapon) as IWeapon);
            }

            if (Math.Abs(fireRight) > float.Epsilon)
            {
                this.FireWeapon(this.Character.GetGear(GearType.RightWeapon) as IWeapon);
            }

            this.movementController.Velocity = this.Character.GetCurrentStat(StatType.Velocity);
            this.movementController.RotationSpeed = this.Character.GetCurrentStat(StatType.RotationSpeed);

            bool didUpdate = this.movementController.Update();
            this.UpdateMoveAnimation(didUpdate);
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D other)
        {
            GearView gearView = other.gameObject.GetComponent<GearView>();
            if(gearView != null)
            {
                IGear currentGear = Character.GetGear(gearView.Gear.Type);
                if(currentGear == null)
                {
                    Character.SetGear(gearView.Gear.Type, gearView.Gear);
                    Arena.Arena.Instance.ClaimGear(gearView.Gear);
                }
            }
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

            var context = new WeaponFireContext
                              {
                                  ProjectileParent = this.projectileParent,
                                  Origin = this.gameObject,
                                  Character = this.Character
                              };

            weapon.Fire(context);
        }

        private void UpdateMoveAnimation(bool didUpdate)
        {
            //If there is movement our next move is true
            this.nextMove = didUpdate;

            if (this.nextMove != this.currentMove && Time.time > this.changeTime + 0.25f)
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
        }
    }
}
