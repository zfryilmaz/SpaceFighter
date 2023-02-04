using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick_Touch : MonoBehaviour
{
    public GameObject Player;
    public float speed = 5f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform circle;
    public Transform outerCircle;
    // Start is called before the first frame update
    void Start()
    {
        circle.GetComponent<SpriteRenderer>().enabled = false;
        outerCircle.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                foreach (var cTouch in Input.touches)
                {
                    if (cTouch.position.x < Screen.width / 2)
                    {
                        switch (cTouch.phase)
                        {
                            case TouchPhase.Began:
                                pointA = Camera.main.ScreenToWorldPoint(new Vector3(cTouch.position.x, cTouch.position.y, Camera.main.transform.position.z));
                                circle.transform.position = pointA;
                                outerCircle.transform.position = pointA;
                                circle.GetComponent<SpriteRenderer>().enabled = true;
                                outerCircle.GetComponent<SpriteRenderer>().enabled = true;
                                touchStart = true;
                                break;
                            case TouchPhase.Moved:
                                pointB = Camera.main.ScreenToWorldPoint(new Vector3(cTouch.position.x, cTouch.position.y, Camera.main.transform.position.z));
                                break;
                            case TouchPhase.Stationary:
                                pointB = Camera.main.ScreenToWorldPoint(new Vector3(cTouch.position.x, cTouch.position.y, Camera.main.transform.position.z));

                                //touchStart = true;
                                break;
                            case TouchPhase.Ended:
                                circle.GetComponent<SpriteRenderer>().enabled = false;
                                outerCircle.GetComponent<SpriteRenderer>().enabled = false;
                                touchStart = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }   
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                foreach (var cTouch in Input.touches)
                {
                    if (cTouch.position.x < Screen.width / 2)
                    {
                        if (touchStart)
                        {
                            Vector2 offset = pointB - pointA;
                            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
                            moveCharacter(direction);

                            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y);
                        }
                        else
                        {
                            circle.GetComponent<SpriteRenderer>().enabled = false;
                            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                }
            }
        }
        

    }
    void moveCharacter(Vector2 direction)
    {
        Vector2 directionX;
        directionX = new Vector2(direction.x, 0);
        Player.transform.Translate(directionX * speed * Time.deltaTime);

    }
}
