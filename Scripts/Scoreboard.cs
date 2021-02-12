using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    int Score;
    Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<Text>();
        ScoreText.text = Score.ToString();
    }

    public void ScoreHit(int Addedscore)
    {
        Score = Score + Addedscore;
        ScoreText.text = Score.ToString();
    }
}