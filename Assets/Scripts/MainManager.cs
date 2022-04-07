using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int bestScore = 0;
    private string bestScoreName = "Nobody";
    [SerializeField] Text bestScoreText;

    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {

        LoadHighScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        
        if (bestScore == 0)
        {
            bestScoreText.text = "Best Score: " + GameManager.instance.nameText + ": " + bestScore;
            Debug.Log(GameManager.instance.nameText);
        }
        else
        {
            bestScoreText.text = "Best Score: " + bestScoreName + ": " + bestScore;
        }


        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points >= bestScore)
        {
            bestScore = m_Points;
            bestScoreName = GameManager.instance.nameText;
            bestScoreText.text = "Best Score: " + bestScoreName + ": " + bestScore;
            SaveHighScore();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }






    [Serializable]
    class SaveData
    {
     
        public string SavedHighScoreName;
        public int SavedHighScore;

    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.SavedHighScoreName = bestScoreName;
        data.SavedHighScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log(data);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScoreName = data.SavedHighScoreName;
            bestScore = data.SavedHighScore;
        }
    }

}
