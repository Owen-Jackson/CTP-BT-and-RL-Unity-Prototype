using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
    protected Alignment actorAlignment;
    public Alignment ActorAlignment
    {
        get { return actorAlignment; }
        set { actorAlignment = value; }
    }

    [SerializeField]
    protected Dictionary<string, float> resistances;
    [SerializeField]
    protected string actorName;    //Used to identify the enemy type
    [SerializeField]
    protected float maxHealth = 100;
    [SerializeField]
    protected float health = 100;
    [SerializeField]
    protected bool isAlive;
    [SerializeField]
    protected float timeToRespawn;
    [SerializeField]
    protected float respawnTimer;
    [SerializeField]
    protected float moveSpeed = 10f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public virtual void Start()
    {
        ActorAlignment = Alignment.NEUTRAL;
    }

    private void OnEnable()
    {        
        isAlive = true;
    }

    public void Die()
    {
        isAlive = false;
        Environment.Instance.GameRespawner.StartRespawnTimer(this, timeToRespawn);
        //this.gameObject.SetActive(false);
    }

    public float TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Debug.Log(name + " defeated");
            Die();
        }
        return health;
    }

    public void Heal(float healthGained)
    {
        if (health + healthGained > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healthGained;
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetHealthAsPercentage()
    {
        return health / maxHealth;
    }

    public string GetName()
    {
        return actorName;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void Respawn()
    {
        health = maxHealth;
        isAlive = true;
        gameObject.SetActive(true);
    }
}
