using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

//the sensors for the friendly AI 
public class AIPerceptor : MonoBehaviour
{
    [SerializeField]
    private Blackboard blackboard; //this will become a reference to the agent's blackboard, it will be updated with the targets

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float distanceFromPlayer;

    [SerializeField]
    private List<GameObject> targets;

    [SerializeField]
    private Enemy currentTarget;    //the enemy this AI is currently targetting for attacks

    [SerializeField]
    private List<GameObject> interactables; //no functionality with this program but could be used for e.g. buttons, doors or chests
    
    // Use this for initialization
    void Awake()
    {
        targets = new List<GameObject>();
        interactables = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        }
        if(currentTarget == null)
        {
            if(targets.Count > 0)
            {
                SelectEnemy();
            }
        }
    }

    public void SetBlackboard(Blackboard newBlackboard)
    {
        blackboard = newBlackboard;
    }

    public float GetDistanceFromPlayer()
    {
        return distanceFromPlayer;
    }

    //sense new enemies and add them to the list
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(!targets.Contains(other.gameObject))
            {
                targets.Add(other.gameObject);
                if(blackboard != null)
                {
                    blackboard.SetValue("enemies", targets);
                }
            }
        }
        if(other.CompareTag("Interactable"))
        {
            if(!interactables.Contains(other.gameObject))
            {
                interactables.Add(other.gameObject);
            }
        }
    }

    //remove enemies from the list when they move out of range
    private void OnTriggerExit(Collider other)
    {
        if(targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
            if(blackboard != null)
            {
                blackboard.SetValue("enemies", targets);
            }
        }
        if(interactables.Contains(other.gameObject))
        {
            interactables.Remove(other.gameObject);
        }
        if (currentTarget)
        {
            if (other.gameObject == currentTarget.gameObject)
            {
                currentTarget = null;
            }
        }
    }

    public void RemoveTargetFromList()
    {
        currentTarget.gameObject.SetActive(false);
        targets.Remove(currentTarget.gameObject);
        currentTarget = null;
    }

    public bool SelectEnemy()
    {
        if (targets.Count > 0)
        {
            currentTarget = targets.FirstOrDefault(x => x.tag == "Enemy").GetComponent<Enemy>();
            if (currentTarget != null)
            {
                return true;
            }
        }
        return false;
    }

    public Enemy GetCurrentTarget()
    {
        if(currentTarget == null)
        {
            SelectEnemy();
        }
        return currentTarget;
    }

    public void SetCurrentTarget(Enemy newTarget)
    {
        currentTarget = newTarget;
    }
}
