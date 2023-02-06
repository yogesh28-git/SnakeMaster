using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private AudioController audiocontroller;
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject snakeHead1;
    [SerializeField] private GameObject snakeHead2;
    [SerializeField] private Button restart;
    [SerializeField] private Button menu;
    [SerializeField] private Button menu2;
    [SerializeField] private Button resume;
    [SerializeField] private TextMeshProUGUI victory;

    private bool menuLoad = false;
    private bool restartLoad = false;
    void Start()
    {
        gameover.SetActive(false);
        pause.SetActive(false);
    }

    void Update()
    {
        restart.onClick.AddListener(Restart);
        menu.onClick.AddListener(Menu);
        menu2.onClick.AddListener(Menu);
        resume.onClick.AddListener(Resume);

        if (menuLoad)
        {
            StartCoroutine(LoadYourAsyncScene(0));
        }
        if (restartLoad)
        {
            StartCoroutine(LoadYourAsyncScene(SceneManager.GetActiveScene().buildIndex));
        }
    }
    private void Resume()
    {
        audiocontroller.Play(Sounds.buttonClick);
        pause.SetActive(false);
        snakeHead1.SetActive(true);
        snakeHead2.SetActive(true);
    }
    private void Restart()
    {
        audiocontroller.Play(Sounds.buttonClick);
        gameover.SetActive(false);
        restartLoad = true;
    }
    private void Menu()
    {
        audiocontroller.Play(Sounds.buttonClick);
        menuLoad = true;
    }
    public void EnableGameOver(string scoretext = null)
    {
        gameover.SetActive(true);

        if (scoretext!=null)
        {
            if(snakeHead1 == snakeHead2)
            {
                scoretext = "Draw !!!";
            }
            victory.text = scoretext;
        }
    }

    public void EnablePauseMenu()
    {
        pause.SetActive(true);
    }

    IEnumerator LoadYourAsyncScene(int i)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i);
        while (!asyncLoad.isDone)
        {
            menuLoad = false;
            restartLoad = false;
            yield return null;
        }
    }

}
