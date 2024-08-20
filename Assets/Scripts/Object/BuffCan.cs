using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Speed, Boost, Jump
}

public class BuffCan : MonoBehaviour
{
    private Animator _animator;
    public BuffType Type;
    public float modify;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void AddBuff(PufferFishController controller)
    {
        switch (Type)
        {
            case BuffType.Speed:
                controller.MoveMultiplier += modify;
                break;
            case BuffType.Jump:
                //controller.JumpMultiplier += modify;
                controller.JumpSkill = true;
                break;
            case BuffType.Boost:
                controller.BoostSkill = true;
                
                controller.popScale += modify;
                if (controller.popScale < 1.1f) controller.popScale = 1.1f;
                break;
        }
        
        _animator.SetTrigger("Shrink");
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PufferFishController controller = other.gameObject.GetComponent<PufferFishController>();
            if (controller != null)
            {
                AddBuff(controller);
            }
        }
    }
}
