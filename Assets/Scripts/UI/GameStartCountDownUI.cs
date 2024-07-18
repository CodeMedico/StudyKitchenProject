using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;


    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        Hide();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountDownTimer()).ToString();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
