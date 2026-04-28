using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class LaravelManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;

    public GameObject registerCanvas;

    public GameObject loginCanvas;

    private string savedToken;
    private string baseUrl = "http://127.0.0.1:8000/api";

    public UnityEvent StartGame;

    public string currentUsername;
    private Coroutine leaderboardCoroutine;

    public LeaderboardUI leaderboardUI;

    public void StartLeaderboardLoop()
    {
        if (leaderboardCoroutine == null)
        {
            leaderboardCoroutine = StartCoroutine(LeaderboardLoop(3f));
        }
    }

    public void StopLeaderboardLoop()
    {
        if (leaderboardCoroutine != null)
        {
            StopCoroutine(leaderboardCoroutine);
            leaderboardCoroutine = null;
        }
    }

    // ---------------- LOGIN ----------------

    public void Login()
    {
        StartCoroutine(LoginRoutine(
            nameField.text,
            passwordField.text
        ));
    }

    public void onRegisterPage()
    {
        registerCanvas.SetActive(true);
        loginCanvas.SetActive(false);
    }

    public void onLoginPage()
    {
        loginCanvas.SetActive(true);
        registerCanvas.SetActive(false);
    }


    IEnumerator LoginRoutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AuthResponse response =
                    JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);

                savedToken = response.token;

                currentUsername = username;

                Debug.Log("Login Successful!");
                StartGame?.Invoke();
                StartLeaderboardLoop();
            }
            else
            {
                Debug.LogError(www.downloadHandler.text);
            }
        }
    }

    // ---------------- SPRITE ----------------

    public void SendScore(int score)
    {
        if (string.IsNullOrEmpty(savedToken))
        {
            Debug.LogError("No token!");
            return;
        }

        StartCoroutine(SaveScoreRoutine(score));
    }

    IEnumerator SaveScoreRoutine(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);

        using (UnityWebRequest www =
               UnityWebRequest.Post($"{baseUrl}/update-score", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + savedToken);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score saved successfully" + score);
            }
            else
            {
                Debug.LogError(www.downloadHandler.text);
            }
        }
    }

    // ---------------- LEADERBOARD ----------------

    IEnumerator LeaderboardLoop(float refreshRate)
    {
        while (true)
        {
            yield return FetchLeaderboard();
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public IEnumerator FetchLeaderboard()
    {

        using (UnityWebRequest www =
               UnityWebRequest.Get($"{baseUrl}/leaderboard"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;

                LeaderboardWrapper data =
                    JsonUtility.FromJson<LeaderboardWrapper>(json);

                leaderboardUI.UpdateTop5(data.items);
            }
            else
            {
                Debug.LogError(www.downloadHandler.text);
            }
        }
    }
    public void refreshButton()
    {
        int score = GameManager.playerController.points;
        Debug.Log("Refresh clicked. Sending score: " + score);

        StartCoroutine(RefreshAfterSave(score));
    }

    IEnumerator RefreshAfterSave(int score)
    {
        yield return SaveScoreRoutine(score);
        yield return FetchLeaderboard();
    }
}

// ---------------- DATA MODELS ----------------

[System.Serializable]
public class AuthResponse
{
    public string token;
}

[System.Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}

[System.Serializable]
public class LeaderboardWrapper
{
    public LeaderboardEntry[] items;
}