using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BT_and_RL.Behaviour_Tree;

public class ActionPoolTest : MonoBehaviour {
    [SerializeField]
    public Dictionary<string, System.Type> actionPool;
    [SerializeField]
    public List<BTAction> actions;

    private void Awake()
    {
        actionPool = new Dictionary<string, System.Type>();
        System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            System.Type[] actions = assemblies[i].GetTypes().Where(t => t.IsSubclassOf(typeof(BTAction))).ToArray();
            for (int j = 0; j < actions.Length; j++)
            {
                if (!actionPool.ContainsKey(actions[j].Name))
                {
                    //Debug.Log("adding a new action");
                    //Debug.Log("adding: " + actions[j].Name);
                    //var obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(actions[j].GetType());
                    actionPool.Add(actions[j].Name, actions[j]);
                    //actionPool.Add(actions[j].GetType(), System.Activator.CreateInstance(actions[j].GetType()));
                }
            }
        }                
    }
}
