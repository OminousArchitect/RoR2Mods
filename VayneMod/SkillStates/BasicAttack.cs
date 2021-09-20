using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class BasicAttack : GenericProjectileBaseState
    {
        public override void OnEnter()
        {
            baseDuration = 0.75f;
            baseDelayBeforeFiringProjectile = 0.2f;
            projectilePrefab = Assets.maincontentpack.projectilePrefabs[0]; // Boltprefab
            damageCoefficient = 1.75f;
            force = 75f;
            targetMuzzle = "Muzzle";
            base.OnEnter();
        }
        
        // duration = 2f / 1.15     = 1.73
        // delayBefore = 0.2 / 1.15 = 0.17
        //
        // FixedUpdate()
        // if this.stopwatch >= 1.73(delayBefore) && !firedProjectile
        // { fire your prefab }
        //
        // "Cancel"
        // if this.stopwatch >= 0.17(duration) && base.isAuthority
        // {
        //  this.outer.SetNextStateToMain();
        //  return;
        // }
    }
}