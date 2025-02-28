﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.Utility;
using Unity;

namespace UnityStandardAssets.Characters.ThirdPerson {

    public class RunnerMove : MonoBehaviour {

        public float speed = 10.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;
        public float rotateSpeed = 20.0F;
        private Vector3 moveDirection = Vector3.zero;

        public int MaxEnergy;
        public float SurgeMultiplier;
        public float KickMultiplier;
        public int EnergyReload;
        public int KickLength;
        private int energy = 0;
        private int time = 0;
        private bool surging = false;
        private float normalSpeed;
        private bool kickUsed = false;
        private bool running = false;
        private AICharacterControl opponent;
        public int laps;

        private int place = -1;

        // Start is called before the first frame update
        void Start () {
            opponent = GameObject.FindGameObjectWithTag ("npc").GetComponent<AICharacterControl> ();
            laps = opponent.laps;
            normalSpeed = speed;
            if (energy == 0) {
                energy = 100;
            }
            if (KickLength == 0) {
                KickLength = 200;
            }
        }

        // Update is called once per frame
        void Update () {
            if (running) {

                speed = normalSpeed;
                if (Input.GetButton ("Fire1") && energy > MaxEnergy * .15) {
                    speed *= SurgeMultiplier;
                    surging = true;
                    energy -= EnergyReload;
                } else {
                    surging = false;
                    energy++;
                    if (energy > MaxEnergy) {
                        energy = MaxEnergy;
                    }
                }
                if (Input.GetButton ("Fire2") && KickLength > 0) {
                    speed *= KickMultiplier;
                    KickLength--;
                }
                CharacterController controller = GetComponent<CharacterController> ();
                Vector3 moveDirection = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
                moveDirection.y -= gravity * Time.deltaTime;
                moveDirection = transform.TransformDirection (moveDirection);
                moveDirection *= speed;
                //if (Input.GetButton("Jump")) moveDirection.y = jumpSpeed;

                controller.Move (moveDirection * Time.deltaTime);

                //Rotate Player
                transform.Rotate (0, Input.GetAxis ("Horizontal"), 0);
            }
        }

        public void stopRunning () {
            running = false;
        }

        public void startRunning () {
            running = true;
        }

        public void setPlace (int finish) {
            place = finish;
            running = false;
            GameObject.FindGameObjectWithTag("DisplayText").GetComponent<DisplayText>().displayFinishPlace(place);
        }
    }
}