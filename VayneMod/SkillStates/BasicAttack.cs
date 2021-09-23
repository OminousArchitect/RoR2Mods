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
                case 1:
                    Debug.Log("Normal");
                    _projectileDamage.damageType = DamageType.Generic;
                    break;
                case 2:
                    Debug.Log("Silver bolt");
                    _projectileDamage.damageType = DamageType.BypassArmor;
                    break;
            }
            
            base.OnEnter();
        }

        public void SetStep(int i)
        {
            step = i;
        }
    }
}