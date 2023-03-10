using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < Screen.width / 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

                circle.transform.position = pointA;
                outerCircle.transform.position = pointA;
                circle.GetComponent<SpriteRenderer>().enabled = true;
                outerCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    touchStart = true;
                    pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
                }
            } 
            
            else
            {
                touchStart = false;
            }
        }
       
    }
    private void FixedUpdate()
    {
        if (Input.mousePosition.x < Screen.width / 2)
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
    void moveCharacter(Vector2 direction)
    {
        Vector2 directionX;
        directionX = new Vector2(direction.x, 0);
        Player.transform.Translate(directionX * speed * Time.deltaTime);

    }
}
