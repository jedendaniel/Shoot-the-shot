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
    Vector3 resultPosition;

    [SerializeField]
    Button startButton;
    Text startButtonText;
    
    Vector2 spawnRectStart = new Vector2(-5,2);
    Vector2 spawnRectEnd = new Vector2(10,6);
    float spin = -10;
    float forceMultiplier = 2.75f;
    int leftBound = -20;
    int rightBound = 20;
    int upBound = 20;
    
    int points;
    float timeLeft;

	void Start () {
        Time.timeScale = 0;
        resultPosition = resultText.transform.position;
        resultText.text = "";
        startButtonText = startButton.GetComponentInChildren<Text>();
        startButton.onClick.AddListener(StartGame);
    }
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(GetPointWithRaycast());
        }
        if (Input.GetMouseButtonDown(1))
        {
            SpawnBall();
        }
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = timeLeft.ToString();
        }
        else
        {
            timeText.text = 0.ToString();
            EndGame();
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
    }

    void SpawnBall()
    {
        ball.transform.position = new Vector3(
            Random.Range(spawnRectStart.x, spawnRectEnd.x), Random.Range(spawnRectStart.y, spawnRectEnd.y),0);
        ball.velocity = Vector2.zero;
        ball.useGravity = false;
    }

    void StartGame()
    {
        startButton.gameObject.SetActive(false);
        resultText.transform.position = resultPosition;
        resultText.text = "Result: " + points.ToString();
        SpawnBall();
        Time.timeScale = 1;
        timeLeft = 10;
        points = 0;
    }

    void EndGame()
    {
        Time.timeScale = 0;
        resultText.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        startButton.gameObject.SetActive(true);
    }
    
    bool IsOutOfBounds()
    {
        if (ball.position.x < leftBound
            || ball.position.y > rightBound
            || ball.position.z > upBound)
            return true;
        return false;
    }
}
