using System;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class Countdown : MonoBehaviour
    {
        
        private Text text;
        private Timer timer;
        private int time;

        private void Start()
        {
            text = GetComponent<Text>();
            text.text = "3";
            timer = new Timer(1000);
            //timer.Elapsed += changeText;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            time = 0;
        }


        private void Update()
        {
            time++;   
            if(time % 60 == 0) {
                text.text += "L";
                changeText();
            }
        }

        private void changeText() {
            if(text.text == "3") {
                text.text = "2";
            } else if(text.text == "2") {
                text.text = "1";
            } else if(text.text == "1") {
                text.text = "Go!";
            } else {
                text.text = "";
            }
        }
    }
}
