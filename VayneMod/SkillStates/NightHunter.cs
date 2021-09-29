using System;
using System.Collections.Generic;
using System.Linq;
using R2API;
using RoR2;
using UnityEngine;

namespace VayneMod
{
    public class NightHunter : MonoBehaviour
    {
        // Filled via editor 
        public CharacterBody body;
        public CharacterMotor characterMotor;
        public float speedincrease = 2f;
        public float radius = 5f;
        public float searchrate = 5f;
        
        private float _stopwatch;
        private BullseyeSearch _search;
        private float _oldDistance;
        
        public Direction changeInDirection;

        public void Awake()
        {
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                var component = sender.GetComponent<NightHunter>();
                if (component)
                    if(component.changeInDirection == Direction.Closer)
                        args.baseMoveSpeedAdd += component.speedincrease;
            }; // TODO move this to plugin Awake
            
            _search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(TeamIndex.Monster),
                maxDistanceFilter = radius,
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false,
                maxAngleFilter = 30f
            };
        }

        private HurtBox DoSearch()
        {
            _search.searchOrigin = transform.position;
            _search.searchDirection = characterMotor.velocity.normalized;
            _search.RefreshCandidates();
            _search.FilterOutGameObject(gameObject); // This needs to come after candidates are refreshed
            
            /* instead of using you could do this
            var test = DoSearch();
            var test2 = test.Current;
            test.Dispose();
            */
            using (var results = _search.GetResults().GetEnumerator()) // You have to dispose of enumerables when you're done with them using automatically does this.
            {
                return results.Current;
            }
        }

        public void FixedUpdate()
        {
            _stopwatch += Time.fixedDeltaTime;
            if (_stopwatch >= 1f / searchrate)
            {
                var hurtBox = DoSearch();
                if (hurtBox)
                {
                    var currentDist = Vector3.Distance(transform.position, hurtBox.healthComponent.transform.position);
                    var deltaDist = _oldDistance - currentDist; // Might need to swap the substraction around here
                    if (deltaDist > 0.001f) // Most likely condition first
                        changeInDirection = Direction.Closer; // moving towards
                    else if (deltaDist < -0.001f)
                        changeInDirection = Direction.Further; // moving away
                    else
                        changeInDirection = Direction.None; // no change
                    _oldDistance = currentDist;
                }
            }
        }

        public enum Direction
        {
            None,
            Closer,
            Further
        }
    }
}