using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Model;

public class EnemyAgent : NPCAgentBase
{
    protected override Faction SelfFaction { get { return Faction.Enemy; } }

    public new EnemyAgent Init(GameProvider gameProvider)
    {
        base.Init(gameProvider);
        return this;
    }
}
