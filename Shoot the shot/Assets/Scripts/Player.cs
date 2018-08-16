using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    
    [SerializeField]
    Rigidbody ball;
    
    [SerializeField]
    Text timeText;
    [SerializeField]
    Text resultText;

    [SerializeField]
    Button startButton;
    Text startButtonText;

    [SerializeField]
    Transform basketPosition;

    Vector2 spawnRectStart = new Vector2(-10,4);
    Vector2 spawnRectEnd = new Vector2(11,7);
    float spin = -10;
    float forceMultiplier = 2.75f;
    int leftBound = -20;
    int rightBound = 20;
    int upBound = 20;
    int downBound = 2;
    int fulTime = 60;
    List<CheckPoint> checkPoints = new List<CheckPoint>();
    float shootDistance;
    bool clockTicking;

    float points;
    float timeLeft;
    private Vector3 resultPosition;

    void Start () {
        Time.timeScale = 0;
        resultPosition = resultText.transform.position;
        resultText.text = "";
        startButtonText = startButton.GetComponentInChildren<Text>();
        startButton.onClick.AddListener(StartGame);
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (clockTicking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(GetPointWithRaycast());
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                SpawnBall();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                EndGame();
                StartGame();
            }
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timeText.text = timeLeft.ToString();
            }
            else
            {
                timeText.text = 0.ToString();
                clockTicking = false;
            }
        }
        else
        {
            if (ball.velocity.magnitude <= 0)
                EndGame();
        }

        if (checkPoints.Count == 2)
        {
            CountBasket();
            UpdateResult();
        }
        if (IsOutOfBounds()) SpawnBall();
    }

    Vector3 GetPointWithRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    void Shoot(Vector3 target)
    {
        ball.velocity = (target - ball.position) * forceMultiplier;
        ball.AddTorque(new Vector3(0, 0, spin));
        ball.useGravity = true;
        CalculateShootDistance();
    }

    void CalculateShootDistance()
    {
        shootDistance = (basketPosition.position - ball.position).magnitude;
    }

    void SpawnBall()
    {
        checkPoints.Clear();
        ball.transform.position = new Vector3(
            Random.Range(spawnRectStart.x, spawnRectEnd.x), Random.Range(spawnRectStart.y, spawnRectEnd.y),0);
        ball.velocity = Vector2.zero;
        ball.useGravity = false;
    }

    void StartGame()
    {
        clockTicking = true;
        startButton.gameObject.SetActive(false);
        resultText.transform.position = resultPosition;
        points = 0;
        resultText.text = "Result: " + points.ToString();
        SpawnBall();
        Time.timeScale = 1;
        timeLeft = fulTime;
        points = 0;
    }

    void EndGame()
    {
        clockTicking = false;
        Time.timeScale = 0;
        resultText.transform.position = new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
        startButton.gameObject.SetActive(true);
    }
    
    bool IsOutOfBounds()
    {
        if (ball.position.x < leftBound || 
            ball.position.y > upBound || 
            ball.position.y < downBound || 
            ball.position.z > rightBound)
            return true;
        return false;
    }

    public void AcceptCheckPoint(CheckPoint checkPoint)
    {
        if (!checkPoints.Contains(checkPoint))
            checkPoints.Add(checkPoint);
    }

    void CountBasket()
    {
        points += Mathf.Ceil(shootDistance);
        
    }

    void UpdateResult()
    {
        resultText.text = "Result: " + points.ToString();
    }
}
