using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StateMachine : MonoBehaviour
{
    public abstract class StateMachine
    {
        public string stateName;
        public abstract void StateEnter();
        public abstract void StateUpdate();
        public abstract string CheckForTransition();
    }

    [Header("ステート"), SerializeField]
    Component[] State;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
