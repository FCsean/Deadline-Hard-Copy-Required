using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour
{

    public Scroll road;

    private float lastSpeed;
    
    enum Action {
        Running,
        Jumping,
        Sliding,
        Umbrella,
        Hurt,
    };

    private float endTime;
    private float invincibleTime;
    private Action currentAction;
    
    // Use this for initialization
    void Start()
    {
        SetAction(Action.Running);
    }

    // Update is called once per frame
    void Update()
    {
        CheckActionFinished();
    }

    public void DownGesture()
    {
        SetAction(Action.Sliding);
    }

    public void UpGesture()
    {
        SetAction(Action.Jumping);
    }

    private void CheckActionFinished()
    {
        if (Time.time < endTime)
            return;
            switch (currentAction)
        {
            case Action.Hurt:
                road.speed = lastSpeed;
                goto case Action.Jumping;
            case Action.Jumping:
            case Action.Sliding:
                    SetAction(Action.Running);
                break;
        }
    }

    internal void UmbrellaAction(bool use)
    {
        SetAction(use ? Action.Umbrella : Action.Running);
        }

    private void SetAction(Action action)
    {
        if (currentAction == Action.Jumping && action != Action.Running)
            return;

        if (currentAction == Action.Hurt && action != Action.Running)
            return;

        var anim = GetComponent<Animator>();
        anim.speed = 1f;

        switch (action) {
            case Action.Jumping:
                anim.runtimeAnimatorController = Resources.Load("Player/jumping_0") as RuntimeAnimatorController;
                var rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, 600));
                endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                break;
            case Action.Sliding:
                if(currentAction == Action.Sliding)
                {
                    var state = anim.GetCurrentAnimatorStateInfo(0);
                    anim.Play(state.fullPathHash, 0, .3f);
                    endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length - .3f;
                } else
                {
                    anim.runtimeAnimatorController = Resources.Load("Player/sliding_0") as RuntimeAnimatorController;
                    endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                }
                break;
            case Action.Umbrella:
                anim.runtimeAnimatorController = Resources.Load("Player/umbrella_0") as RuntimeAnimatorController;
                break;
            case Action.Running:
                anim.runtimeAnimatorController = Resources.Load("Player/running_0") as RuntimeAnimatorController;
                anim.speed = 2.5f;
                break;
            case Action.Hurt:
                lastSpeed = road.speed;
                road.speed = 0;
                invincibleTime = Time.time + 6f;
                StartCoroutine(Flash(0.10f));
                break;
        }
        currentAction = action;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Road")
        {
            SetAction(Action.Running);  
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //obstacle collide
        //Debug.Log(col.gameObject.name);
        //Debug.Log(invincibleTime);
        if (invincibleTime > Time.time)
        {
            return;
        }
        var anim = GetComponent<Animator>();
        switch (currentAction)
        {
            case Action.Running:
                if (col.gameObject.name.Contains("sliding"))
                {
                    anim.runtimeAnimatorController = Resources.Load("Player/hurt2_0") as RuntimeAnimatorController;
                    endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                    SetAction(Action.Hurt);
                }
                break;
            default:
                break;
        }
    }

    private int[] colors = { 0, 255/2, 255};
    IEnumerator Flash(float intervalTime)
    {
        int index = 0;
        while (invincibleTime > Time.time)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, colors[index %3]);
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 255);
    }
    
}