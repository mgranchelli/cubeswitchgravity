using System;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class FacebookManager : MonoBehaviour
{
 
    public GameObject FBPanel;
    public Text WelcomeNameText;

    public Transform usernameRegister;
    public Text errorUsernameRegisterFBText;

    private string idFacebook = "";
    private string nameFacebook = "";
    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Couldn't inizialize!");
            },
            isGameShown =>
            {
                if (isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    /*
    public void Login()
    {
        FB.LogInWithReadPermissions(callback: OnLogIn);
    }

    private void OnLogIn(ILoginResult loginResult)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken token = AccessToken.CurrentAccessToken;
            userIdText.text = token.UserId;
        }
        else
        {
            Debug.Log("Cancel Login");
        }
    }
    */
    public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email"};
        FB.LogInWithReadPermissions(permissions);
        print("api");
        FB.API("me?fields=id,name,email", HttpMethod.GET, (result) =>
        {
            if (result.ResultDictionary["id"].ToString() != "")
            {
                idFacebook = result.ResultDictionary["id"].ToString();

                CheckIDFacebookDB();
                nameFacebook = result.ResultDictionary["name"].ToString();
                nameFacebook = nameFacebook.Substring(0, nameFacebook.IndexOf(" "));
                Debug.Log(nameFacebook);
                Debug.Log(result.ResultDictionary["id"].ToString());
                Debug.Log(result.ResultDictionary["name"].ToString().Trim());
                Debug.Log(result.ResultDictionary["email"].ToString());
            }
            else
                Debug.Log("Error Login Facebook!");
            
        });

    }

    public void FacebookLogout()
    {
        FB.LogOut();
    }

    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("https://www.google.it"), "Check it!", "Good game!");
    }

    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Go to play!", title: "CubeSwitchGravity");
    }

    public void FacebookInvite()
    {
        //FB.Mobile.app
    }


    


    void GetFacebookInfo(IResult result)
    { 
        IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
        //userIdText.text = dict["name"].ToString();
        string fbname = dict["name"].ToString();
        Debug.Log(fbname);
    }

    public void CheckIDFacebookDB()
    {
        StartCoroutine(CheckIDFacebookDBRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                string CheckIDFacebookDBReturn = req.downloadHandler.text;
                if (CheckIDFacebookDBReturn.Equals("Is new Facebook user"))
                {
                    Debug.Log("Is new User");
                    TextLocalizer tl = WelcomeNameText.GetComponent<TextLocalizer>();
                    WelcomeNameText.text = tl.Traslate("hi") + nameFacebook + "!";
                    FBPanel.gameObject.SetActive(true);
                }
                else if (CheckIDFacebookDBReturn.Equals("Error!"))
                {
                   
                    Debug.Log("Error!");
                }
                else
                {
                    DataManager.Instance.SetAccount(CheckIDFacebookDBReturn);
                    DataManager.Instance.SingInFBSuccess();
                    Debug.Log("User Already Exist!");
                }
            }

        }));

    }



    IEnumerator CheckIDFacebookDBRequest(Action<UnityWebRequest> callback)
    {
        string IdFacebookCheckURL = "https://gmanu.altervista.org/Game/IsNewFacebookUser.php";
        WWWForm Form = new WWWForm();

        Form.AddField("IdFacebook", idFacebook);

        UnityWebRequest LoginAccountWWW = UnityWebRequest.Post(IdFacebookCheckURL, Form);

        yield return LoginAccountWWW.SendWebRequest();
        callback(LoginAccountWWW);
    }

    public void RegisterIDFacebookDB()
    {
        TextLocalizer tl = errorUsernameRegisterFBText.GetComponent<TextLocalizer>();
        string newUser = usernameRegister.GetComponent<Text>().text.Trim();

        StartCoroutine(RegisterIDFacebookDBRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                string CheckIDFacebookDBReturn = req.downloadHandler.text;
                if (CheckIDFacebookDBReturn.Equals("Success"))
                {
                    errorUsernameRegisterFBText.gameObject.SetActive(false);
                    FBPanel.gameObject.SetActive(false);
                    DataManager.Instance.SetAccount(newUser);
                    DataManager.Instance.SingInFBSuccess();
                    Debug.Log("Created");
                }
                else if (CheckIDFacebookDBReturn.Equals("Username already used"))
                {
                    errorUsernameRegisterFBText.text = tl.Traslate("username_used");
                    errorUsernameRegisterFBText.gameObject.SetActive(true);

                    Debug.Log("Username already used");
                }
                else
                {
                    Debug.Log("Error!: " + CheckIDFacebookDBReturn);
                }
            }

        }));

    }



    IEnumerator RegisterIDFacebookDBRequest(Action<UnityWebRequest> callback)
    {
        string FacebookRegisterURL = "https://gmanu.altervista.org/Game/FacebookRegister.php";
        WWWForm Form = new WWWForm();

        Form.AddField("IdFacebook", idFacebook);
        Form.AddField("Username", usernameRegister.GetComponent<Text>().text.Trim());

        UnityWebRequest LoginAccountWWW = UnityWebRequest.Post(FacebookRegisterURL, Form);

        yield return LoginAccountWWW.SendWebRequest();
        callback(LoginAccountWWW);
    }


    
    
}
