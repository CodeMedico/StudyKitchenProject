using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public EventHandler OnPlayerGetObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
   
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                OnPlayerGetObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
