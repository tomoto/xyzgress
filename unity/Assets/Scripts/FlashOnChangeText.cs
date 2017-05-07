using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashOnChangeText : MonoBehaviour {
    public Color NormalColor;
    public Color FlashColor;
    public float FlashDuration;

    private Text TextComponent { get { return GetComponent<Text>(); } }
    private TimeTicker flashTimer;

    public string Text
    {
        get {
            return TextComponent.text;
        }
        set {
            if (!TextComponent.text.Equals(value))
            {
                TextComponent.text = value;
                if (FlashDuration > 0)
                {
                    TextComponent.color = FlashColor;
                    flashTimer = new TimeTicker(FlashDuration).Start();
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        TextComponent.color = NormalColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (flashTimer != null && flashTimer.IsTimeout())
        {
            TextComponent.color = NormalColor;
            flashTimer = null;
        }
	}

}
