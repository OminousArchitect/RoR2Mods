using System;
using System.Linq;
using R2API;
using RoR2;
using UnityEngine;

namespace VayneMod
{
    public class NightHunter : MonoBehaviour
    {
        private CharacterBody body;
        private float stopwatch;
        private float radius = 5f;
        private float searchrate = 5f;
        private int speedincrease = 2;
        
        public void Awake()
        {
            body = GetComponent<CharacterBody>();
        }

        public void NightHunterBehaviour()
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                
                teamMaskFilter = TeamMask.GetEnemyTeams(TeamIndex.Monster),
                maxDistanceFilter = radius,
                searchOrigin = body.transform.position,
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false
                
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(Prefabs.vayneprefab);

            HurtBox beingHunted = bullseyeSearch.GetResults().First<HurtBox>();

            if (beingHunted)
            {
                RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
                {
                    args.baseMoveSpeedAdd += sender.GetComponent<NightHunter>()?.speedincrease ?? 0f;
                };
            }
        }
        
        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= 1f / searchrate)
            {
                NightHunterBehaviour();
            }
        }
    }
}