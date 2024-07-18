using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjetcSO;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private float spawnPlateTimer = 0f;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnAmount;
    private int platesSpawnAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;
                
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnAmount> 0)
            {
                platesSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjetcSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

