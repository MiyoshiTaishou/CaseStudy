using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StateMachine : MonoBehaviour
{
    public abstract class State
    {
        public string StateName;
        public abstract void StateEnter();
        public abstract void StateUpdate();
        public abstract void StateExit();
        public abstract string CheckForTransition();
    }
    [Header("このオブジェクトに適用するState"), SerializeField]
    State[] StateDictionary;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ChangeState()
    {

    }
}
