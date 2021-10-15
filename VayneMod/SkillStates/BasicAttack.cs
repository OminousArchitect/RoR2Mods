using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class BasicAttack : AimThrowableBase, SteppedSkillDef.IStepSetter
    {
        public int step;
        private ProjectileDamage _projectileDamage;
        private Animator animator;
        private bool tumbled;
        private bool isFH;
        private float tumbleAdd;
        private float baseAD;
        private float magnitude;
        private HuntressTracker _tracker;
        private GameObject bolt;

        public override void OnEnter()
        {
            if (isFH)
            {
                baseAD = 2.6f;
                tumbleAdd = 0.4f;
            }
            else
            {
                baseAD = 2.4f;
                tumbleAdd = 0.2f;
            }

            animator = GetModelAnimator();
            tumbled = characterBody.HasBuff(Buffs.Tumble);
            isFH = characterBody.HasBuff(Buffs.FinalHour);
            projectilePrefab = Assets.maincontentpack.projectilePrefabs[0];
            _tracker = GetComponent<HuntressTracker>();
            _projectileDamage = projectilePrefab.GetComponent<ProjectileDamage>();
            projectileBaseSpeed = 20f;
            baseMinimumDuration = 0.75f;
            minimumDuration = 0.18f;

            if (tumbled)
            {
                damageCoefficient = baseAD + tumbleAdd;
            }
            else
            {
                damageCoefficient = baseAD;
            }

            switch (step)
            {
                case 0:
                case 1:
                    _projectileDamage.damageType = DamageType.Generic;
                    //projectilePrefab.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan; //todo returns null for some reason
                    break;
                case 2:
                    _projectileDamage.damageType = DamageType.BypassArmor;
                    //projectilePrefab.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<MeshRenderer>().material.color = Color.green; 
                    break;
                //Todo Silver bolt is purple, final hour bolts are ???
            }

            //Debug.LogWarning($"{damageCoefficient}");
            animator.SetBool("attacking", true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            animator.SetBool("attacking", false);
            animator.SetBool("hasTumbled", false);
            characterBody.RemoveBuff(Buffs.Tumble);
            base.OnExit();
        }

        public override void FireProjectile()
        {
            HandleAnimations(minimumDuration);
            base.FireProjectile();
        }

        public void HandleAnimations(float minimumDuration)
        {
            if (characterBody.HasBuff(Buffs.FinalHour) && characterBody.HasBuff(Buffs.Tumble))
            {
                PlayAnimation("UltimateAttack", "TumbleUltAttack", "Slash.playbackRate", base.minimumDuration);
            }

            if (base.characterBody.HasBuff(Buffs.FinalHour))
            {
                PlayAnimation("UltimateAttack", "UltAttack", "Slash.playbackRate", base.minimumDuration);
            }
            else if (base.characterBody.HasBuff(Buffs.Tumble))
            {
                PlayAnimation("Gesture, Override", "TumbleAttack", "Slash.playbackRate", base.minimumDuration);
                AkSoundEngine.PostEvent(517366917, gameObject);
            }
            else
            {
                PlayCrossfade("Gesture, Override", "Fire" + (1 + step), "Slash.playbackRate", base.minimumDuration, 0.15f);
                AkSoundEngine.PostEvent(275010454, gameObject);
            }
        }

        public override void Update()
        {
            age += Time.deltaTime;

            if (CameraRigController.IsObjectSpectatedByAnyCamera(base.gameObject))
            {
                UpdateTrajectoryInfo(out currentTrajectoryInfo);
            }
        }

        public override void FixedUpdate()
        {
            fixedAge += Time.fixedDeltaTime;

            if (base.isAuthority && base.fixedAge >= minimumDuration)
            {
                UpdateTrajectoryInfo(out currentTrajectoryInfo);
                EntityState entityState = PickNextState();
                if (entityState != null)
                {
                    outer.SetNextState(entityState);
                    return;
                }

                outer.SetNextStateToMain();
            }
        }

        public void SetStep(int i)
        {
            step = i;
        }
    }
}