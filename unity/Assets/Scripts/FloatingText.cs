using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
    public string Text;
    public float Duration = 1.0f;
    public Vector2 MotionVector = Vector2.zero;

    public void Init(Vector2 position, string text, float duration)
    {
        transform.position = transform.position.Set2D(position);
        Text = text;
        Duration = duration;
    }

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = Text;
        Destroy(gameObject, Duration);
    }

    // Update is called once per frame
    void Update () {
        transform.position += (MotionVector * Time.deltaTime).To3D();
	}
}
