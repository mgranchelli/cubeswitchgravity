using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Transform usernameLogin;
    public Transform passwordLogin;
    public Transform emailRegister;
    public Transform usernameRegister;
    public Transform passwordRegister;
    public Transform confirmPasswordRegister;
    public Text errorLoginText;

    public Text errorUsernameRegisterText;
    public Text errorEmailRegisterText;
    public Text errorPasswordRegisterText;
    public Text containPasswordRegisterText;
    public Text errorConfPasswordRegisterText;


    public static string Email = "";
    public static string Password = "";

    public string CurrentMenu;

    private string CreateAccountUrl = "https://gmanu.altervista.org/Game/Register.php";
    private string LoginUrl = "https://gmanu.altervista.org/Game/Login.php";
    private string ConfirmPass = "";

    private PasswordHasher passwordHasher;
    private string passwordVerify = null;
    private bool passwordOk = false;
    private bool isSingIn;

    private float timeout;

    // Start is called before the first frame update
    void Start()
    {
        containPasswordRegisterText.gameObject.SetActive(false);
        errorLoginText.gameObject.SetActive(false);
        errorUsernameRegisterText.gameObject.SetActive(false);
        errorEmailRegisterText.gameObject.SetActive(false);
        errorPasswordRegisterText.gameObject.SetActive(false);
        errorConfPasswordRegisterText.gameObject.SetActive(false);

        passwordHasher = new PasswordHasher();
        CurrentMenu = "Login";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CheckIsLogin()
    {
        if (isSingIn)
            print("Login Ok");
        else
            print("Errore di login");
    }





    public void CreateAccount()
    {
        string newUser = usernameRegister.GetComponent<Text>().text.Trim();
        TextLocalizer tl = errorConfPasswordRegisterText.GetComponent<TextLocalizer>();

        if  (confirmPasswordRegister.GetComponent<Text>().text == "")
        {
            errorConfPasswordRegisterText.text = tl.Traslate("confirm_password");
            errorConfPasswordRegisterText.gameObject.SetActive(true);
            return;
        }

        if (!passwordOk)
        {
            errorConfPasswordRegisterText.text = tl.Traslate("password_not_equals");
            errorConfPasswordRegisterText.gameObject.SetActive(true);
            return;
        }
            
        isSingIn = false;
        StartCoroutine(CreateAccountRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                string CreateAccountReturn = req.downloadHandler.text;

                if (CreateAccountReturn.Equals("Success"))
                {
                    isSingIn = true;
                    Debug.Log("Account created!");
                    DataManager.Instance.SetAccount(newUser);
                    DataManager.Instance.SingInSuccess();
                }
                else if (CreateAccountReturn.Equals("Email already used"))
                {
                    Debug.Log("Email already used!");
                }
                else if (CreateAccountReturn.Equals("Username already used"))
                {
                    Debug.Log("Username already used!");
                }
                else
                {
                    Debug.Log("Error! " + CreateAccountReturn);
                }
            }

        }));
    }

    public void SingIn()
    {
        StartCoroutine(SingInRequest((UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("Cannot connect!");
            }
            else
            {
                string LoginAccountReturn = req.downloadHandler.text;
                if (IsBase64String(LoginAccountReturn))
                {
                    
                    if (passwordHasher.VerifyHashedPassword(LoginAccountReturn, passwordLogin.GetComponent<Text>().text) == 0)
                    {
                        ErrorLogin("Account");
                        Debug.Log("Password error!");
                    }
                        
                    else
                    {
                        print("sigin check: " + passwordHasher.VerifyHashedPassword(LoginAccountReturn, passwordLogin.GetComponent<Text>().text));
                        DataManager.Instance.SetAccount(usernameLogin.GetComponent<Text>().text.Trim());
                        DataManager.Instance.SingInSuccess();
                    }
                }
                else if (LoginAccountReturn.Equals("Account not found"))
                {
                    ErrorLogin("Account");
                    Debug.Log("Account not found!");
                }
                else
                {
                    ErrorLogin("Error");
                    Debug.Log("Error! " + LoginAccountReturn);
                }
            }

        }));
        
    }

    

    IEnumerator SingInRequest(Action<UnityWebRequest> callback)
    {
        WWWForm Form = new WWWForm();

        Form.AddField("Username", usernameLogin.GetComponent<Text>().text.Trim());
        Form.AddField("Email", usernameLogin.GetComponent<Text>().text.Trim());

        UnityWebRequest LoginAccountWWW = UnityWebRequest.Post(LoginUrl, Form);

        yield return LoginAccountWWW.SendWebRequest();
        callback(LoginAccountWWW);
    }

    IEnumerator CreateAccountRequest(Action<UnityWebRequest> callback)
    {
        WWWForm Form = new WWWForm();


        Form.AddField("Username", usernameRegister.GetComponent<Text>().text.Trim());
        Form.AddField("Email", emailRegister.GetComponent<Text>().text.Trim());
        Form.AddField("Password", passwordHasher.HashPassword(passwordRegister.GetComponent<Text>().text));

        UnityWebRequest CreateAccountWWW = UnityWebRequest.Post(CreateAccountUrl, Form);
        
        yield return CreateAccountWWW.SendWebRequest();
        callback(CreateAccountWWW);
    }

    public void Logout()
    {
        DataManager.Instance.Logout();
    }

    private bool IsBase64String(string str)
    {
        try
        {
            // If not exception is cought, then it is a base64 string
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(str));
            return true;
        }
        catch
        {
            // If exception is cought, then I assumed it is a normal string
            return false;
        }
    }

    public void EmailValidator(string email)
    {
        TextLocalizer tl = errorEmailRegisterText.GetComponent<TextLocalizer>();
        if (!IsValidEmail(email))
        {
            Debug.Log("Email not valid!");
            errorEmailRegisterText.text = tl.Traslate("email_not_valid");
            errorEmailRegisterText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Email valid!");
            errorEmailRegisterText.gameObject.SetActive(false);
        }
    }

    public void ConfirmPasswordValidator(string password)
    {
        TextLocalizer tl = errorConfPasswordRegisterText.GetComponent<TextLocalizer>();
        if (passwordVerify.Equals(password))
        {
            Debug.Log("Confirm password valid!");
            errorConfPasswordRegisterText.gameObject.SetActive(false);
            passwordOk = true;
        }
        else
        {
            Debug.Log("Confirm password not valid!");
            errorConfPasswordRegisterText.text = tl.Traslate("password_not_equals");
            errorConfPasswordRegisterText.gameObject.SetActive(true);
            passwordOk = false;
        }
    }

    public void PasswordValidator(string password)
    {
        if (!IsValidPassword(password))
        {
            Debug.Log("password not valid!");
            errorPasswordRegisterText.gameObject.SetActive(true);
            containPasswordRegisterText.gameObject.SetActive(true);
        }
        else
        {
            passwordVerify = password;
            Debug.Log("password valid!");
            errorPasswordRegisterText.gameObject.SetActive(false);
            containPasswordRegisterText.gameObject.SetActive(false);
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            print("leggo la mail");
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                var domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private static bool IsValidPassword(string input)
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMinimum8Chars = new Regex(@".{8,}");
        var special = new Regex(@"[!@#$%^&*()]");
        var sampleRegex = new Regex(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{2,})$");

        return hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input) && special.IsMatch(input);
    }

    private void ErrorLogin(string error)
    {
        if (error.Equals("Account"))
            errorLoginText.text = "Username o password errate!";
        else
            errorLoginText.text = "Riprova";
        errorLoginText.gameObject.SetActive(true);
    }

    public void CheckUser(string user)
    {
        TextLocalizer tl = errorUsernameRegisterText.GetComponent<TextLocalizer>();
        if (usernameRegister.GetComponent<Text>().text.Trim().Length < 3)
        {
            errorUsernameRegisterText.text = tl.Traslate("min_user_characters");
            errorUsernameRegisterText.gameObject.SetActive(true);
            return;
        }
        else
        {
            errorUsernameRegisterText.gameObject.SetActive(false);
        }

        StartCoroutine("IsNewUser");
    }

    IEnumerator IsNewUser()
    {
        WWWForm Form = new WWWForm();
        string username = usernameRegister.GetComponent<Text>().text.Trim();
        string email = emailRegister.GetComponent<Text>().text.Trim();
        Form.AddField("Username", username);

        if (!email.Equals(""))
            Form.AddField("Email", email);

        string IsNewUserUrl = "https://gmanu.altervista.org/Game/IsNewUser.php";

        UnityWebRequest IsNewUserWWW = UnityWebRequest.Post(IsNewUserUrl, Form);

        yield return IsNewUserWWW.SendWebRequest();
        if (IsNewUserWWW.error != null)
        {
            Debug.Log("Cannot connect!");
        }
        else
        {
            TextLocalizer tlEmail = errorEmailRegisterText.GetComponent<TextLocalizer>();
            TextLocalizer tlUser = errorUsernameRegisterText.GetComponent<TextLocalizer>();
            string IsNewUserReturn = IsNewUserWWW.downloadHandler.text;

            if (IsNewUserReturn.Equals("Email used"))
            {
                Debug.Log("Email used!");
                errorEmailRegisterText.text = tlEmail.Traslate("email_used");
                errorEmailRegisterText.gameObject.SetActive(true);
            }
            else if (IsNewUserReturn.Equals("Username used"))
            {
                Debug.Log("Username used!");
                errorUsernameRegisterText.text = tlUser.Traslate("username_used");
                errorUsernameRegisterText.gameObject.SetActive(true);
            }
            else if (IsNewUserReturn.Equals("Is new user"))
            {
                Debug.Log("Is new user!");
                errorEmailRegisterText.gameObject.SetActive(false);
                errorUsernameRegisterText.gameObject.SetActive(false);
            }
            else if (IsNewUserReturn.Equals("Email user used"))
            {
                Debug.Log("Email and password used!");
            }
            else
            {
                Debug.Log("Error user return! " + IsNewUserReturn);
                errorUsernameRegisterText.text = tlUser.Traslate("error_retry");
                errorEmailRegisterText.text = tlEmail.Traslate("error_retry");
            }
        }
    }
}
