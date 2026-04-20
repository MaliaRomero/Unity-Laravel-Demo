using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public static ApiClient Instance;

    [Header("API Config")]
    [SerializeField] private string baseUrl = "http://127.0.0.1:8000/api";

    [Header("Auth")]
    public string AuthToken; // set after login

    [Serializable]
    public class AuthResponse
    {
        public string token;
        // public User user; // if you want to parse user too
    }

    [Serializable]
    public class LeaderboardEntry
    {
        public string username;
        public int score;
    }

    [Serializable]
    public class LeaderboardWrapper
    {
        public LeaderboardEntry[] items;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------- LOGIN ----------

    public IEnumerator Login(string username, string password, Action<AuthResponse> onSuccess, Action<string> onError)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var json = www.downloadHandler.text;
                Debug.Log("Login response: " + json);

                AuthResponse response = JsonUtility.FromJson<AuthResponse>(json);

                if (!string.IsNullOrEmpty(response.token))
                {
                    AuthToken = response.token;
                    onSuccess?.Invoke(response);
                }
                else
                {
                    onError?.Invoke("No token in response");
                }
            }
            else
            {
                Debug.LogError("Login error: " + www.error);
                onError?.Invoke(www.downloadHandler.text);
            }
        }
    }

    // ---------- LEADERBOARD ----------

    public IEnumerator GetLeaderboard(Action<LeaderboardWrapper> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/leaderboard"))
        {
            // If you later protect this route with Sanctum:
            // request.SetRequestHeader("Authorization", "Bearer " + AuthToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("Leaderboard response: " + json);

                LeaderboardWrapper data = JsonUtility.FromJson<LeaderboardWrapper>(json);
                onSuccess?.Invoke(data);
            }
            else
            {
                Debug.LogError("Leaderboard error: " + request.error);
                onError?.Invoke(request.downloadHandler.text);
            }
        }
    }

    // ---------- OPTIONAL: PLAYERS TEST ----------

    public IEnumerator GetPlayersRaw(Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/players"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }
}
