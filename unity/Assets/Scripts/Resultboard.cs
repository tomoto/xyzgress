using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resultboard : MonoBehaviour {
    public enum FinalResult
    {
        Win, Lose, Draw
    }

    public void DisplayFinalResult(FinalResult result, MedalRow.MedalType[] medalTypes)
    {
        var text = result.ToString().ToUpper();
        transform.Find("FinalResult").GetComponent<Text>().text = text;

        var medalRows = transform.GetComponentsInChildren<MedalRow>();
        var i = 0;
        foreach (var medalType in medalTypes)
        {
            medalRows[i++].SetMedalType(medalType);
        }
    }

    public void Show()
    {
        Vector2 targetPosition = GameObject.Find(GameConstants.MainCamera).transform.position;
        transform.position = transform.position.Set2D(targetPosition);
        gameObject.SetActive(true);
    }

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
