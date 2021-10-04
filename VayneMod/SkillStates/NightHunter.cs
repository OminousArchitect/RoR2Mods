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
        public float speedincrease = 2f;
        public float radius = 30f;
        public float searchrate = 5f;
        private float _stopwatch;
        private float _oldDistance;
        public CharacterBody _body;
        private BullseyeSearch _search;
        public CharacterMotor _motor;
        
        public Distance changeInDirection;

        public void Awake()
        {
            _search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(TeamIndex.Monster),
                maxDistanceFilter = radius,
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false,
                maxAngleFilter = 45f
            };

            _body = GetComponent<CharacterBody>();
            _motor = GetComponent<CharacterMotor>();
        }

        private HurtBox DoSearch()
        {
            _search.searchOrigin = transform.position;
            _search.searchDirection = _motor.velocity.normalized;
            _search.RefreshCandidates();
            _search.FilterOutGameObject(Prefabs.vayneprefab); // This needs to come after candidates are refreshed
            
            /* instead of using you could do this
            var doSearch = DoSearch();
            var results = doSearch.Current;
            doSearch.Dispose();
            */
            using (var results = _search.GetResults().GetEnumerator()) // You have to dispose of enumerables when you're done with them using automatically does this.
            {
                //Debug.LogWarning($"{results.Current}");
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
                    var deltaDist = currentDist - _oldDistance; // Might need to swap the substraction around here
                    if (deltaDist > 0.001f) // Most likely condition first
                        changeInDirection = Distance.Closer; // moving towards
                    else if (deltaDist < -0.001f)
                        changeInDirection = Distance.Further; // moving away
                    else
                        changeInDirection = Distance.None; // no change
                    _oldDistance = currentDist;
                }
            }
            
            var currentmovespeed = _body.moveSpeed;
            Debug.Log($"{_search.GetResults().GetEnumerator().Current}" );
        }

        public enum Distance
        {
            None,
            Closer,
            Further
        }
    }
}