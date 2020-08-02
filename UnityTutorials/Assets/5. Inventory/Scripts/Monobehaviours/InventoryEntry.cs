using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry
{
    public ItemPickUp inventoryEntry;
    public int stackSize;
    public int inventorySlot;
    public int hotBarSlot;
    public Sprite hbSprite;

    public InventoryEntry(int stackSize, ItemPickUp inventoryEntry, Sprite hbSprite)
    {
        this.inventoryEntry = inventoryEntry;

        this.stackSize = stackSize;
        this.hotBarSlot = 0;
        this.inventorySlot = 0;
        this.hbSprite = hbSprite;
    }
}
