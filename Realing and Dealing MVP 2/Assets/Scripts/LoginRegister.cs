/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking; //this
using System.Text;

// CSRF
/*
public class LoginRegister : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public TextMeshProUGUI displayText; //idk if needed

    public UnityEvent onLoggedIn;

    public static LoginRegister instance;
    void Awake() { instance = this; }

    
    private string apiUrl = "http://127.0.0.1:8000/api/register";


    /*public void OnLoginButton()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(LoginRequest(email, password));
    }*/
    /*
    public void OnRegisterButton()
    {
        Debug.Log("button clicked");
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(RegisterRequest(email, password));
    }

    /*
        IEnumerator LoginRequest(string email, string password)
        {
            LoginData data = new LoginData(email, password);
            string jsonData = JsonUtility.ToJson(data);

            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData); //look into this more

            request.uploadHandler = new UploadHandlerRaw(bodyRaw); //Same here
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            Debug.Log("Request Sent");

            if (request.result == UnityWebRequest.Result.ConnectionError) //Failed to communicate with server
            {
                Debug.Log("Broken :(");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError) //Something serverside broken
            {
                SetDisplayText("Login failed: " + request.error, Color.red); 
            }
            else
            {
                Debug.Log("yippee!");
                /*string response = request.downloadHandler.text;
                Debug.Log("Response: " + response);

                // Parse token auth- FOR PROTOTYPE USE ONLY- I know this is not secure
                AuthResponse auth = JsonUtility.FromJson<AuthResponse>(response);

                if (!string.IsNullOrEmpty(auth.token))
                {
                    PlayerPrefs.SetString("auth_token", auth.token);

                    SetDisplayText("Login successful!", Color.green);

                    onLoggedIn?.Invoke();
                }
                else
                {
                    SetDisplayText("Invalid login", Color.red);
                }*//*
            }
        }*/
/*
    IEnumerator RegisterRequest(string email, string password)
    {
        Debug.Log("register request called");
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(apiUrl, form);

        yield return request.SendWebRequest();

        Debug.Log("Response Code: " + request.responseCode);
        Debug.Log("Response Body: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Connection failed");
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            SetDisplayText("Error: " + request.error, Color.red);
        }
        else
        {
            Debug.Log("Success!");
        }

        Debug.Log("Done!");
    }

    void SetDisplayText(string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }
}

    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;

        public LoginData(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }

    [System.Serializable]
    public class AuthResponse
    {
        public string token;
    }

    // Start is called before the first frame update
    /*public void OnRegisterButton()
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            result => SetDisplayText("Registered a new account as: " + result.PlayFabId, Color.green),
            error => SetDisplayText(error.ErrorMessage, Color.red)
        );
    }

    void SetDisplayText(string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }
(/)*/