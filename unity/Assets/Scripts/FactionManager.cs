using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class FactionManager : MonoBehaviour {
    public static FactionManager GetInstance()
    {
        return GameObject.Find("FactionManager").GetComponent<FactionManager>();
    }

    public Material[] SolidMaterials = new Material[2];
    public Material[] TransparentMaterials = new Material[2];
    public readonly int PlayerFaction = InitialMenuController.Faction; // 0:RES, 1:ENL

    public Material GetSolidMaterial(Faction faction)
    {
        return SolidMaterials[(int)faction ^ PlayerFaction];
    }

    public Material GetTransparentMaterial(Faction faction)
    {
        return TransparentMaterials[(int)faction ^ PlayerFaction];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
