using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSucces;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSo recipeListSO;


    private List<RecipeSO> waitingRecipeList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitningRecipeMax = 4;
    private int successfulRecipesDelivered;

    private void Awake()
    {
        waitingRecipeList = new List<RecipeSO>();
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeList.Count < waitningRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                waitingRecipeList.Add(waitingRecipeSO);

                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    waitingRecipeList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);  
                    OnRecipeSucces?.Invoke(this, EventArgs.Empty);
                    successfulRecipesDelivered++;
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipesSOList()
    {
        return waitingRecipeList;
    }

    public int GetSuccsesfulRecipeDelivered()
    {
        return successfulRecipesDelivered;
    }
}
