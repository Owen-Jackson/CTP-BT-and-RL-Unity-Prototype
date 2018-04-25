using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this enum is used to indicate which situations the task is compatible to
//each action will relate to only one of these and each RLSelector can use multiple
public enum TaskCompatibility
{
    NONE = 0,
    IDLE,
    ATTACK,
    MOVEMENT
}