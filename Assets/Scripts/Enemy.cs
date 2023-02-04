using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite bigFoot, smallFoot,Blast1,Blast2, Blast3, Blast4, Blast5;
    public List<Sprite> Blasts;
    bool isBigFoot,isBlasted;
    public GameObject bullet;
    public List<GameObject> bulletPool;
    bool ScanStarted = false;
    public int EnemyPoint = 50;
    bool isDead;
    Transform EnemyBarrel,EnemyRadar;
    Rigidbody2D rb;
    int prob;
    int difficulty = 25;//(0 - 100)
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        EnemyBarrel = transform.Find("Barrel");
        EnemyRadar = transform.Find("Radar");
        for (int i = 1; i < 2; i++)
        {
            bullet.SetActive(false);
            bulletPool.Add(Instantiate((GameObject)bullet));
        }
        StartCoroutine(changeSprite());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameStatus == GameStatusTypes.inPlay && !isDead)
        {
            if (isFirstRow() && !ScanStarted)
            {
                StartCoroutine(scanForShips());
            }
        }
    }
    void Shoot()
    {
        GameObject CurrentBullet = GetPooledBullet();
        if (CurrentBullet != null && !isDead)
        {
            CurrentBullet.transform.position = EnemyBarrel.position;
            CurrentBullet.SetActive(true);
            //this.transform.GetComponent<AudioSource>().Play();
        }
    }
    IEnumerator changeSprite()
    {
        while (!isBlasted)
        {
            if (isBigFoot)
            {
                transform.GetComponent<SpriteRenderer>().sprite = smallFoot;
                isBigFoot = false;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sprite = bigFoot;
                isBigFoot = true;
            }
            yield return new WaitForSeconds(1f);
        }
        if (isBlasted)
        {
            
            for (int i = 0; i < Blasts.Count; i++)
            {
                transform.GetComponent<SpriteRenderer>().sprite = Blasts[i];
                yield return new WaitForSeconds(0.2f);
            }
            this.gameObject.SetActive(false);
            GameData.PlayerPoint += EnemyPoint;
            Destroy(this.gameObject);
        }
        
    }
    IEnumerator scanForShips()
    {
        float fromAngle, ToAngle,CurrentAngle;
        fromAngle = 160;
        ToAngle = 200;
        CurrentAngle = 180;
        float yon = 1f;
        ScanStarted = true;
        while (true)
        {
            
            if (CurrentAngle == ToAngle)
            {
                yon = -1f;
            }else if (CurrentAngle == fromAngle) {
                yon = 1f;
            }
            CurrentAngle += yon;
            //CurrentAngle = CurrentAngle <= 120 ? CurrentAngle++ : CurrentAngle;
            //CurrentAngle = CurrentAngle >= 270 ? CurrentAngle-- : CurrentAngle;
            EnemyRadar.Rotate(0, 0, yon);
            RaycastHit2D hit = Physics2D.Raycast(EnemyRadar.position, EnemyRadar.up);
            Debug.DrawRay(EnemyRadar.position, EnemyRadar.up, Color.red);
            if (hit.collider != null && getProb())
            {
                Shoot();
            }
            yield return new WaitForSeconds(0.07f);
        }
    }
    bool getProb()
    {
        prob = Random.Range(1 ,100);
        if (prob < this.difficulty)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tag_bullet" && !isDead)
        {
            isBlasted = true;
            isDead = true;
            this.transform.GetComponent<AudioSource>().Play();
            transform.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(changeSprite());
        }
       
    }
    bool isFirstRow()
    {
        RaycastHit2D hit = Physics2D.Raycast(EnemyRadar.position, -Vector2.up);
        Debug.DrawRay(EnemyRadar.position, -Vector2.up, Color.red);
        if (hit.collider != null && hit.transform.name != "SpaceShip")
        {
            //Debug.Log(hit.transform.name);
            return false;
        }
        return true;
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

    

}
