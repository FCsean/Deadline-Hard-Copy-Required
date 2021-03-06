﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerScript : MonoBehaviour
{

    public Scroll road;
    public HighwayRandomizer randomizer;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject gameover;

    private List<GameObject> lives = new List<GameObject>();

	private AudioSource audioSource;

	public RuntimeAnimatorController jumpingAnimation;
	public RuntimeAnimatorController umbrellaAnimation;
	public RuntimeAnimatorController slidingAnimation;
	public RuntimeAnimatorController wimperAnimation;
	public RuntimeAnimatorController manholeAnimation;
	public RuntimeAnimatorController hurtAnimation;
	public RuntimeAnimatorController runningAnimation;

	public GameObject exitButton;
	public GameObject restartButton;

   	public enum Action {
        Running,
        Jumping,
        Sliding,
        Umbrella,
        Hurt,
        Lose,
		None,
    };

    private float endTime;
    private float invincibleTime;
    private Action currentAction;

	public AudioClip running;
	public AudioClip umbrella;
	public AudioClip hurt;
	public AudioClip falling;
	public AudioClip sliding;
	public AudioClip jumping;

	public ScoreScript scoreScript;

	public Action queuedAction = Action.None;

    public Action CurrentAction {
        get { return currentAction; }
    }


    int life;
    // Use this for initialization
    void Start()
    {
        life = 3;
        lives.Add(life1);
        lives.Add(life2);
        lives.Add(life3);

        SetAction(Action.Running);
        gameover.GetComponent<SpriteRenderer>().enabled = false;

		int mute = PlayerPrefs.GetInt ("mute", 0);
		GetComponent<AudioSource> ().mute = mute == 0 ? false : true;
	}

	void Awake()
	{
		audioSource = this.GetComponent<AudioSource> ();
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
                goto case Action.Sliding;
			case Action.Jumping:
				if (queuedAction != Action.None) {
					SetAction (queuedAction);
					queuedAction = Action.None;
					break;
				}
				goto case Action.Sliding;
            case Action.Sliding:
                SetAction(Action.Running);
                break;
        }
    }

    internal void UmbrellaAction(bool use)
    {
        if (currentAction == Action.Hurt)
            return;

		if (use && (currentAction == Action.Running || currentAction == Action.Sliding || currentAction == Action.Jumping))
        {
            SetAction(Action.Umbrella);
		}else if(currentAction == Action.Umbrella || queuedAction == Action.Umbrella)
        {
            SetAction(Action.Running);
        }
        
    }

	private void QueueAction(Action action) {
		if (action == Action.Running) {
			queuedAction = Action.None;
			return;
		}
		queuedAction = action;
	}

	private void SetAction(Action action, bool force = false)
    {
		if (!force) {
			if (currentAction == Action.Lose)
				return;
        
			if (currentAction == Action.Jumping && action != Action.Running && action != Action.Hurt) {
				QueueAction (action);
				return;
			}

			if (currentAction == Action.Hurt && action != Action.Running)
				return;
		}
        var anim = GetComponent<Animator>();
        anim.speed = 1f;

        switch (action) {
		case Action.Jumping:
				audioSource.Pause ();
				audioSource.clip = running;
				audioSource.PlayOneShot (jumping);
                anim.runtimeAnimatorController = jumpingAnimation;
                var rb = GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, 1000));
                endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                break;
		case Action.Sliding:
				audioSource.clip = sliding;
				audioSource.Play ();
	            if(currentAction == Action.Sliding)
                {
                    var state = anim.GetCurrentAnimatorStateInfo(0);
                    anim.Play(state.fullPathHash, 0, .3f);
                    endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length - .3f;
                } else
                {
					anim.runtimeAnimatorController = slidingAnimation;
                    endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
                }
                break;
		case Action.Umbrella:
				audioSource.PlayOneShot (umbrella);
				audioSource.clip = running;
				audioSource.Play ();
				anim.runtimeAnimatorController = umbrellaAnimation;
				anim.speed = 2.5f;
                break;
		case Action.Running:
				audioSource.clip = running;
				audioSource.Play ();
				anim.runtimeAnimatorController = runningAnimation;
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
		if (col.gameObject.name.ToLower().Contains("road") && currentAction != Action.Hurt && currentAction != Action.Running)
        {
			if (queuedAction != Action.None) {
				SetAction (queuedAction, true);
				queuedAction = Action.None;
				return;
			}
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
				Hurt(hurtAnimation);
            }
        } else if (col.gameObject.name.Contains("sliding") && currentAction != Action.Sliding)
        {
			Hurt(hurtAnimation);   
        } else if (col.gameObject.name.Contains("jumping"))
        {
			if (col.gameObject.name.Contains ("manhole")) 
			{
				life -= 3;
				transform.position = col.gameObject.transform.position;
				Hurt (manholeAnimation);
			} else 
			{
				Hurt (hurtAnimation);
			}
        } else if(col.gameObject.name.Contains("umbrella") && currentAction != Action.Umbrella)
        {
			Hurt(hurtAnimation);
        }
         
    }

	private void Hurt(RuntimeAnimatorController animator)
    {
		if (currentAction == Action.Hurt || currentAction == Action.Lose)
			return;
		
        var anim = GetComponent<Animator>();
		anim.speed = 1f;
		anim.runtimeAnimatorController = animator;
        endTime = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
        life--;
        GetComponent<SpriteRenderer>().sortingOrder = 99;

		for(int i = lives.Count - 1; i >= 0 && animator == manholeAnimation; i--)
        {
            Destroy(lives[i]);
        }
        if(lives.Count != 0 && life >= 0) { 
            Destroy(lives[life]);
        }

		audioSource.Pause ();
        if (life > 0)
        {
			audioSource.PlayOneShot (hurt);
            SetAction(Action.Hurt);
        } else
        {
			audioSource.PlayOneShot (falling);
            road.StopSpeed();
            randomizer.StopSpeed();
            currentAction = Action.Lose;
            gameover.transform.position = Vector3.zero;
            gameover.GetComponent<SpriteRenderer>().enabled = true;
			restartButton.GetComponent<SpriteRenderer> ().enabled = true;
			restartButton.GetComponent<BoxCollider2D> ().enabled = true;
			exitButton.GetComponent<SpriteRenderer> ().enabled = true;
			exitButton.GetComponent<BoxCollider2D> ().enabled = true;

			scoreScript.ShowScore ();
			if (animator != manholeAnimation)
				anim.runtimeAnimatorController = wimperAnimation;
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