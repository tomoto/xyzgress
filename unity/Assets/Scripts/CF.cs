using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Model;

public class CF : MonoBehaviour {
    public CFModel Model { get; private set; }

    private Mesh mesh;

    public CF Init(CFModel model)
    {
        Model = model;

        mesh = new Mesh();
        mesh.vertices = model.Portals.Select(p => p.Position.To3D()).ToArray();
        mesh.uv = new [] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1) };
        mesh.triangles = new[] { 0, 1, 2 };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.name = "CFMesh";

        return this;
    }

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material = FactionManager.GetInstance().GetTransparentMaterial(Model.Faction);
        GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
