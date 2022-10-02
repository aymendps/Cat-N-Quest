using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public GameObject inventoryGrid;
    private Transform[] children;

    private void Awake()
    {
        instance = this;
        children = inventoryGrid.GetComponentsInChildren<Transform>(true);
    }

    public void UpdateInventoryGrid()
    {
        foreach (Transform child in children)
        {
            if (PlayerCharacterController.player.isInInventory(child.name))
            {
                child.gameObject.SetActive(true);
            }
            else if (child.name != inventoryGrid.name)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
