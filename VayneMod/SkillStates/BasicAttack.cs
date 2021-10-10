using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class BasicAttack : GenericProjectileBaseState, SteppedSkillDef.IStepSetter
    {
        public int step;
        private ProjectileDamage _projectileDamage;
        private Animator animator;
        private bool tumbled;
        private bool isFH;
        private float tumbleMult;
        private float totalCoeff;
        private GameObject bolt;
        public override void OnEnter()
        {
            animator = GetModelAnimator();
            tumbled = characterBody.HasBuff(Buffs.Tumble);
            isFH = characterBody.HasBuff(Buffs.FinalHour);
            bolt = Assets.maincontentpack.projectilePrefabs[0];
            var ghost = bolt.GetComponent<ProjectileController>().ghostPrefab;
            
            baseDuration = 0.75f;
            baseDelayBeforeFiringProjectile = 0.18f;
            force = 3000f;
            projectilePrefab = bolt;
            _projectileDamage = projectilePrefab.GetComponent<ProjectileDamage>();
            switch (step)
            {
                case 0:
                case 1:
                    _projectileDamage.damageType = DamageType.Generic;
                    projectilePrefab.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
                    break;
                case 2:
                    _projectileDamage.damageType = DamageType.BypassArmor;
                    projectilePrefab.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                    break;
            }
            if (tumbled)
            {
                damageCoefficient = 1.75f;
            }
            else
            {
                damageCoefficient = 1.5f;
            }
            animator.SetBool("attacking", true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            animator.SetBool("attacking", false);
            animator.SetBool("hasTumbled", false);
            base.OnExit();
        }

        public override void PlayAnimation(float duration)
        {
            if (characterBody.HasBuff(Buffs.FinalHour) && characterBody.HasBuff(Buffs.Tumble))
            {
                PlayAnimation("UltimateAttack", "TumbleUltAttack", "Slash.playbackRate", base.duration);
            }
            if (base.characterBody.HasBuff(Buffs.FinalHour))
            {
                PlayAnimation("UltimateAttack", "UltAttack", "Slash.playbackRate", base.duration);
            }
            else if (base.characterBody.HasBuff(Buffs.Tumble))
            { 
                PlayAnimation("Gesture, Override", "TumbleAttack", "Slash.playbackRate", base.duration);
            }
            else
            {
                PlayCrossfade("Gesture, Override", "Fire" + (1 + step), "Slash.playbackRate", base.duration, 0.15f);
            }
            base.PlayAnimation(duration);
        }

        public void SetStep(int i)
        {
            step = i;
        }
    }
}