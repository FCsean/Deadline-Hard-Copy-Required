using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour
{

    public Scroll road;
    public HighwayRandomizer randomizer;
    
    enum Action {
        Running,
        Jumping,
        Sliding,
        Umbrella,
        Hurt,
        Lose,
    };

    private float endTime;
    private float invincibleTime;
    private Action currentAction;

    int life;
    // Use this for initialization
    void Start()
    {
        life = 3;
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
                road.ResumeSpeed();
                randomizer.ResumeSpeed();
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                goto case Action.Jumping;
            case Action.Jumping:
            case Action.Sliding:
                    SetAction(Action.Running);
                break;
        }
    }

    internal void UmbrellaAction(bool use)
    {
        if (currentAction == Action.Hurt)
            return;

        if (use && (currentAction == Action.Running || currentAction == Action.Sliding))
        {
            SetAction(Action.Umbrella);
        }else if(currentAction == Action.Umbrella)
        {
            SetAction(Action.Running);
        }
        
    }

    private void SetAction(Action action)
    {
        if (currentAction == Action.Lose)
            return;
        
        if (currentAction == Action.Jumping && action != Action.Running && action != Action.Hurt)
            return;

        if (currentAction == Action.Hurt && action != Action.Running)
            return;

        var anim = GetComponent<Animator>();
        anim.speed = 1f;

        switch (action) {
            case Action.Jumping:
                anim.runtimeAnimatorController = Resources.Load("Player/jumping_0") as RuntimeAnimatorController;
                var rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, 1000));
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
				anim.runtimeAnimatorController = Resources.Load ("Player/umbrella_0") as RuntimeAnimatorController;
				anim.speed = 2.5f;
                break;
            case Action.Running:
                anim.runtimeAnimatorController = Resources.Load("Player/running_0") as RuntimeAnimatorController;
                anim.speed = 2.5f;
                break;
            case Action.Hurt:
                road.StopSpeed();
                randomizer.StopSpeed();
                invincibleTime = Time.time + 6f;
                StartCoroutine(Flash(0.10f));
                break;
        }
        currentAction = action;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.ToLower().Contains("road") && currentAction != Action.Hurt)
        {
            SetAction(Action.Running);  
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //obstacle collide
        //Debug.Log(col.gameObject.name);
        //Debug.Log(invincibleTime);
        if (invincibleTime > Time.time)
        {
            return;
        }
        
        if (col.gameObject.name.Contains("sliding") && col.gameObject.name.Contains("jumping"))
        {
            if(currentAction != Action.Sliding) {
                Hurt("hurt2_0");
            }
        } else if (col.gameObject.name.Contains("sliding") && currentAction != Action.Sliding)
        {
            Hurt("hurt2_0");   
        } else if (col.gameObject.name.Contains("jumping"))
        {
			if (col.gameObject.name.Contains ("manhole")) 
			{
				life -= 3;
				transform.position = col.gameObject.transform.position;
				Hurt ("fallManhole_0");
			} else 
			{
				Hurt ("hurt2_0");
			}
        } else if(col.gameObject.name.Contains("umbrella") && currentAction != Action.Umbrella)
        {
            Hurt("hurt2_0");
        }
         
    }

    private void Hurt(String animator)
    {
        var anim = GetComponent<Animator>();
		anim.speed = 1f;
        anim.runtimeAnimatorController = Resources.Load("Player/"+animator) as RuntimeAnimatorController;
        endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
        life--;
        GetComponent<SpriteRenderer>().sortingOrder = 99;
        if (life > 0)
        {
            SetAction(Action.Hurt);
        } else
        {
            road.StopSpeed();
            randomizer.StopSpeed();
            currentAction = Action.Lose;
			if(!animator.ToLower().Contains("manhole"))
				anim.runtimeAnimatorController = Resources.Load("Player/wimper_0") as RuntimeAnimatorController;
            // LOSE
        }
    }

    private float[] colors = { 0.1f, .5f, 1f};
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