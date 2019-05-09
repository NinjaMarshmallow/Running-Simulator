using System;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class Countdown : MonoBehaviour
    {
        
        private Text text;
        private Timer timer;
        private int time;

        private GameObject[] opponents;
        private List<AICharacterControl> scripts = new List<AICharacterControl>();
        public RunnerMove playerScript;

        private void Start()
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerMove>();
            opponents = GameObject.FindGameObjectsWithTag("npc");
            foreach (var opp in opponents)
            {
                scripts.Add(opp.GetComponent<AICharacterControl>());
            }
            scripts.ForEach(script => {
                script.stopRunning();
            });
            playerScript.stopRunning();
            text = GetComponent<Text>();
            text.text = "3";
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            time = 0;
        }


        private void Update()
        {
            time++;   
            if(time % 60 == 0) {
                changeText();
            }
        }

        private void changeText() {
            if(text.text.Equals("3")) {
                text.text = "2";
            } else if(text.text.Equals("2")) {
                text.text = "1";
            } else if(text.text.Equals("1")) {
                text.text = "Go!";
                startRace();
            } else {
                text.text = "";
            }
        }

        private void startRace() {
            scripts.ForEach(script => {
                script.startRunning();
            });
            playerScript.startRunning();
        }
    }
}
