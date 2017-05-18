using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitController : MonoBehaviour {
    public static int InstanceCount = 0;

    // Use this for initialization
    void Start () {
        InstanceCount++;
        Destroy(gameObject, 5); // shows up for 5 seconds
	}
	
	// Update is called once per frame
	void Update () {
	    if (InputUtil.GetButtonDown(0))
        {
            Screen.fullScreen = false;
            Application.ExternalCall("recruit");
            Destroy(gameObject);
        }
        else if (InputUtil.GetButtonDown(1))
        {
            Destroy(gameObject);
        }
	}
}
