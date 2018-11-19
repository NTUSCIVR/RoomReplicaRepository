using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankEvent : MonoBehaviour {

    bool fall = false;
    [SerializeField]
    GameObject plankA;
    [SerializeField]
    GameObject plankB;
    [SerializeField]
    GameObject fallArea;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if press one on keyboard, will drop the plank
		if(Input.GetKey(KeyCode.Alpha1))
        {
            if(!fall)
            {
                fall = true;
                //enable the plank rb
                plankA.GetComponent<Rigidbody>().isKinematic = false;
                plankB.GetComponent<Rigidbody>().isKinematic = false;
                //make the fall area around the plank active
                fallArea.SetActive(true);
            }
        }
	}
}
