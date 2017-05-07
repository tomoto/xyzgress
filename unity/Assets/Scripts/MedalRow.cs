using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalRow : MonoBehaviour {
    public enum MedalType
    {
        None, Blonze, Silver, Gold, Platinum, Onyx, NumberOfMedalTypes
    }

    public enum AchievementType
    {
        Liberator, Connector, MindController, Illuminator, Explorer, Pioneer, Hacker, NumberOfAchievements
    }

    public Material[] MedalMaterials = new Material[(int)MedalType.NumberOfMedalTypes];

    public void SetMedalType(MedalType medalType)
    {
        var material = MedalMaterials[(int)medalType];
        if (material != null)
        {
            gameObject.SetActive(true);
            transform.Find("MedalType").GetComponent<Text>().text = medalType.ToString().ToUpper();
            transform.Find("Medal").GetComponent<Renderer>().material = material;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
