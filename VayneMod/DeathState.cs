using EntityStates;
using UnityEngine;

namespace VayneMod
{
    public class DeathState : GenericCharacterDeath
    {
        
        public override void PlayDeathAnimation(float crossfadeDuration)
        {
            if (isPlayerDeath)
            {
                base.PlayAnimation("Body","Death");
                Debug.Log("This is firing");
            }
            base.PlayDeathAnimation(crossfadeDuration);
        }
    }
}