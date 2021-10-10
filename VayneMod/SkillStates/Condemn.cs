using EntityStates;
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
            force = 15f;
            targetMuzzle = "Muzzle";
            base.OnEnter();
        }

        public override void PlayAnimation(float duration)
        {
            PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", base.duration);
            base.PlayAnimation(duration);
        }
    }
}