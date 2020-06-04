using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Inventory/New Item")]
public class item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemInfo;
    public Sprite itemImage;
    public int itemHeld;
}
