using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson {
    [RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent (typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;
        public Transform[] targets;
        public int current; // target to aim for
        public bool racing = false;

        private void Start () {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent> ();
            character = GetComponent<ThirdPersonCharacter> ();

            agent.updateRotation = false;
            agent.updatePosition = true;
            Array.Sort(targets, CompareTransform);
            // Array.Sort(targets, delegate(Transform node1, Transform node2) {
            //     return node1.Name[]
            // });
        }

        private void Update () {
            if (racing) {
                if (target != null) {
                    Vector3 leveledPos = target.position;
                    leveledPos.y = character.transform.position.y;
                    agent.SetDestination (leveledPos);
                }
                if (agent.remainingDistance > agent.stoppingDistance) {
                    character.Move (agent.desiredVelocity, false, false);
                } else {
                    current++;
                    current %= targets.Length;
                    target = targets[current];
                    agent.SetDestination (target.position);
                }
            }

        }

        public void SetTarget (Transform target) {
            this.target = target;
        }

        private static int CompareTransform (Transform A, Transform B) {
            return A.name.CompareTo(B.name);
        }
    }
}