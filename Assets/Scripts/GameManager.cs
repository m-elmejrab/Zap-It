using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;


    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject zapPrefab;
    Canvas gameCanvas;
    bool gameIsRunning = false;
    bool enemyIsAlive = false;
    bool playerBall1IsAlive = false;
    bool playerBall2IsAlive = false;
    Vector3 ball1Position;
    Vector3 ball2Position;
    float hardship = 1f;
    float zapDuration = 0.4f;
    float zapLifeTime = 0f;
    int score = -1;


    // Use this for initialization
    void Awake () {


        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SetupScene();
	}
	
	// Update is called once per frame
	void Update () {

        if (playerBall1IsAlive&&playerBall2IsAlive)
            zapLifeTime = zapLifeTime + Time.deltaTime;

        if (zapLifeTime >= zapDuration)

        {
            DestroyZap();
            zapLifeTime = 0;
        }

        
        



    }

    void DestroyZap()
    {
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Player");
        if (ballObjects.Length > 0)
        {
            for (int i = 0; i < ballObjects.Length; i++)
            {
                GameObject.Destroy(ballObjects[i]);
            }
            playerBall1IsAlive = false;
            playerBall2IsAlive = false;
        }

        GameObject[] zapObjects = GameObject.FindGameObjectsWithTag("Zap");
        if (zapObjects.Length > 0)
        {
            for (int i = 0; i < zapObjects.Length; i++)
            {
                GameObject.Destroy(zapObjects[i]);
            }
            playerBall1IsAlive = false;
            playerBall2IsAlive = false;
        }
    }

    void SetupScene()
    {
        //Turn off UI
        gameIsRunning = true;

        GameObject obj = GameObject.FindGameObjectWithTag("Canvas");
        gameCanvas = obj.GetComponent<Canvas>();
        CreateEnemy();

    }

    public void CreateEnemy()
    {

        score += 1;
        GameObject obj = GameObject.FindGameObjectWithTag("Canvas");
        gameCanvas = obj.GetComponent<Canvas>();
        Text[] canvasTexts = gameCanvas.GetComponentsInChildren<Text>();
        if (canvasTexts.Length > 0)
        {
            foreach (Text t in canvasTexts)
            {
                if (t.name == "ScoreText")
                {
                    t.text = "Score: " + score;
                }
            }
        }

        GameObject enemy = Instantiate(enemyPrefab);
        Vector3 pos = new Vector3(0, 0, 0);
        enemy.transform.position = pos;
        enemy.transform.SetPositionAndRotation(pos, new Quaternion(0, 0, 0, 0));
        float x = Random.Range(30f, 40f);
        float signX = Random.Range(-1, 1);
        if (signX < 0)
            x = x * -1;
        float y = Random.Range(30f, 40f);
        float signY = Random.Range(-1, 1);
        if (signY < 0)
            y = y * -1;
        Rigidbody enemyRb;
        enemyRb = enemy.GetComponent<Rigidbody>();
        enemyRb.AddForce(x* hardship, y*hardship,0, ForceMode.Acceleration);
        enemyIsAlive = true;
        hardship = hardship + 0.25f;
    }

    public void CreatePlayer(Vector3 position)
    {
        position.z = 0;

        if (!playerBall1IsAlive)
        {

            GameObject ball1 = Instantiate(playerPrefab, position, new Quaternion (0,0,0,0));
            ball1Position = ball1.transform.position;
            playerBall1IsAlive = true;       
        }
        else if(!playerBall2IsAlive)
        {
            GameObject ball2 = Instantiate(playerPrefab, position, new Quaternion(0, 0, 0, 0));
            ball2Position = ball2.transform.position;

            CreateZap(ball1Position, ball2Position);
            playerBall2IsAlive = true;
        }
        else
        {
            DestroyZap();
          
        }
        

    }

    void CreateZap(Vector3 p1, Vector3 p2)
    {
        GameObject zap = Instantiate(zapPrefab, p1, new Quaternion(0, 0, 0, 0));
        
        zap.transform.LookAt(p2);
        zap.transform.Rotate(Vector3.left, 90);
        zap.transform.position = zap.transform.position - ((p1 - p2) / 2);
        Vector3 newScale = zap.transform.localScale;
        newScale.y = Vector2.Distance(p1,p2)/2;
        newScale.x = newScale.x / 2;
        newScale.z = newScale.z / 2;
        zap.transform.localScale = newScale ;

    }

    public void GameOver()
    {
        Image[] canvasChildren = gameCanvas.GetComponentsInChildren<Image>();
        
        foreach (Image g in canvasChildren)
        {
            if (g.name == "PausePanel")
            {
                Image i = g.GetComponent<Image>();
                i.color = Color.black;
            }
        }

        Invoke("ResetGame", 1f);

    }

    void ResetGame()
    {
        Image[] canvasChildren = gameCanvas.GetComponentsInChildren<Image>();

        foreach (Image g in canvasChildren)
        {
            if (g.name == "PausePanel")
            {
                Image i = g.GetComponent<Image>();
                i.color = Color.clear;
            }
        }

        hardship = 1f;
        zapLifeTime = 0f;
        score = -1;

        enemyIsAlive = false;
        playerBall1IsAlive = false;
        playerBall2IsAlive = false;

        SceneManager.LoadScene(0);

    }

}
