using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class LaravelManager : MonoBehaviour
{
//-----------------Inputs-----------------
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
//-----------------UI-----------------
    public GameObject registerCanvas;
    public GameObject loginCanvas;
    public TextMeshProUGUI LoginDisplayText;
    public TextMeshProUGUI RegisterDisplayText;
    public LeaderboardUI leaderboardUI;

//-----------------VARIABLES-----------------
    private string savedToken;
    private string baseUrl = "http://127.0.0.1:8000/api";

    public UnityEvent StartGame;

    public string currentUsername;
    private Coroutine leaderboardCoroutine;

//-----------------LEADERBOARD-----------------

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

//-----------------LOGIN/REGISTER-----------------
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

    public void Login()
    {
        StartCoroutine(LoginRoutine(
            nameField.text,
            passwordField.text
        ));
    }

    public void Register()
    {
        Debug.Log("Register Button");
        StartCoroutine(RegisterRoutine(
            nameField.text,
            passwordField.text
        ));
    }

    IEnumerator LoginRoutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/login", form))
        {
            yield return www.SendWebRequest();

            string responseText = www.downloadHandler.text;

            if (www.result == UnityWebRequest.Result.Success)
            {
                AuthResponse response =
                    JsonUtility.FromJson<AuthResponse>(responseText);

                savedToken = response.token;
                currentUsername = username;

                StartGame?.Invoke();
                StartLeaderboardLoop();
            }
            else
            {
                Debug.LogError(responseText);

                // Errors for UI
                if (www.responseCode == 404)
                {
                    LoginDisplayText.text = "An account with that username does not exist!";
                    LoginDisplayText.gameObject.SetActive(true);
                }

                else if (www.responseCode == 401)
                {
                    LoginDisplayText.text = "Incorrect password. Please try again!";
                    LoginDisplayText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Login failed.");
                }
            }
        }
    }

    IEnumerator RegisterRoutine(string username, string password)
    {
        Debug.Log("RegisterRouotine");
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/register", form))
        {
            yield return www.SendWebRequest();

            string responseText = www.downloadHandler.text;

            if (www.result == UnityWebRequest.Result.Success ||
                www.responseCode == 201)
            {
                onLoginPage();
                LoginDisplayText.text = "Account created successfully!";
            }
            else
            {
                Debug.LogError(responseText);

                if (www.responseCode == 422)
                {
                    Debug.Log("Username already exists.");
                }
                else
                {
                    Debug.Log("Registration failed.");
                }
            }
        }
    }

//-----------------SCORE-----------------

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

//---------------- LEADERBOARD ----------------

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