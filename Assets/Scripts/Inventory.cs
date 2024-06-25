using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public EMgineController EMgineController;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the inventory");

        if (other.TryGetComponent<Item>(out Item item))
        {
            int itemid = item.id;
            Debug.Log("Item with id " + itemid + " is in the inventory");

            int[] InventoryUpdate = new int[] { 0, 0, 0 };
            InventoryUpdate[itemid] = 1; // corresponds to the index of the item being added

            //TODO: This is why something triggers an error when it leaves? Because it always passes an inventory update of 1? 

            EMgineController.UpdateEMgine(InventoryUpdate);

        }


    }
}
