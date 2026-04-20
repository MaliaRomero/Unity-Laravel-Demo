using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI statusText;
    public GameObject loginCanvas;
    public GameObject leaderboardCanvas;

    public void OnLoginButton()
    {
        string username = usernameInput.text;
        string password = string.IsNullOrEmpty(passwordInput.text) ? "test123" : passwordInput.text;

        statusText.text = "Logging in...";
        StartCoroutine(ApiClient.Instance.Login(
            username,
            password,
            onSuccess: (resp) =>
            {
                statusText.text = "Login successful";
                loginCanvas.SetActive(false);
                leaderboardCanvas.SetActive(true);
            },
            onError: (err) =>
            {
                statusText.text = "Login failed: " + err;
            }
        ));
    }
}
