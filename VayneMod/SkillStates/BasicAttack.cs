using EntityStates;
using RoR2.Projectile;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class BasicAttack : GenericProjectileBaseState
    {

        private ProjectileController controller;
        public override void OnEnter()
        {
            base.projectilePrefab = Assets.maincontentpack.projectilePrefabs[0]; // Boltprefab
            base.damageCoefficient = 1.5f;
            base.force = 75f;
            base.baseDuration = 1.2f;
            base.baseDelayBeforeFiringProjectile = 0.2f;
            controller = projectilePrefab.GetComponent<ProjectileController>();
           
            Debug.LogWarning("This Skill is firing");

            if (controller.ghostPrefab == null)
            {
                Debug.LogWarning("No ghost is attached");
            }
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}