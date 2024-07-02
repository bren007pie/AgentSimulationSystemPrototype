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

            // corresponds to the index of the item being added
            InventoryUpdate[itemid] = 1;

            EMgineController.UpdateEMgine(InventoryUpdate);

        }
    }
}