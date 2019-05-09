using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class DetermineResults : MonoBehaviour {
    private List<AICharacterControl> scripts = new List<AICharacterControl> ();
    private RunnerMove playerScript;
    private Collider playerCollider;
    private GameObject player;
    private GameObject[] opponents;
    public int NumberOfRacers;
    private List<GameObject> finishers;
    private bool placingComplete = false;
    private Collider checkpoint;
    private bool playerPastCheckpoint = false;
    // Start is called before the first frame update
    void Start () {
        checkpoint = GameObject.Find("Checkpoint").GetComponent<Collider>();
        finishers = new List<GameObject> ();
        if (NumberOfRacers == 0) NumberOfRacers = 10;
        player = GameObject.FindGameObjectWithTag ("Player");
        playerCollider = player.GetComponent<Collider> ();
        playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<RunnerMove> ();
        opponents = GameObject.FindGameObjectsWithTag ("npc");
        foreach (var opp in opponents) {
            scripts.Add (opp.GetComponent<AICharacterControl> ());
        }
    }

    // Update is called once per frame
    void Update () {
        if(checkpoint.bounds.Intersects(playerCollider.bounds)) {
            playerPastCheckpoint = true;
        } else {
            if(playerPastCheckpoint) {
                playerScript.laps--;
                playerPastCheckpoint = false;
            }
        }
        if (!placingComplete) {

            if (GetComponent<Collider> ().bounds.Intersects (playerCollider.bounds) && !finishers.Contains (player) && playerScript.laps <= 0) {
                finishers.Add (player);
            }
            foreach (var opp in opponents) {
                if (GetComponent<Collider> ().bounds.Intersects (opp.GetComponent<Collider> ().bounds) && !finishers.Contains (opp) && opp.GetComponent<AICharacterControl>().laps <= 0) {
                    finishers.Add (opp);
                }
            }
            if (finishers.Count >= NumberOfRacers) {
                foreach (var opp in opponents) {
                    AICharacterControl script = opp.GetComponent<AICharacterControl> ();
                    script.setPlace (finishers.IndexOf (opp) + 1);
                }
                playerScript.setPlace (finishers.IndexOf (player) + 1);
                placingComplete = true;
            }
        }
    }
}