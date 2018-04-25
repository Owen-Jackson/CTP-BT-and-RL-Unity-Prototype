using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour {

    public float healthPerRegenTick = 5;
    public float regenRate = 0.5f;
    public float regenTimer = 0f;
    List<Actor> actors;

    private void Awake()
    {
        actors = new List<Actor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Actor>())
        {
            if (!actors.Contains(other.GetComponent<Actor>()))
            {
                actors.Add(other.GetComponent<Actor>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Actor>())
        {
            if (actors.Contains(other.GetComponent<Actor>()))
            {
                actors.Remove(other.GetComponent<Actor>());
            }
        }
    }

    private void Update()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenRate)
        {
            if (actors.Count > 0)
            {
                foreach (Actor actor in actors)
                {
                    actor.Heal(healthPerRegenTick);
                }
            }
            regenTimer = 0f;
        }
    }
}
