using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI altInteractText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Transform pressToRebindKey;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;
        soundEffectButton.onClick.AddListener(() => { SoundManager.Instance.ChangeVolume(); UpdateVisual(); });
        musicButton.onClick.AddListener(() => { MusicManager.Instance.ChangeVolume();UpdateVisual(); });
        closeButton.onClick.AddListener(() => { Hide(); onCloseButtonAction(); });
        moveUpButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Interact) );
        altInteractButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.InteractAlternate));
        pauseButton.onClick.AddListener(() => RebindBinding(PlayerInput.Binding.Pause));
    }


    private void Start()
    {
        PlayerInput.Instance.OnPauseAction += PlayerInput_OnPauseAction; 
        
        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }
    private void PlayerInput_OnPauseAction(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume()*10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Up);
        moveDownText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Down);
        moveLeftText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Left);
        moveRightText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Move_Right);
        interactText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Interact);
        altInteractText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlternate);
        pauseText.text = PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction= onCloseButtonAction;
        gameObject.SetActive(true);

        closeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);


    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(false);
    }

    private void RebindBinding(PlayerInput.Binding binding)
    {
        ShowPressToRebindKey();
        PlayerInput.Instance.RebindBindings(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
