/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking; //this
using System.Text;
//using PlayFab;
//using PlayFab.ClientModels;
//using PlayFab.MultiplayerModels;

public class Leaderboard : MonoBehaviour
{
    public TMP_InputField nameInput;

    public GameObject leaderboardCanvas;

    public GameObject loginCanvas;
    public GameObject[] leaderboardEntries;
    private string savedToken;

    private string baseUrl = "http://localhost:8000/api";

    //Call Laravel API

    public static Leaderboard instance;

    [System.Serializable]
    public class AuthResponse
    {
        public string token;
    }


    void Awake() { instance = this; }

    void Start()
    {
        StartCoroutine(CallApi());
    }

    public void Login(string username)
    {
        StartCoroutine(LoginRoutine(username));
    }

    public void OnLoginButton()
    {
        string name = nameInput.text;
        loginCanvas.SetActive(false);

        Login(name);
    }

    IEnumerator LoginRoutine(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", "test123"); // temporary

        using (UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AuthResponse response =
                    JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);

                savedToken = response.token;

                Debug.Log("Login success");

                StartCoroutine(CallApi()); // fetch leaderboard after login
            }
            else
            {
                Debug.LogError(www.downloadHandler.text);
            }
        }
    }


    IEnumerator CallApi()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/leaderboard"))
        {
            yield return request.SendWebRequest(); //Get score

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("API Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                LeaderboardWrapper data =
                    JsonUtility.FromJson<LeaderboardWrapper>(jsonResponse);

                foreach (var entry in data.items)
                {
                    Debug.Log(entry.username + " - " + entry.score);
                }
            }
        }
    }
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
*/