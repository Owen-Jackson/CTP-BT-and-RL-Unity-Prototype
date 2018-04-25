using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCrab : Enemy {

    // Use this for initialization
    new void Start () {
        base.Start();
        resistances["Water"] = 0.25f;
        resistances["Earth"] = 1f;
        resistances["Air"] = 1.5f;
        resistances["Fire"] = 0.75f;
        maxHealth = 2000;
        health = 2000;
        actorName = "Mud Crab";
    }
}
