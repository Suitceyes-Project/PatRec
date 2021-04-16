using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Happify.User;

public class Navigation : MonoBehaviour, IPointerClickHandler
{
    public AudioSource _audio;
    private float interval = 0.3f;
    int tap = 0;

    private UserDescription currentUser; 

    public void Start()
    {
        currentUser = UserManager.Instance.CurrentUser;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        string objName = eventData.selectedObject.name;

        if (!currentUser.RemainingVision && currentUser.RemainingHearing)
        {
            tap++;

            if (tap == 1)
            {
                StartCoroutine(DoubleTapInterval());
                OnDoubleClick(objName);
            }
            else if (tap > 1)
            {
                OnSingleClick(objName);
                tap = 0;
            }
        }
        else
            OnSingleClick(objName);
    }

    IEnumerator DoubleTapInterval()
    {
        yield return new WaitForSeconds(interval);
        this.tap = 0;
    }

    void OnDoubleClick(string button)
    {
        if (button.Equals("Play") || button.Equals("BackToLevels"))
            StartCoroutine(DownloadTheAudio("Naar levels"));
        else if (button.Equals("Settings"))
            StartCoroutine(DownloadTheAudio("Naar instellingen"));
        else if (button.Equals("Achievements") || button.Equals("BackToAchievements"))
            StartCoroutine(DownloadTheAudio("Naar badges collectie"));
        else if (button.Equals("ListOfScores"))
            StartCoroutine(DownloadTheAudio("Naar de lijst met scores per persoon"));
        else if (button.Equals("BackToHome"))
            StartCoroutine(DownloadTheAudio("Terug naar menu"));
        //else if (button.Equals("StudySymbols"))
        //    StartCoroutine(DownloadTheAudio("Patronen bestuderen"));
        //else if (button.Equals("PlayGame"))
        //    StartCoroutine(DownloadTheAudio("Speel het spel"));
        //else if (button.Equals("Previous"))
        //    StartCoroutine(DownloadTheAudio("Vorige"));
        //else if (button.Equals("Next"))
        //    StartCoroutine(DownloadTheAudio("Volgende"));
        else if (button.Equals("replayPattern"))
            StartCoroutine(DownloadTheAudio("Speel patroon opnieuw"));
    }

    void OnSingleClick(string button)
    {
        if (button.Equals("Play") || button.Equals("BackToLevels"))
            SceneManager.LoadScene("scn_Levels");
        else if (button.Equals("Settings"))
            SceneManager.LoadScene("scn_Settings");
        else if (button.Equals("Achievements"))
            SceneManager.LoadScene("scn_Achievements");
        else if (button.Equals("BackToHome"))
            SceneManager.LoadScene("scn_MainMenu");
        //else if (button.Equals("StudySymbols"))
        //    SceneManager.LoadScene("scn_StudyEnvironment");
        //else if (button.Equals("PlayGame"))
        //    SceneManager.LoadScene("scn_MainGameScreen");
    }
    IEnumerator DownloadTheAudio(string message)
    {
        using (UnityWebRequest website = UnityWebRequestMultimedia.GetAudioClip("https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + message + "&tl=NL", AudioType.MPEG))
        {
            yield return website.SendWebRequest();

            if (website.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(website.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(website);
                _audio.clip = myClip;
                _audio.Play();
            }
        }
    }

    void NextLevel()
    {

    }
}
