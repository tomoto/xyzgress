using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public abstract class Link : MonoBehaviour {
    public LinkModel Model { get; private set; }

    public Link Init(LinkModel model)
    {
        Model = model;
        return this;
    }
}
