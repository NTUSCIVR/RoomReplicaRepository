using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//manage the events that occurs in the application
public class GameController : MonoBehaviour {

    public static GameController Instance;
    //bool to control the drop
    bool dropped = false;
    //the player
    public GameObject cameraRig;
    //the parent of the falling cube objects
    public GameObject collaspeFloor;

    //bool to check if the player is falling
    public bool fall = false;
    //speed of falling
    float fallSpeed = 0f;

    //the id of the user
    string userID;

    private void Awake()
    {
        Instance = this;
        //find the steamvr eye and assign it to data collector
        if (DataCollector.Instance != null)
        {
            DataCollector.Instance.user = FindObjectOfType<SteamVR_Camera>().gameObject;
            userID = DataCollector.Instance.dataID;
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //to make the floor drops
        if(Input.GetKey(KeyCode.Return))
        {
            if (!dropped)
            {
                //play the hole opening audio
                AudioController.Instance.PlaySingle(AudioController.Instance.openHole);
                EngageFloorDrop();
                ActiveFallZones();
            }
        }
        //restart the scene
        if(Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("StartScene");
            //destroy datacollector as a new one get created in startscene
            Destroy(DataCollector.Instance.gameObject);
        }

        //if the user is falling
        if(fall)
        {
            //accelerate them falling to simulate gravity
            fallSpeed += 10f * Time.deltaTime;
            //move the whole camera rig
            cameraRig.transform.position = new Vector3(0f, cameraRig.transform.position.y - fallSpeed * Time.deltaTime, 0f);
            if(cameraRig.transform.position.y < -10)
            {
                //when it reaches the bottom stop making the player fall
                //play the fell audio
                AudioController.Instance.PlaySingle(AudioController.Instance.fall);
                fall = false;
                //sets the y position
                cameraRig.transform.position = new Vector3(0f, -7f, 0);
            }
        }
    }
    
    void EngageFloorDrop()
    {
        dropped = true;
        //itterate through all of the children to disable kinematci and enable gravity
        for (int i = collaspeFloor.transform.childCount - 1; i >= 0; --i)
        {
            //make every cube object under collaspe floor drop
            Rigidbody childRB = collaspeFloor.transform.GetChild(i).GetComponent<Rigidbody>();
            childRB.isKinematic = false;
            childRB.useGravity = true;
        }
    }

    //activate all fallzones that is a child of game controller
    void ActiveFallZones()
    {
        for(int i = transform.childCount - 1; i >= 0; --i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if(child.tag == "FallArea")
            {
                child.SetActive(true);
            }
        }
    }
}
