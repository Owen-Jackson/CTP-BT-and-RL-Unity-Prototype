using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {
    public float GetResistance(string rType)
    {
        if (resistances.ContainsKey(rType))
        {
            return resistances[rType];
        }
        else
        {
            return 1;
        }
    }

	// Use this for initialization
	public override void Start () {
        resistances = new Dictionary<string, float>
        {
            //initialise elemental resistances, done as a multiplier: 0 = 100% resistance, 1 = no resistance, anything over 1 = weakness
            { "Water", 1f },
            { "Earth", 1f },
            { "Air", 1f },
            { "Fire", 1f }
        };
        isAlive = true;
        respawnTimer = 0;
        timeToRespawn = 1f;
        ActorAlignment = Alignment.ENEMY;
    }
}
