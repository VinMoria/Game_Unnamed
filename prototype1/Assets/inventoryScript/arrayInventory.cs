using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Inventory", menuName="Inventory/New Array Inventory")]
public class arrayInventory : ScriptableObject
{
    public item[] itemArray = new item[5];
}
