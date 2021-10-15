using EntityStates;
using RoR2;
using UnityEngine;

namespace VayneMod.SkillStates
{
    public class FinalHour : BaseSkillState
    {
        public override void OnEnter()
        {
            base.characterBody.AddTimedBuff(Buffs.FinalHour, 15f);
            AkSoundEngine.PostEvent(133898581, gameObject);
            base.OnEnter();
        }
    }
}