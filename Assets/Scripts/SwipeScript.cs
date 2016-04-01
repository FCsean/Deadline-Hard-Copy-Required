using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class SwipeScript : MonoBehaviour
{
	public Scroll road;
	public HighwayRandomizer randomizer;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    int lastCount = 0;

    private bool isSwipe = false;
    private float minSwipeDist = 1.0f;
    private float maxSwipeTime = 0.5f;

    public PlayerScript player;

    private bool hold = false;
	private bool pause = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player.UpGesture();
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.DownGesture();
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            player.UmbrellaAction(true);
		} else if (Input.GetKeyDown(KeyCode.P)) {
			if (pause) {
				Time.timeScale = 1;
				pause = false;
				road.ResumeSpeed ();
				randomizer.ResumeSpeed ();
			} else {
				pause = true;
				Time.timeScale = 0;
				road.StopSpeed ();
				randomizer.StopSpeed ();
			}
		}

        if(Input.GetKeyUp(KeyCode.Space))
        {
            player.UmbrellaAction(false);
            if (player.CurrentAction == PlayerScript.Action.Lose)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        if (Input.touchCount > 0)
        {
            if (Input.touchCount >= 2)
            {
                if (!hold) { 
                    player.UmbrellaAction(true);
                    hold = true;
                }
            } else
            {
                if (lastCount >= 2 && hold)
                {
                    hold = false;
                    player.UmbrellaAction(false);
                }
            }
            lastCount = Input.touchCount;
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Stationary:
                        float gestureTime = Time.time - fingerStartTime;
                        if (gestureTime > maxSwipeTime && !hold) { 
                            
                        }
                        break;
                    case TouchPhase.Began:
                        /* this is a new touch */
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
						if (player.CurrentAction == PlayerScript.Action.Lose)
						{
							SceneManager.LoadScene("GameScene");
						}
                        break;

                    case TouchPhase.Canceled:
                        /* The touch is being canceled */
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:
                        gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    // MOVE RIGHT
                                }
                                else {
                                    // MOVE LEFT
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    // MOVE UP
                                    player.UpGesture();
                                }
                                else
                                {
                                    // MOVE DOWN
                                    player.DownGesture();
                                }
                            }

                        }

                        break;
                }
            }
        }

    }

    
}