using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction { Right = 0, Left = 1 } 

public class SpaceShip : MonoBehaviour
{
    Rigidbody2D rb;
    float speed, acc, SpeedLimit,BaseSpeed;
    public GameObject bullet;
    public List<GameObject> bulletPool;
    public GameObject Button, JoyStick;
    public List<Sprite> BigBlastSprites;
    Direction direction;
    Vector2 firstTouchPoint, LastTouchPoint, ButtonPoint;
    public bool isDead;
    public AudioClip BlastAudio;
    public AudioClip ShootAudio;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        BaseSpeed = 3f;
        speed = BaseSpeed;
        SpeedLimit = 6f;
        acc = ( SpeedLimit - BaseSpeed ) / 200;

        rb = transform.GetComponent<Rigidbody2D>();
        for (int i = 1; i < 2; i++)
        {
            bullet.SetActive(false);
            bulletPool.Add(Instantiate((GameObject)bullet));
        }
        //Button.SetActive(false);
        //JoyStick.SetActive(false);
        //Button = Instantiate(Button, new Vector2(0, 0), Quaternion.identity);
        //JoyStick = Instantiate(JoyStick, new Vector2(0, 0), Quaternion.identity);


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay && isDead == false)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                foreach (var cTouch in Input.touches)
                {
                    if (cTouch.position.x > Screen.width / 2)
                    {
                        //ButtonPoint = cTouch.position;
                        //Button.transform.position = cTouch.position;
                        //Button.SetActive(true);
                        Shoot();
                    }
                    switch (cTouch.phase)
                    {
                        //case TouchPhase.Began:

                        //    if (cTouch.position.x < Screen.width / 2)
                        //    {
                        //        firstTouchPoint = cTouch.position;
                        //    }

                        //    break;
                        //case TouchPhase.Moved:
                        //    if (cTouch.position.x < Screen.width / 2)
                        //    {
                        //        JoyStick.transform.position = cTouch.position;
                        //        JoyStick.SetActive(true);
                        //        LastTouchPoint = cTouch.position;
                        //        if (LastTouchPoint.x < firstTouchPoint.x)
                        //        {
                        //            if (direction == Direction.Right)
                        //            {
                        //                speed = BaseSpeed;
                        //            }
                        //            direction = Direction.Left;
                        //            speed = speed < SpeedLimit ? speed + acc : speed;

                        //            rb.MovePosition(new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y));
                        //            //Debug.Log("Sol");
                        //        }
                        //        else
                        //        {
                        //            if (direction == Direction.Left)
                        //            {
                        //                speed = BaseSpeed;
                        //            }
                        //            direction = Direction.Right;
                        //            speed = speed < SpeedLimit ? speed + acc : speed;
                        //            rb.MovePosition(new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y));
                        //            //Debug.Log("Sağ");
                        //        }
                        //    }
                        //    break;
                        //case TouchPhase.Ended:
                        //    speed = BaseSpeed;
                        //    if (cTouch.position.x < Screen.width / 2)
                        //    {
                        //        JoyStick.SetActive(false);
                        //    }
                        //    else
                        //    {
                        //        Button.SetActive(false);
                        //    }
                        //    break;
                        case TouchPhase.Stationary:
                            //if (cTouch.position.x < Screen.width / 2)
                            //{
                            //    if (LastTouchPoint.x < firstTouchPoint.x)
                            //    {
                            //        if (direction == Direction.Right)
                            //        {
                            //            speed = BaseSpeed;
                            //        }
                            //        direction = Direction.Left;
                            //        speed = speed < SpeedLimit ? speed + acc : speed;
                            //        rb.MovePosition(new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y));
                            //        //Debug.Log("Sol");
                            //    }
                            //    else
                            //    {
                            //        if (direction == Direction.Left)
                            //        {
                            //            speed = BaseSpeed;
                            //        }
                            //        direction = Direction.Right;
                            //        speed = speed < SpeedLimit ? speed + acc : speed;
                            //        rb.MovePosition(new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y));
                            //        //Debug.Log("Sağ");
                            //    }
                            //}
                            break;
                        default:
                            break;
                    }
                }


            }



            //Debug.Log("Speed :" + speed  + "  Acc : " + acc);


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.MovePosition(new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.MovePosition(new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y));
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Instantiate((GameObject)bulletPool[1], transform.FindChild("Namlu").transform.position, Quaternion.identity);
                Shoot();
            }
        }
        
    }
    public void Shoot()
    {
        GameObject CurrentBullet = GetPooledBullet();
        if (CurrentBullet != null)
        {
            CurrentBullet.transform.position = transform.Find("Namlu").transform.position;
            CurrentBullet.SetActive(true);
            this.transform.GetComponent<AudioSource>().Play();
            
        }
    }
    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
    IEnumerator BigBlast()
    {
        foreach (var sprite in BigBlastSprites)
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprite;
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.SetActive(false);
        isDead = true;
        GameData.CurrentLife--;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tag_EnemyBullet" && !isDead)
        {
            this.transform.GetComponent<AudioSource>().PlayOneShot(BlastAudio);
            StartCoroutine(BigBlast());
            
            
        }
    }

}
