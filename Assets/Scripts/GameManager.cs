using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatusTypes {inPlay, inPause, inLevelFinished}
public class GameManager : MonoBehaviour
{
    public GameObject Enemy, SpaceShip;
    Vector3 StartingPoint;
    public GameObject JoyStick, Button, StartingPointGO;
    public Sprite SpaceShipSprite;
    public GameObject EnemyStartingPoint;
    public Text LifeValue;
    public Text PlayePointValue;
    public int GamePoint;
    public GameObject GameOwer;
    Vector2 GameOwerPoint;
    public GameObject GameOwerTarget;
    public Button Replay;
    public Sprite youWin;
    float targetY = 0f, StartY = 0f, DeltaY = 0, StepY = 0f;
    int ind = 0;
    public GameStatusTypes GameStatus;
    public static GameManager instance = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        GameData.CurrentLife = 3;
        GameData.PlayerPoint = 0;
        GameStatus = GameStatusTypes.inPause;
        GameOwerPoint = GameOwer.transform.position;
        Replay.gameObject.SetActive(false);
        StartY = GameOwer.GetComponent<Transform>().position.y ;
        targetY = GameOwerTarget.transform.position.y;
        StartingPoint = EnemyStartingPoint.transform.position;
        setEnemies();
        GameStatus = GameStatusTypes.inPlay;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStatus == GameStatusTypes.inPlay)
        {
            StartCoroutine(moveEnemies());
            if (GameData.CurrentLife == 0)
            {
                StartCoroutine(YouCanFly(GameOwer,false));
                SpaceShip.SetActive(false);

            }
            if (!SpaceShip.activeInHierarchy && GameData.CurrentLife > 0)
            {
                StartCoroutine(ReleaseShip());
            }
            if (isWinner())
            {
                GameStatus = GameStatusTypes.inLevelFinished;
                StartCoroutine(YouCanFly(GameOwer, true));
            }
            LifeValue.text = GameData.CurrentLife.ToString();
            PlayePointValue.text = GameData.PlayerPoint.ToString();
        }
        
    }
    IEnumerator ReleaseShip()
    {
        int ind = 0;
        Color GhostShipColor = Color.clear;
        int Transp = 0;
        SpaceShip.transform.position = StartingPointGO.transform.position;
        SpaceShip.SetActive(true);
        SpaceShip.GetComponent<SpaceShip>().isDead = false;
        SpaceShip.GetComponent<SpriteRenderer>().sprite = SpaceShipSprite;
        while (ind < 50)
        {
            GhostShipColor = SpaceShip.GetComponent<Renderer>().material.color;
            GhostShipColor.a = Transp / 255f;
            SpaceShip.GetComponent<SpriteRenderer>().sortingOrder = 999;
            SpaceShip.GetComponent<Renderer>().material.color = GhostShipColor;
            ind++;
            Transp += 6;
            yield return new WaitForSeconds(0.01f);
        }
        GhostShipColor.a = 1f;
        SpaceShip.GetComponent<Renderer>().material.color = GhostShipColor;
    }

    void setEnemies()
    {
        float xStartingPoint;
        float yStartingPoint;

        xStartingPoint = EnemyStartingPoint.transform.position.x;
        yStartingPoint = EnemyStartingPoint.transform.position.y;
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                Enemy.GetComponent<SpriteRenderer>().sortingOrder = 99;
                Instantiate(Enemy, new Vector3(xStartingPoint + i, yStartingPoint - j), Quaternion.identity);
            }
        }
        
    }
    void WipeEnemies()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("tag_Enemy");
        GameObject[] EnemyBullets = GameObject.FindGameObjectsWithTag("tag_EnemyBullet");
        foreach (var EBullet in EnemyBullets)
        {
            Destroy(EBullet);
        }
        foreach (var enemy in Enemies)
        {
            Destroy(enemy);
        }
       
    }
    IEnumerator moveEnemies()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("tag_Enemy");
        while (true)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy != null)
                {
                    enemy.transform.position = Vector2.Lerp(enemy.transform.position, new Vector2(enemy.transform.position.x, enemy.transform.position.y - 0.1f), 0.001f);
                }
            }
            yield return new WaitForSeconds(20);
        }
    }
    IEnumerator YouCanFly(GameObject GhostGO, bool isWin)
    {
        DeltaY = targetY - StartY;
        StepY = DeltaY / 255;
        if (!isWin)
        {
            Replay.gameObject.SetActive(true);
        }
        else
        {
            GameOwer.GetComponent<SpriteRenderer>().sprite = youWin;
            GameOwer.GetComponent<Transform>().localScale = new Vector3(2, 2);
        }
        while (ind < 400)
        {
            GhostGO.GetComponent<SpriteRenderer>().sortingOrder = 99;
            GhostGO.GetComponent<Transform>().position = Vector3.Lerp(new Vector3(GhostGO.GetComponent<Transform>().position.x, GhostGO.GetComponent<Transform>().position.y, GhostGO.GetComponent<Transform>().position.z), new Vector3(GhostGO.GetComponent<Transform>().position.x, GhostGO.GetComponent<Transform>().position.y + 1f, GhostGO.GetComponent<Transform>().position.z), Time.deltaTime);
            ind++;
            yield return new WaitForSeconds(0.01f);
            //GameStatus = GameStatusTypes.inPause;
        }
        
        //Destroy(GhostGO);
    }
    public void PlayAgain()
    {
        GameOwer.transform.position = GameOwerPoint;
        Replay.gameObject.SetActive(false);
        WipeEnemies();
        setEnemies();
        GameData.PlayerPoint = 0;
        GameData.CurrentLife = 3;
        GameStatus = GameStatusTypes.inPlay;
    }
    bool isWinner()
    {
        bool result = true;
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("tag_Enemy");
        if (Enemies.Length == 0)
        {
            return true;
        }
        foreach (var enemy in Enemies)
        {
            if (enemy.activeInHierarchy)
            {
                result = false;
            }
        }
        return result;
    }
}
