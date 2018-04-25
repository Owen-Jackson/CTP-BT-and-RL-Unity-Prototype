using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : Enemy {

	// Use this for initialization
	new void Start () {
        base.Start();
        resistances["Water"] = 2f;
        resistances["Earth"] = 1f;
        resistances["Air"] = 1f;
        resistances["Fire"] = -1f;
        maxHealth = 10000;
        health = 10000;
        actorName = "Fire Elemental";
    }
}
