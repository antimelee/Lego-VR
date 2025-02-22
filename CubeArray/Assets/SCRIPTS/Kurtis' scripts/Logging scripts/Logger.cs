﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour {

    float updateRate = 1;
    float cTime;
    float nTime;

    float cTimeU;
    float nTimeU;

    public GameObject cameraRig;
    public GameObject controllerLeft;
    public GameObject controllerRight;
    public GameObject headCam;
    public trial_Info bank;

    public string filename;
    System.IO.FileStream logFile;

    public struct posTime
    {
        public Vector3 p;
        public Vector3 a;
        public float t;
    }

    public static List<Vector2> grabs;
    public static List<Vector2> flyTouchs;
    public static List<Vector2> teleports;

    public static List<posTime> rigPositions;
    public static List<posTime> leftHPositions;
    public static List<posTime> rightHPositions;




    // Use this for initialization
    void Start () {


        StartCoroutine(setup_file());

        rigPositions = new List<posTime>();
        leftHPositions = new List<posTime>();
        rightHPositions = new List<posTime>();

        grabs = new List<Vector2>();
        flyTouchs = new List<Vector2>();
        teleports = new List<Vector2>();
        cTime = nTime = cTimeU = nTimeU = Time.time;



	}

    IEnumerator setup_file()
    {
        bool isSetup = false;
        do
        {
            var check = bank.participant_name;
            if (bank.result_filename == null || bank.participant_name == null)
                yield return null;
            else
            {
                filename = "Log_" + bank.result_filename + ".csv" ;
                using (StreamWriter sw = File.AppendText(filename))
                {
                        sw.Write("Date,Participant_name,Event_type,start_time,end_time,x_pos,y_pos,z_pos,x_angle,y_angle,z_angle,Trial,task\n");
                }
                // logFile = File.Open(filename, System.IO.FileMode.Append);
                isSetup = true;
            }
        } while (!isSetup);
    }
	
	void FixedUpdate()
    {
        cTimeU = Time.time;

        if (cTimeU > nTimeU)
        {
            StartCoroutine(logData(filename));
            nTimeU = cTimeU + updateRate * 10;
        }
    }
    
    // Update is called once per frame
	void Update () {

        cTime = Time.time;

        if (cTime > nTime)
        {
            posTime temp = new posTime();
            temp.p = headCam.transform.position;
            temp.a = headCam.transform.transform.forward;
            temp.t = Time.time;
            rigPositions.Add(temp);

            posTime tempR = new posTime();
            tempR.p = controllerRight.transform.position;
            tempR.a = controllerRight.transform.transform.forward;
            tempR.t = Time.time;
            rightHPositions.Add(tempR);

            posTime tempL = new posTime();
            tempL.p = controllerLeft.transform.position;
            tempL.a = controllerLeft.transform.transform.forward;
            tempL.t = Time.time;
            leftHPositions.Add(tempL);

            nTime = cTime + updateRate;
        }
    }

    void OnApplicationQuit()
    {
        if(this.enabled == true)
        logDataFull(filename);
    }

    IEnumerator logData(string filename)
    {
        using (StreamWriter sw = File.AppendText(filename))
        {
           
            if (!File.Exists(filename))
                sw.Write("Date,Participant_name,Event_type,start_time,end_time,x_pos,y_pos,z_pos,x_angle,y_angle,z_angle,Trial,task\n");
            string format_string = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n";

            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;

            foreach (posTime v in rigPositions)
            {
                sw.Write( string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "HeadPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z, bank.trial_number,bank.tName));
            }
            rigPositions.Clear();
            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;

            foreach (posTime v in rightHPositions)
            {
                sw.Write( string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "RightHPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z, bank.trial_number, bank.tName));
            }
            rightHPositions.Clear();
            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;

            foreach (posTime v in leftHPositions)
            {
                sw.Write( string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "LeftHPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z,  bank.trial_number,bank.tName));
            }
            leftHPositions.Clear();
            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;

            foreach (Vector2 t in grabs)
            {
                sw.Write( string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "Grab", t.x, t.y, "", "", "", "", "", "",  bank.trial_number,bank.tName));
            }
            grabs.Clear();
            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;
            foreach (Vector2 t in flyTouchs)
            {
                sw.Write( string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "FlyEvent", t.x, t.y, "", "", "", "", "", "",  bank.trial_number, bank.tName));
            }
            flyTouchs.Clear();

            if (1 / Time.smoothDeltaTime <= 50)
                yield return null;
            foreach (Vector2 t in teleports)
            {
                string event_type = "";
                if (t.y == 1)
                {
                    event_type = "TeleportMM";
                }
                else
                {
                    event_type = "Teleport";
                }
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, event_type, t.x, t.x, "", "", "", "", "", "", bank.trial_number,bank.tName));
            }
            teleports.Clear();
        }
    }

    void logDataFull(string filename)
    {
        using (StreamWriter sw = File.AppendText(filename))
        {
            
            if (!File.Exists(filename))
                sw.Write("Date,Participant_name,Event_type,start_time,end_time,x_pos,y_pos,z_pos,x_angle,y_angle,z_angle,Trial,task,\n");
            string format_string = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n";

            foreach (posTime v in rigPositions)
            {
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "HeadPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z, bank.trial_number, bank.tName));
            }
            rigPositions.Clear();

            foreach (posTime v in rightHPositions)
            {
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "RightHPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z, bank.trial_number, bank.tName));
            }
            rightHPositions.Clear();

            foreach (posTime v in leftHPositions)
            {
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "LeftHPosition", v.t.ToString(), v.t.ToString(), v.p.x, v.p.y, v.p.z, v.a.x, v.a.y, v.a.z, bank.trial_number, bank.tName));
            }
            leftHPositions.Clear();

            foreach (Vector2 t in grabs)
            {
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "Grab", t.x, t.y, "", "", "", "", "", "", bank.trial_number, bank.tName));
            }
            grabs.Clear();
            foreach (Vector2 t in flyTouchs)
            {
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, "FlyEvent", t.x, t.y, "", "", "", "", "", "", bank.trial_number, bank.tName));
            }
            flyTouchs.Clear();

            foreach (Vector2 t in teleports)
            {
                string event_type= "";
                if(t.y == 1)
                {
                    event_type = "TeleportMM";
                }
                else
                {
                    event_type = "Teleport";
                }
                sw.Write(string.Format(format_string, System.DateTime.Now.ToString(), bank.participant_name, event_type, t.x, t.x, "", "", "", "", "", "", bank.trial_number, bank.tName));
            }
            teleports.Clear();
        }
    }
}
