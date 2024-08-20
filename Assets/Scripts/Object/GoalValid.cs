using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalValid : MonoBehaviour
{
    private PufferFishController pufferTarget;

    public float speedRequire = 20f;
    public float boostRequire = 20f;
    
    private void OnTriggerEnter(Collider other)
    {
        if(pufferTarget == null) 
            pufferTarget = other.GetComponent<PufferFishController>();
        
    }

    void CheckCondition()
    {
        if(pufferTarget.currentSpeed < speedRequire) return;
        if(!pufferTarget.BoostSkill || pufferTarget.boostForce < boostRequire) return;
        if(!pufferTarget.JumpSkill) return;

        Goal();
    }

    void Goal()
    {
        //Show Title "Thanks for playing the demo"
    }
}
