using System;
using System.Threading.Tasks;
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
        private int place = -1;
        private float normalSpeed;
        private System.Random rand;
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
            rand = new System.Random ();
        }

        private void Update () {
            if (racing) {
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
                        Vector3 leveledPos = target.position;
                        leveledPos.x = leveledPos.x + rand.Next (0, 10) - 5;
                        leveledPos.z = leveledPos.z + rand.Next (0, 10) - 5;
                        agent.SetDestination (target.position);
                    }

                }
            } else {
                agent.SetDestination(getFinalDestination(place));
                if (agent.remainingDistance > agent.stoppingDistance) {
                    character.Move (agent.desiredVelocity, false, false);
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

        public void setPlace (int finish) {
            place = finish;
            Task.Delay (250).ContinueWith (t => racing = false);
        }

        private Vector3 getFinalDestination(int place) {
            return GameObject.Find("Podium" + place % 10).GetComponent<Transform>().position;
        }
    }
}