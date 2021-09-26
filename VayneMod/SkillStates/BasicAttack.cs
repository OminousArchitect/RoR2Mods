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
        public override void OnEnter()
        {
            baseDuration = 0.75f;
            baseDelayBeforeFiringProjectile = 0.2f;
            projectilePrefab = Assets.maincontentpack.projectilePrefabs[0]; // Boltprefab
            damageCoefficient = 1.75f;
            force = 75f;
            targetMuzzle = "Muzzle";
            _projectileDamage = projectilePrefab.GetComponent<ProjectileDamage>();
            switch (step)
            {
                case 0:
                    Debug.Log("Normal 1");
                    _projectileDamage.damageType = DamageType.Generic;
                    break;
                case 1:
                    Debug.Log("Normal 2");
                    _projectileDamage.damageType = DamageType.Generic;
                    break;
                case 2:
                    Debug.Log("Silver bolt");
                    _projectileDamage.damageType = DamageType.BypassArmor;
                    break;
            }
            animator = base.GetModelAnimator();
            this.animator.SetBool("attacking", true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            animator.SetBool("attacking", false);
            base.OnExit();
        }

        public override void PlayAnimation(float duration)
        {
            var body = Prefabs.vayneprefab.GetComponent<CharacterBody>();
            if (body.HasBuff(Projectiles.FinalHour))
            {
                PlayAnimation("Gesture, Override", "UltAttack", "Slash.playbackRate", base.duration);
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