using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public event EventHandler<OnStateChangedEvents> OnStateChanged;
    public class OnStateChangedEvents : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;


    private FryingRecipeSO fryingRecipeSO;
    private State state;
    private float fryingTimer;
    private float burningTimer;


    private void Start()
    {
        state = State.Idle;
    }

    public void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    OnStateChanged?.Invoke(this, new OnStateChangedEvents
                    {
                        state = state
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEvents
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = burningTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (burningTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEvents
                        {
                            state = state
                        });
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs 
                    {
                        progressNormalized =  fryingTimer / fryingRecipeSO.fryingTimerMax 
                    });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEvents
                {
                    state = state
                });
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEvents
                        {
                            state = state
                        });
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
        }

    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);
        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
}
