using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Link : MonoBehaviour {
    public LinkModel Model { get; private set; }

    public Link Init(LinkModel model)
    {
        Model = model;
        return this;
    }

    // Use this for initialization
    void Start() {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new Vector3[] { Model.Source.Position, Model.Target.Position });
        lineRenderer.material = FactionManager.GetInstance().GetSolidMaterial(Model.Faction);
    }

    // Update is called once per frame
    void Update() {
		
	}
}
