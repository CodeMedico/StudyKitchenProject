using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimation : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGetObject += ContainerCounter_OnPlayerGetObject;
    }

    private void ContainerCounter_OnPlayerGetObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
