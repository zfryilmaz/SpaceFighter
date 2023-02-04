using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //public float bulletSpeed;
    Rigidbody2D rb;
    float BULLET_SPEED = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay)
            rb.MovePosition(new Vector2(rb.transform.position.x, rb.transform.position.y - BULLET_SPEED * Time.deltaTime));
    }
    void Update()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay)
        {
            if (this.rb.transform.position.y < -5)
            {
                this.gameObject.SetActive(false);
            }
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tag_SpaceShip")
        {
            this.gameObject.SetActive(false);
            
        }
    }

}
