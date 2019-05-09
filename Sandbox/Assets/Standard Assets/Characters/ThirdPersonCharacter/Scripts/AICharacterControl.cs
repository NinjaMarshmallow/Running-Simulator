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
        public int laps;
        private float normalSpeed;
        private void Start () {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent> ();
            character = GetComponent<ThirdPersonCharacter> ();

            agent.updateRotation = false;
            agent.updatePosition = true;
            Array.Sort (targets, CompareTransform);
            // Array.Sort(targets, delegate(Transform node1, Transform node2) {
            //     return node1.Name[]
            // });
            normalSpeed = agent.speed;
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

                    if (current == targets.Length - 1) {
                        laps--;
                    }
                    if (laps == 0) {
                        racing = false;
                    } else {
                        current++;
                        current %= targets.Length;

                        if (current % 3 == 0) {
                            System.Random rand = new System.Random ();
                            int delta = (rand.Next (15, 25) - 15) / 5;
                            agent.speed += 3.0f * delta;

                        } else {
                            agent.speed = normalSpeed;
                        }
                        float barnfactor = current / (targets.Length / 2);
                        if (barnfactor > 1) {
                            agent.speed += barnfactor * 2;
                        }
                        target = targets[current];
                        agent.SetDestination (target.position);
                    }

                }
            }

        }

        public void SetTarget (Transform target) {
            this.target = target;
        }

        private static int CompareTransform (Transform A, Transform B) {
            return A.name.CompareTo (B.name);
        }

        public void startRunning () {
            racing = true;
        }

        public void stopRunning () {
            racing = false;
        }
    }
}