using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataCollector : MonoBehaviour {

    //the input field in the start scenes
    public InputField inputField;
    //the id of the user
    public string dataID = "";
    public static DataCollector Instance;

    //bool to control when the collector starts recording data
    public bool startRecording = false;
    //how often the collector writes into the file in data/second
    public float dataRecordInterval = 1f;
    float time = 0f;

    string currentPath;
    
    //the players
    public GameObject user;

    private void Awake()
    {
        //dont destroy the datacollector through scenes
        DontDestroyOnLoad(this);
        //get the input field in the scene
        AssignInputField();
    }

    // Use this for initialization
    void Start () {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        if(startRecording)
        {
            time += Time.deltaTime;
            if (time > dataRecordInterval)
            {
                time = 0;
                //add the new data sets into the csv
                StreamWriter sw = File.AppendText(GetPath());
                sw.WriteLine(GenerateData());
                sw.Close();
            }
        }
	}

    //when input get submited
    void OnInputSubmitCallback()
    {
        Debug.Log("change scene");
        //assign the data id
        dataID = inputField.text;
        //create the csv
        CreateCSV();
        //start recording the user tracking data
        startRecording = true;
        //load the next scene
        SceneManager.LoadScene("MainScene");
    }

    void AssignInputField()
    {
        //wew find the object
        //probably shouldnt do like tat
        inputField = FindObjectOfType<InputField>();
        //add the callback function for on submit
        inputField.onEndEdit.AddListener(delegate { OnInputSubmitCallback(); });
    }

    string GenerateData()
    {
        //get the time in hour, min, second, millisecond
        string data = "";
        data += System.DateTime.Now.ToString("HH");
        data += ":";
        data += System.DateTime.Now.ToString("mm");
        data += ":";
        data += System.DateTime.Now.ToString("ss");
        data += ":";
        data += System.DateTime.Now.ToString("FFF");
        data += ",";
        //gets the position
        string posstr = user.GetComponent<SteamVR_Camera>().head.transform.position.ToString("F3");
        //change the commas cuz csv shit
        data += ChangeLetters(posstr, ',', '.');
        data += ",";
        //get the rotations
        string rotstr = user.GetComponent<SteamVR_Camera>().head.transform.rotation.ToString("F3");
        data += ChangeLetters(rotstr, ',', '.');
        return data;
    }

    //returns the file path being used to store the data
    private string GetPath()
    {
        if (currentPath == "")
        {
            //if the filepath already exists, create a new file with a duplicate number
            string filePath = Application.dataPath + "/Data/" + dataID + ".csv";
            int duplicateCounts = 0;
            while (true)
            {
                if (File.Exists(filePath))
                {
                    ++duplicateCounts;
                    filePath = Application.dataPath + "/Data/" + dataID + "(" + duplicateCounts.ToString() + ")" + ".csv";
                }
                else
                    break;
            }
            currentPath = filePath;
            return filePath;
        }
        else
            return currentPath;
    }

    void CreateCSV()
    {
        //create a new csv for the user
        StreamWriter output = System.IO.File.CreateText(GetPath());
        output.WriteLine("Time, Position, Rotation");
        output.Close();
    }

    //need this as csv cant use commas .-.
    string ChangeLetters(string str, char letter, char toBeLetter)
    {
        char[] ret = str.ToCharArray();
        for(int i = 0; i < ret.Length; ++i)
        {
            if(ret[i] == letter)
            {
                ret[i] = toBeLetter;
            }
        }
        return new string(ret);
    }
}
