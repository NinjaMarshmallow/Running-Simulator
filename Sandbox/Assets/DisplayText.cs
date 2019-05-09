using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace UnityStandardAssets.Utility {
    [RequireComponent (typeof (Text))]
    public class DisplayText : MonoBehaviour {

        private Text text;
        private Timer timer;
        private int time;

        private GameObject[] opponents;
        private List<AICharacterControl> scripts = new List<AICharacterControl> ();
        public RunnerMove playerScript;
        private bool finished = false;
        private int place = -1;

        private void Start () {
            playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<RunnerMove> ();
            opponents = GameObject.FindGameObjectsWithTag ("npc");
            foreach (var opp in opponents) {
                scripts.Add (opp.GetComponent<AICharacterControl> ());
            }
            scripts.ForEach (script => {
                script.stopRunning ();
            });
            playerScript.stopRunning ();
            text = GetComponent<Text> ();
            text.text = "3";
            timer = new Timer (1000);
            timer.Start ();
            time = 0;
        }

        private void Update () {
            if (!finished) {
                time++;
                if (time % 60 == 0 && text.text != "") {
                    changeText ();
                }
                if(text.text == "") {
                    timer.Stop();
                }
            }
        }

        public void displayFinishPlace (int place) {
            this.place = place;
            text.fontSize += 40;
            if (place == 1) {
                text.text = "1st!";
            } else if (place == 2) {
                text.text = "2nd!";
            } else if (place == 3) {
                text.text = "3rd!";
            } else {
                text.text = place + "th";
            }
            finished = true;
        }

        private void changeText () {
            if (text.text.Equals ("3")) {
                text.text = "2";
            } else if (text.text.Equals ("2")) {
                text.text = "1";
            } else if (text.text.Equals ("1")) {
                text.text = "Go!";
                startRace ();
            } else {
                text.text = "";
            }
        }

        private void startRace () {
            scripts.ForEach (script => {
                script.startRunning ();
            });
            playerScript.startRunning ();
        }
    }
}