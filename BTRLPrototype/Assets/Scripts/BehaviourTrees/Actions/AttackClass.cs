using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT_and_RL.Behaviour_Tree;

public class AttackClass : BTAction {
    [SerializeField]
    protected Enemy target;
    [SerializeField]
    protected GameObject caster;
    [SerializeField]
    protected float baseDamage;
    [SerializeField]
    protected float damageDealt = 0f;  //stores how much damage was done the last time this attack was used
    [SerializeField]
    protected string damageType;
    [SerializeField]
    protected float cooldownMax;
    [SerializeField]
    protected float cooldownTimer = 0f;

    public AttackClass()
    {
        taskName = "Attack Class";
    }

    public void SetTarget(Enemy _target)
    {
        target = _target;
    }

    public string GetElementType()
    {
        return damageType;
    }

    public virtual float LaunchAttack(Blackboard blackboard)
    {
        AIPerceptor senses = (AIPerceptor)blackboard.GetValue("senses");
        //first time setup for this attack's caster
        if (caster == null)
        {
            caster = (GameObject)blackboard.GetValue("owner");
        }
        //calculate damage based on base damage and the target's resistances (in some games this would also involve the caster's stats)
        target = senses.GetCurrentTarget();
        if(target == null)
        {
            Debug.Log("No target to attack");
            return 0;
        }
        float damage = baseDamage * target.GetResistance(damageType);
        //deal the damage to the enemy
        target.TakeDamage(damage);

        //the damage value returned is used as the reward value
        return damage;
    }

    public float GetCooldown()
    {
        return cooldownTimer;
    }

    public float GetDamageDealt()
    {
        return damageDealt;
    }
}
