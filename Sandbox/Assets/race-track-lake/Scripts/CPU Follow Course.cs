using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUFollowCourse : MonoBehaviour
{
    public Transform[] target;
    public float speed;
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!waypointMet())  {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
            
        } else {
            current++;
            current %= target.Length;
        }
    }

    bool waypointMet() {
        Vector3 pos = transform.position;
        Vector3 targetPos =  target[current].position;
        return (pos.x == targetPos.x && pos.z == targetPos.z);
    }
}
