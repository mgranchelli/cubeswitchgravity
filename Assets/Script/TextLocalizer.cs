using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    private static TextLocalizer _Instance;
    public static TextLocalizer Instance { get { return _Instance; } }

    [SerializeField] public string id;

    static Dictionary<string, Dictionary<string, string>> Translations = new Dictionary<string, Dictionary<string, string>>()
    {
        ["english"] = new Dictionary<string, string>()
        {
            ["play"] = "Play",
            ["menu_resume"] = "Resume",
            ["menu_settings"] = "Options",
            ["menu_highscore"] = "Highscores",
            ["play_to_rank"] = "Play to enter in rank!",
            ["sing_up_to_rank"] = "Sing in to enter in rank!",
            ["position_in_rank"] = "You are in x° position!",
            ["options"] = "Options",
            ["music"] = "Music",
            ["sound_effects"] = "Sound Effects",
            ["language"] = "Language",
            ["italian"] = "Italian",
            ["english"] = "English",
            ["enter_username"] = "Enter username...",
            ["enter_password"] = "Enter password...",
            ["enter_conf_password"] = "Confirm password...",
            ["enter_email"] = "Enter email...",
            ["sing_up"] = "Sing Up",
            ["set_username"] = "Set your username",
            ["continue"] = "Continue",
            ["logged_in"] = "Logged in: ",
            ["resume"] = "Resume",
            ["reload"] = "Reload",
            ["exit"] = "Exit",
            ["highscore_text"] = "Your score: ",
            ["best_highscore_text"] = "Best score: ",
            ["hi"] = "Hi ",
            ["left_jump"] = "- On left edge tap left to jump",
            ["left_switch"] = "- After jump tap right to switch gravity",
            ["right_jump"] = "- On right edge tap right to jump",
            ["right_switch"] = "- After jump tap left to switch gravity",
            ["avoid_enemies"] = "- Avoid the enemies",
            ["collect_money"] = "- Collect money and enter in ranking!",
            ["best_score"] = "Your best score: ",
            ["score"] = "Your score: ",
            ["password_contain"] = "The password must contain:\n- Minimum 8 characters\n- At least one uppercase character\n- At least one lowercase character\n- At least one number\n- At least one special character (!?$%)",
            ["password_not_valid"] = "Password not valid",
            ["confirm_password"] = "Confirm password!",
            ["password_not_equals"] = "Password is not equals!",
            ["email_not_valid"] = "Email not valid!",
            ["min_user_characters"] = "User must be at least 4 character!",
            ["email_used"] = "Email already Used!",
            ["username_used"] = "Username already Used!",
            ["error_retry"] = "Error! Retry!",
            ["restart"] = "Restart",
        },
        ["italian"] = new Dictionary<string, string>()
        {
            ["play"] = "Gioca",
            ["menu_resume"] = "Riprendi",
            ["menu_settings"] = "Impostazioni",
            ["menu_highscore"] = "Highscores",
            ["play_to_rank"] = "Gioca per entrare in classifica!",
            ["sing_up_to_rank"] = "Accedi per entrare in classifica!",
            ["position_in_rank"] = "Tu sei in x° posizione!",
            ["options"] = "Impostazioni",
            ["music"] = "Musica",
            ["sound_effects"] = "Suoni",
            ["language"] = "Lingua",
            ["italian"] = "Italiano",
            ["english"] = "Inglese",
            ["enter_username"] = "Inserisci username...",
            ["enter_password"] = "Inserisci password...",
            ["enter_conf_password"] = "Conferma password...",
            ["enter_email"] = "Inserisci email...",
            ["sing_up"] = "Registrati",
            ["set_username"] = "Inserisci username",
            ["continue"] = "Continua",
            ["logged_in"] = "Accesso eseguito: ",
            ["resume"] = "Continua",
            ["reload"] = "Ricarica",
            ["exit"] = "Esci",
            ["highscore_text"] = "Il tuo score: ",
            ["best_highscore_text"] = "Best score: ",
            ["hi"] = "Ciao ",
            ["left_jump"] = "- Sul bordo sinistro tocca a sinistra per saltare",
            ["left_switch"] = "- Dopo il salto tocca a destra per cambiare gravità",
            ["right_jump"] = "- Sul bordo destro tocca a destra per saltare",
            ["right_switch"] = "- Dopo il salto tocca a sinistra per cambiare gravità",
            ["avoid_enemies"] = "- Evita i nemici",
            ["collect_money"] = "- Raccogli le monete e entra in classifica!",
            ["best_score"] = "Il tuo best score: ",
            ["score"] = "Il tuo score: ",
            ["password_contain"] = "La password deve contenere:\n- Minimo 8 caratteri\n- Almeno un carattere maiuscolo\n- Almeno un carattere minuscolo\n- Almeno un numero\n- Almeno un carattere speciale (!?$%)",
            ["password_not_valid"] = "Password non valida",
            ["confirm_password"] = "Conferma la password!",
            ["password_not_equals"] = "Le password non coincidono!",
            ["email_not_valid"] = "Email non valida!",
            ["min_user_characters"] = "Username deve contenere almeno 4 caratteri!",
            ["email_used"] = "Email usata!",
            ["username_used"] = "Username non disponibile!",
            ["error_retry"] = "Errore! Riprova!",
            ["restart"] = "Ricomincia",

        }
    };
    

    public string ResolveStringValue(string id)
    {
        string language = PlayerPrefs.GetString("language");

        if (language == "")
            language = "english";

        return Translations[language][id];
    }

    void Start()
    {
        if (!id.Equals(""))
            GetComponent<Text>().text = ResolveStringValue(id);
    }

    private void Update()
    {
        if (!id.Equals(""))
            GetComponent<Text>().text = ResolveStringValue(id);
    }

    void OnValidate()
    {
        if (!id.Equals(""))
            GetComponent<Text>().text = ResolveStringValue(id);
    }

    public string Traslate(string id)
    {
        string language = PlayerPrefs.GetString("language");

        if (language == "")
            language = "english";

        return Translations[language][id];
    }
}
