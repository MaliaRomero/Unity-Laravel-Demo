using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContainer;
    //public TextMeshProUGUI statusText;
    /*
    private void OnEnable()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        //statusText.text = "Loading leaderboard...";
        StartCoroutine(ApiClient.Instance.GetLeaderboard(
            onSuccess: (data) =>
            {
                //statusText.text = "Leaderboard loaded";

                foreach (Transform child in leaderboardContainer)
                    Destroy(child.gameObject);

                if (data.items == null || data.items.Length == 0)
                {
                    //statusText.text = "No entries yet";
                    return;
                }

                foreach (var entry in data.items)
                {
                    var go = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
                    var text = go.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = $"{entry.username} - {entry.score}";
                }
            },
            onError: (err) =>
            {
                //statusText.text = "Error: " + err;
            }
        ));
    }*/
}
