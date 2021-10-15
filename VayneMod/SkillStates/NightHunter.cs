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
        public float speedincrease;
        public float searchrate = 8f;
        
        private float _stopwatch;
        private BullseyeSearch _search;
        private float _oldDistance;
        
        public Direction changeInDirection;

        public void Awake()
        {
            //characterMotor = GetComponent<CharacterMotor>();
            //body = GetComponent<CharacterBody>();
            
            _search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetUnprotectedTeams(body.teamComponent.teamIndex),
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false,
                maxDistanceFilter = 40f,
                maxAngleFilter = 65f,
            };
        }

        private HurtBox DoSearch()
        {
            _search.searchOrigin = transform.position;
            _search.searchDirection = characterMotor.velocity.normalized;
            _search.RefreshCandidates();
            _search.FilterOutGameObject(gameObject); // This needs to come after candidates are refreshed
            
            //Debug.Log("doing search");
            /* instead of using you could do this
            var test = DoSearch();
            var test2 = test.Current;
            test.Dispose();
            */
            return _search.GetResults().FirstOrDefault();
            //using (var results = _search.GetResults().GetEnumerator()) // You have to dispose of enumerables when you're done with them using automatically does this.
            {
                //return results.Current;
            }
        }

        public void FixedUpdate()
        {
            if (body.HasBuff(Buffs.FinalHour))
            {
                speedincrease = 4f;
            }
            else
            {
                speedincrease = 3f;
            }
            _stopwatch += Time.fixedDeltaTime;
            //Debug.Log(_stopwatch + "/" + 1f / searchrate);
            if (_stopwatch >= 1f / searchrate)
            {
                _stopwatch -= 1f / searchrate;
                var hurtBox = DoSearch();
                if (hurtBox)
                {
                    Debug.Log("found hurtbox");
                    var currentDist = Vector3.Distance(transform.position, hurtBox.healthComponent.transform.position);
                    var deltaDist = _oldDistance - currentDist; // Might need to swap the substraction around here
                    if (deltaDist > 0.001f) // Most likely condition first
                        changeInDirection = Direction.Closer; // moving towards
                    else if (deltaDist < -0.001f)
                        changeInDirection = Direction.Further; // moving away
                    else
                        changeInDirection = Direction.None; // no change
                    Debug.Log($"{speedincrease}");
                    _oldDistance = currentDist;
                }
                else
                {
                    changeInDirection = Direction.None;
                }
                body.RecalculateStats();
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