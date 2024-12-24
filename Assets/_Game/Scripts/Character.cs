using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim; // animator: điền khiển anim, animation: clip

    private float hp;

    public bool IsDead => hp <= 0;
    private string currentAnimName;

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        //throw new NotImplementedException();
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    // Di chuyển
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            //anim.ResetTrigger(animName);
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damsge)
    {
        if (!IsDead)
        {
            hp -= damsge;

            if(IsDead)
            {
                OnDeath();
            }
        }
    }

}
