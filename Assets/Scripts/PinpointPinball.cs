using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameMode
{                                                     
    idle,
    playing,
    levelEnd
}
public class PinpointPinball : MonoBehaviour
{
    static private PinpointPinball S;
    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 barrierPos;
    public GameObject[] barriers;
    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int lives;
    public GameObject barrier;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    void Start()
    {
        S = this;
        level = 0;
        levelMax = barriers.Length;
        StartLevel();
    }
    void StartLevel()
    {
        if (barrier != null)
        {
            Destroy(barrier);
        }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Pinball");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);;
        }
        barrier = Instantiate<GameObject>(barriers[level]);
        barrier.transform.position = barrierPos;
        lives = 3;
        PinballProjection.S.Clear();
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }
    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Lives: " + lives;
    }
    void Update()
    {
        UpdateGUI();
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            if (lives >= 0)
            {
                Invoke("NextLevel", 2f);
            }
			else
			{
                SceneManager.LoadScene("Scene_0");
            }
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public static void LivesLost()
    {
        S.lives--;
    }
    public static void LivesGained()
    {
        S.lives++;
    }
}
