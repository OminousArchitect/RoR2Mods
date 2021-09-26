using EntityStates;
using RoR2;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class FinalHour : BaseSkillState
    {
        public override void OnEnter()
        {
            var FHbuff = Assets.serialcontentpack.buffDefs[0];
            var vaynebody = Prefabs.vayneprefab.GetComponent<CharacterBody>();
            vaynebody.AddTimedBuff(Projectiles.FinalHour, 8f);
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}