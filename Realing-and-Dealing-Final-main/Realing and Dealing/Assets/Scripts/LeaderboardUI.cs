using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class LeaderboardUI : MonoBehaviour
{
    // End screen called from GameManager, This is just leaderboard
    [SerializeField]
    GameObject leaderboard;

    //Score
    [SerializeField]
    TMPro.TMP_Text ScoreText;
    public int score;

    //LEADERBOARD
    [SerializeField] private TMP_Text top1NameText;
    [SerializeField] private TMP_Text top1ScoreText;
    [SerializeField] private TMP_Text top2NameText;
    [SerializeField] private TMP_Text top2ScoreText;
    [SerializeField] private TMP_Text top3NameText;
    [SerializeField] private TMP_Text top3ScoreText;
    [SerializeField] private TMP_Text top4NameText;
    [SerializeField] private TMP_Text top4ScoreText;
    [SerializeField] private TMP_Text top5NameText;
    [SerializeField] private TMP_Text top5ScoreText;

    void Start()
    {
        GameManager gamemanager = GetComponent<GameManager>();
    }

    public void DisplayLeaderboard()
    {
        leaderboard.SetActive(true);
    }

    public void HideLeaderboard()
    {
        leaderboard.SetActive(false);
    }

    public void UpdateTop5(LeaderboardEntry[] list)
    {
        if (list.Length > 0)
        {
            top1NameText.text = list[0].username;
            top1ScoreText.text = list[0].score.ToString();
        }
        else
        {
            top1NameText.text = "---";
            top1ScoreText.text = "---";
        }

        if (list.Length > 1)
        {
            top2NameText.text = list[1].username;
            top2ScoreText.text = list[1].score.ToString();
        }
        else
        {
            top2NameText.text = "---";
            top2ScoreText.text = "---";
        }

        if (list.Length > 2)
        {
            top3NameText.text = list[2].username;
            top3ScoreText.text = list[2].score.ToString();
        }
        else
        {
            top3NameText.text = "---";
            top3ScoreText.text = "---";
        }
        if (list.Length > 3)
        {
            top4NameText.text = list[3].username;
            top4ScoreText.text = list[3].score.ToString();
        }
        else
        {
            top4NameText.text = "---";
            top4ScoreText.text = "---";
        }
        if (list.Length > 4)
        {
            top5NameText.text = list[4].username;
            top5ScoreText.text = list[4].score.ToString();
        }
        else
        {
            top5NameText.text = "---";
            top5ScoreText.text = "---";
        }
    }
}