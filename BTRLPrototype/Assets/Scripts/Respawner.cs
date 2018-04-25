using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {
    public void StartRespawnTimer(GameObject deadObject, float timeToRespawn)
    {       
        StartCoroutine(RespawnTimer(deadObject, timeToRespawn));
    }

    public void StartRespawnTimer(Actor deadObject, float timeToRespawn)
    {
        //Debug.Log("respawning enemy");
        StartCoroutine(RespawnTimer(deadObject, timeToRespawn));
    }

    private IEnumerator RespawnTimer(GameObject respawningObj, float duration)
    {
        yield return new WaitForSeconds(duration);
        //Debug.Log("respawned after " + duration + " seconds");
    }

    private IEnumerator RespawnTimer(Actor respawningObj, float duration)
    {
        yield return new WaitForSeconds(duration);
        //Debug.Log("respawned enemy after " + duration + " seconds");
        respawningObj.Respawn();
    }
}
