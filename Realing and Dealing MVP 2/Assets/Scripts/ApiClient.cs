using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public static ApiClient Instance;

    [Header("API Config")]
    [SerializeField] private string baseUrl = "http://127.0.0.1:8000/api";

    // ---------- MODELS ----------

    [Serializable]
    public class UserData
    {
        public string username;
        public string name;
        public int score;
    }

    [Serializable]
    public class LoginResponse
    {
        public UserData user;
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

    public IEnumerator Login(string username, string password, Action<UserData> onSuccess, Action<string> onError)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login response: " + www.downloadHandler.text);

                LoginResponse response =
                    JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

                if (response != null && response.user != null)
                {
                    Debug.Log("Logged in as: " + response.user.username);
                    onSuccess?.Invoke(response.user);
                }
                else
                {
                    onError?.Invoke("Invalid response format");
                }
            }
            else
            {
                Debug.LogError("Login error: " + www.downloadHandler.text);
                onError?.Invoke(www.downloadHandler.text);
            }
        }
    }

    // ---------- LEADERBOARD ----------

    public IEnumerator GetLeaderboard(Action<LeaderboardWrapper> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/leaderboard"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Leaderboard response: " + request.downloadHandler.text);

                LeaderboardWrapper data =
                    JsonUtility.FromJson<LeaderboardWrapper>(request.downloadHandler.text);

                onSuccess?.Invoke(data);
            }
            else
            {
                Debug.LogError("Leaderboard error: " + request.downloadHandler.text);
                onError?.Invoke(request.downloadHandler.text);
            }
        }
    }

    // ---------- OPTIONAL TEST ----------

    public IEnumerator GetPlayersRaw(Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/players"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                onSuccess?.Invoke(request.downloadHandler.text);
            else
                onError?.Invoke(request.downloadHandler.text);
        }
    }
}