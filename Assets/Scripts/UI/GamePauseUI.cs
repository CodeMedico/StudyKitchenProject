using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            isGamePaused = !isGamePaused;
            Time.timeScale = 1;
            Hide();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show(Show);
            Hide();
        });
    }

    public void Start()
    {
        PlayerInput.Instance.OnPauseAction += Instance_OnPauseAction;
        Hide();
    }

    private void Instance_OnPauseAction(object sender, System.EventArgs e)
    {
        if (isGamePaused)
        {
            Hide();
        }
        else
        {
            Show();
        }
        isGamePaused =!isGamePaused;
    }


    private void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
