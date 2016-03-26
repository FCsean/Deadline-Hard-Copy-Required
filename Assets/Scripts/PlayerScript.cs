using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour
{

    enum Action {
        Running,
        Jumping,
        Sliding,
        Umbrella,
    };

    private float endTime;
    private Action currentAction;

    // Use this for initialization
    void Start()
    {

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
        switch(currentAction)
        {
            case Action.Jumping:
            case Action.Sliding:
                if (Time.time > endTime)
                {
                    SetAction(Action.Running);
                }
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
        currentAction = action;
        var anim = GetComponent<Animator>();
        
        switch (action) {
            case Action.Jumping:
                anim.runtimeAnimatorController = Resources.Load("Player/jumping_0") as RuntimeAnimatorController;
                var rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, 600));
                endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                break;
            case Action.Sliding:
                anim.runtimeAnimatorController = Resources.Load("Player/sliding_0") as RuntimeAnimatorController;
                endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                break;
            case Action.Umbrella:
                anim.runtimeAnimatorController = Resources.Load("Player/umbrella_0") as RuntimeAnimatorController;
                break;
            case Action.Running:
                anim.runtimeAnimatorController = Resources.Load("Player/running_0") as RuntimeAnimatorController;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "road")
        {
            SetAction(Action.Running);  
        }
    }
}