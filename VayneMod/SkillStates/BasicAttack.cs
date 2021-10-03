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
        private float tumbleMult;
        private float totalCoeff;
        public override void OnEnter()
        {
            tumbled = base.characterBody.HasBuff(Buffs.Tumble);
            baseDuration = 0.75f;
            baseDelayBeforeFiringProjectile = 0.2f;
            projectilePrefab = Assets.maincontentpack.projectilePrefabs[0]; // Boltprefab
            force = 75f;
            targetMuzzle = "Muzzle";
            _projectileDamage = projectilePrefab.GetComponent<ProjectileDamage>();
            switch (step)
            {
                case 0:
                case 1:
                    _projectileDamage.damageType = DamageType.Generic;
                    break;
                case 2:
                    _projectileDamage.damageType = DamageType.BypassArmor;
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
            animator = base.GetModelAnimator();
            animator.SetBool("attacking", true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            animator.SetBool("attacking", false);
            base.OnExit();
        }

        public override void PlayAnimation(float duration)
        {
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
                base.PlayCrossfade("Gesture, Override", "Fire" + (1 + step), "Slash.playbackRate", base.duration, 0.15f);
            }
            base.PlayAnimation(duration);
        }

        public void SetStep(int i)
        {
            step = i;
        }
    }
}