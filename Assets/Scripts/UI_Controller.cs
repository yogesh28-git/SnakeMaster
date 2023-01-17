using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject snakeHead;
    [SerializeField] private Button restart;
    [SerializeField] private Button menu;
    [SerializeField] private Button menu2;
    [SerializeField] private Button resume;

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
            StartCoroutine(LoadYourAsyncScene(2));
        }
        
    }
    private void Resume()
    {
        pause.SetActive(false);
        snakeHead.SetActive(true);
    }
    private void Restart()
    {
        gameover.SetActive(false);
        restartLoad = true;
    }
    private void Menu()
    {
        menuLoad = true;
    }
    public void EnableGameOver()
    {
        gameover.SetActive(true);
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
