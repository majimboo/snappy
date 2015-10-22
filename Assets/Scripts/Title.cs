using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {
    public Text scoreTxt;

    void Start()
    {
        int curr_score = PlayerPrefs.GetInt("CurrentScore");
        int hi_score = PlayerPrefs.GetInt("HiScore");

        int score = hi_score;

        if (curr_score <= hi_score) score = curr_score;

        scoreTxt.text = score.ToString().PadLeft(6, '0');
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0)) {
            Application.LoadLevel("Game");
        }
    }

}
