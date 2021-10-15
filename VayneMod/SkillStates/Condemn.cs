using EntityStates;
using RoR2;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class Condemn : GenericProjectileBaseState
    {
        public override void OnEnter()
        {
            baseDuration = 0.75f;
            baseDelayBeforeFiringProjectile = 0.2f;
            projectilePrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("silverboltprefab"); 
            damageCoefficient = 1.75f;
            force = 3000f;
            base.OnEnter();
        }

        /*private void DoBlastAttack()
        {
            BlastAttack blast = new BlastAttack();
            blast.attacker = gameObject;
            blast.crit = RollCrit();
            blast.inflictor = gameObject;
            blast.position = characterDirection.forward;
            blast.radius = 4f;
            blast.baseDamage = damageCoefficient * this.damageStat;
            blast.baseForce = 3000f;
            blast.damageType = DamageType.Generic;
        }*/
        
        public override void PlayAnimation(float duration)
        {
            PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", base.duration);
            base.PlayAnimation(duration);
        }
    }
}