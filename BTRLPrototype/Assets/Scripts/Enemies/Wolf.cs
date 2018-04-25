using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy {

	// Use this for initialization
	new void Start () {
        base.Start();
        resistances["Water"] = 1f;
        resistances["Earth"] = 0.5f;
        resistances["Air"] = 1f;
        resistances["Fire"] = 2f;
        maxHealth = 5000;
        health = 5000;
        actorName = "Wolf";
    }
}
