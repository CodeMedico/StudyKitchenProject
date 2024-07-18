using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footStepsTimer;
    private float footStepsTimerMax = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footStepsTimer -= Time.deltaTime;
        if (footStepsTimer < 0f)
        {
            footStepsTimer = footStepsTimerMax;
            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFootStepSound();
            }
        }
    }
}
