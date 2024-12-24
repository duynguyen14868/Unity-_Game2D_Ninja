using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim; // animator: điền khiển anim, animation: clip
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] protected CombatText combatTextPrefab;

    private float hp;
    private string currentAnimName;

    public bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
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

    public void OnHit(float damage)
    {
        Debug.Log("Hit");
        if (!IsDead)
        {
            hp -= damage;

            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

}
