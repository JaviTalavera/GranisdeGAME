using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GooglePlayHandler : Singleton<GooglePlayHandler>
{
    #region PRIVATE_VAR
    private string leaderboard = GPGSIds.leaderboard_puntuacion;

    //private Text signInButtonText;
    //private Text authStatus;

    #endregion
    #region DEFAULT_UNITY_CALLBACKS
    void Start()
    {

        //signInButtonText = GameObject.Find("txtLogin").GetComponentInChildren<Text>();
        //authStatus = GameObject.Find("txtEstado").GetComponent<Text>();

        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;


        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }
    #endregion
    #region BUTTON_CALLBACKS

    /// <summary>
    /// Login In Into Your Google+ Account
    /// </summary>
    public void LogIn()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            // Sign out of play games
            //PlayGamesPlatform.Instance.SignOut();

            // Reset UI
            //signInButtonText.text = "Sign In";
            //authStatus.text = "";
        }
    }

    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("(Lollygagger) Signed in!");
            // Change sign-in button text
            //signInButtonText.text = "Sign out";

            // Show the user's name
            //authStatus.text = "Signed in as: " + Social.localUser.userName;
            SceneManager.LoadScene("02_Menu");
        }
        else
        {
            Debug.Log("(Lollygagger) Sign-in failed...");
            //authStatus.text = "Error :(";
        }
    }
    /// <summary>
    /// Shows All Available Leaderborad
    /// </summary>
    public void OnShowLeaderBoard()
    {

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboard);
        }
        else
        {
            Debug.Log("Cannot show leaderboard: not authenticated");
        }
        //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard); // Show current (Active) leaderboard
    }
    /// <summary>
    /// Adds Score To leader board
    /// </summary>
    public void OnAddScoreToLeaderBorad(int puntuacion)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(puntuacion, leaderboard, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");

                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
            OnShowLeaderBoard();
        }
    }
    /// <summary>
    /// On Logout of your Google+ Account
    /// </summary>
    public void OnLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }
    #endregion
}