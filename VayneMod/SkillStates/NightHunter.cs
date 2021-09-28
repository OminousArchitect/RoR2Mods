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

        private BullseyeSearch search;
        private float oldDistance;
        private int changeInDirection;

        public void Awake()
        {
            body = GetComponent<CharacterBody>();
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                args.baseMoveSpeedAdd += sender.GetComponent<NightHunter>()?.speedincrease ?? 0f;
            };
            
            search = new BullseyeSearch
            {
                
                teamMaskFilter = TeamMask.GetEnemyTeams(TeamIndex.Monster),
                maxDistanceFilter = radius,
                searchOrigin = body.transform.position,
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false
                
            };
            search.FilterOutGameObject(Prefabs.vayneprefab);
            search.RefreshCandidates();
        }

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= 1f / searchrate)
            {
                search.RefreshCandidates();
                using (var results = search.GetResults().GetEnumerator())
                {
                    if (results.Current != null)
                    {
                        var currentDist = Vector3.Distance(body.corePosition, results.Current.transform.position);
                        changeInDirection = Mathf.RoundToInt(Mathf.Clamp(oldDistance - currentDist, -1, 1)); // Might need to swap the substraction around here
                        oldDistance = currentDist;
                    }
                }
            }
        }
    }
}