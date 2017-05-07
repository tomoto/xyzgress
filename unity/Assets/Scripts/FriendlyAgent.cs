using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Model;

public class FriendlyAgent : NPCAgentBase
{
    protected override Faction SelfFaction { get { return Faction.Player; } }

    public new FriendlyAgent Init(GameProvider gameProvider)
    {
        base.Init(gameProvider);
        return this;
    }
}
