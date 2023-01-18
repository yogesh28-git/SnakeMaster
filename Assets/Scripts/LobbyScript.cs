using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private AudioController audiocontroller;
    [SerializeField] private GameObject highScoreObj;
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private Button single;
    [SerializeField] private Button deathMatch;
    [SerializeField] private Button highScore;
    [SerializeField] private Button quit;
    [SerializeField] private Button yesQuit;
    [SerializeField] private Button noQuit;
    [SerializeField] private Button back;
    [SerializeField] private Button reset;
    private int highScoreValue;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private bool singleLoad = false;
    private bool deathmacthLoad = false;

    private void Start()
    {
        highScoreObj.SetActive(false);
        areYouSure.SetActive(false);
    }
    private void Update()
    {
        single.onClick.AddListener(LoadSingle);
        deathMatch.onClick.AddListener(LoadDeathMatch);
        highScore.onClick.AddListener(EnableHighScore);
        quit.onClick.AddListener(EnableQuit);
        yesQuit.onClick.AddListener(YesQuit);
        noQuit.onClick.AddListener(DisableQuit);
        back.onClick.AddListener(DisableHighScore);
        reset.onClick.AddListener(ResetScore);

        if (singleLoad)
        {
            StartCoroutine(LoadYourAsyncScene(2));
        }
        if (deathmacthLoad)
        {
            StartCoroutine(LoadYourAsyncScene(1));
        }
    }

    private void LoadSingle()
    {
        audiocontroller.Play(Sounds.buttonClick);
        singleLoad = true;
    }
    private void LoadDeathMatch()
    {
        audiocontroller.Play(Sounds.buttonClick);
        deathmacthLoad = true;
    }
    private void EnableHighScore()
    {
        audiocontroller.Play(Sounds.buttonClick);
        highScoreObj.SetActive(true);
        highScoreValue = PlayerPrefs.GetInt("score");
        highScoreText.text = "High Score is " + highScoreValue;
    }
    private void DisableHighScore()
    {
        audiocontroller.Play(Sounds.buttonClick);
        highScoreObj.SetActive(false);
    }
    private void EnableQuit()
    {
        audiocontroller.Play(Sounds.buttonClick);
        areYouSure.SetActive(true);
    }
    private void DisableQuit()
    {
        audiocontroller.Play(Sounds.buttonClick);
        areYouSure.SetActive(false);
    }
    private void YesQuit()
    {
        audiocontroller.Play(Sounds.buttonClick);
        Application.Quit();
    }
    private void ResetScore()
    {
        audiocontroller.Play(Sounds.buttonClick);
        PlayerPrefs.DeleteAll();
        highScoreText.text = "High Score is 0";
    }

    IEnumerator LoadYourAsyncScene(int i)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i);
        while (!asyncLoad.isDone)
        {
            singleLoad = false;
            deathmacthLoad = false;
            yield return null;
        }
    }
}
