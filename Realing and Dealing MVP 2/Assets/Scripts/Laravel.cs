using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Laravel : MonoBehaviour
{
    private string apiUrl = "http://ReelingAndDealing-Laravel.test/players";

    void Start()
    {
        StartCoroutine(CallApi());
    }

    IEnumerator CallApi()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("API Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("API Response: " + jsonResponse);
            }
        }
    }
}