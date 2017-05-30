using System;
using System.Collections.Generic;
using Game.Model;

internal class NPCAgentHelper
{
    public static void InitNPCAgentProfiles<T>(List<T> sortedNPCAgents, FieldModel field) where T: NPCAgentBase
    {
        // sortedNPCAgents should be sorted from left to right

        if (sortedNPCAgents.Count >= 3)
        {
            var w = field.Width;
            sortedNPCAgents[0].SetTerritory(0, w * 0.5f);
            sortedNPCAgents[sortedNPCAgents.Count - 1].SetTerritory(w * 0.5f, w);
            sortedNPCAgents[sortedNPCAgents.Count / 2].SetTerritory(w * 0.25f, w * 0.75f);
            return;
        }
    }
}