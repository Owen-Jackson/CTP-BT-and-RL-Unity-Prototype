using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this tells the player if they are under attack, aka too close to an enemy
public class PlayerPerceptor : MonoBehaviour {
    [SerializeField]
    PlayerController player;

    [SerializeField]
    Actor NPC;

    [SerializeField]
    List<GameObject> nearbyEnemies;

    [SerializeField]
    List<GameObject> attackingEnemies;

    [SerializeField]
    private float maxUnderAttackDistance = 5.0f;

    private void Awake()
    {
        nearbyEnemies = new List<GameObject>();
        attackingEnemies = new List<GameObject>();
        player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        //check if the player is 'under attack', this basic version triggers when
        //the player is too close to an enemy, similar to aggro ranges in RPGs
        bool isEnemyTooClose = false;
        for(int i = nearbyEnemies.Count - 1; i >= 0; i--)
        {
            if(Vector3.Distance(player.gameObject.transform.position, nearbyEnemies[i].transform.position) <= maxUnderAttackDistance)
            {
                isEnemyTooClose = true;
                if(!attackingEnemies.Contains(nearbyEnemies[i]))
                {
                    attackingEnemies.Add(nearbyEnemies[i]);
                }                
            }
            else if(attackingEnemies.Contains(nearbyEnemies[i]))
            {
                attackingEnemies.Remove(nearbyEnemies[i]);
            }
        }
        //let the ally know if i am under attack by saying how many enemies are attacking me
        ((DynamicTreeTest)NPC).GetBlackboard().SetValue("attackingEnemies", attackingEnemies);
        if(isEnemyTooClose && !player.IsUnderAttack)
        {
            player.IsUnderAttack = true;
        }
        else if(!isEnemyTooClose && player.IsUnderAttack)
        {
            player.IsUnderAttack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(!nearbyEnemies.Contains(other.gameObject))
            {
                nearbyEnemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (nearbyEnemies.Contains(other.gameObject))
            {
                nearbyEnemies.Remove(other.gameObject);
            }
        }
    }

}
