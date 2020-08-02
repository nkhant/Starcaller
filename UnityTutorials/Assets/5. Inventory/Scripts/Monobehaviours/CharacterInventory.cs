using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations
    public static CharacterInventory instance;
 
    //name is weird should be like hero stats
    public CharacterStats characterStats;
    GameObject foundStats;

    public Image[] hotBarDisplayHolders = new Image[4];
    public GameObject InventoryDisplayHolder;
    public Image[] inventoryDisplaySlots = new Image[30];
    //this is for just starting out
    public ItemPickUp_SO healthPotion;
    //public List<ItemPickUp_SO> startingItems;
    //public ItemPickUp[] startingItems;
    //private List<ItemPickUp> clonedItems;
    //private int startingPotionAmount = 3;

    int inventoryItemCap = 20;
    int idCount = 1;
    bool addedItem = true;

    public Dictionary<int, InventoryEntry> itemsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry itemEntry;
    #endregion

    #region Initializations
    private void Start()
    {
        instance = this;
        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();

        //inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();
        inventoryDisplaySlots = GameObject.Find("preInventoryHotbarDisplay").transform.GetChild(1).GetComponentsInChildren<Image>();

        characterStats = PlayerManager.Instance.GetPlayerStats(1); // Hard coded for single player atm

        for (int i = 0; i < hotBarDisplayHolders.Length; i++)
        {
            hotBarDisplayHolders[i] = GameObject.Find("preInventoryHotbarDisplay").transform.GetChild(0).transform.GetChild(i).GetComponent<Image>();
        }

        //foundStats = GameObject.FindGameObjectWithTag("Player");
        //characterStats = foundStats.GetComponent<CharacterStats>();

        //tartingItems = new ItemPickUp_SO[startingPotionAmount];
        //starting
        //for (int i = 0; i < startingPotionAmount; i++)
        //{
        //    clonedItems.Add(Object.Instantiate(startingItems[0]));
        //    StoreItem(clonedItems[i]);
        //    TryPickUp();
        //}
    }
    #endregion

    private void Update()
    {
        #region Hotbar Key presses - Called by Character conroller later

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TriggerItemUse(101);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TriggerItemUse(102);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerItemUse(103);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TriggerItemUse(104);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DisplayInventory();
        }

        #endregion

        if (!addedItem)
        {
            TryPickUp();
        }

    }
    public void StoreItem(ItemPickUp itemToStore)
    {
        //makes so character doesn't pick up multiple items when we run into an item
        //set false cause havent added yet
        addedItem = false;
        //Debug.Log("weapon storing");
        //Debug.Log("1: " + characterStats);
        //Debug.Log("2: " + characterStats.characterStats);
        //Debug.Log("3: " + itemToStore);
        //Debug.Log("4: " + itemToStore.itemDefinition);
        //Debug.Log("5: " + itemToStore.itemDefinition.itemWeight);

        if ((characterStats.characterStats.currentEncumbrance + itemToStore.itemDefinition.itemWeight) <= characterStats.characterStats.maxEncumbrance){
            itemEntry.inventoryEntry = itemToStore;
            itemEntry.stackSize = 1;
            itemEntry.hbSprite = itemToStore.itemDefinition.itemIcon;
            //Debug.Log("hello");
            //means item is gone and picked up
            itemToStore.gameObject.SetActive(false);
        }
    }

    void TryPickUp()
    {
        bool itsInInventory = true;

        //check to see if the item to be stored was properly submitted to the inventory
        // if fails do nothing
        if (itemEntry.inventoryEntry)
        {
            //check to see if any items exist in the inventory already - if not add
            if(itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInventory(addedItem);
            }
            //if items exist in inventory
            else
            {
                if (itemEntry.inventoryEntry.itemDefinition.isStackable)
                {
                    foreach(KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
                    {
                        //Does this item already exist in inventory? - continue if so
                        if(itemEntry.inventoryEntry.itemDefinition == ie.Value.inventoryEntry.itemDefinition)
                        {
                            //add 1 to stack and destroy the new instance
                            ie.Value.stackSize += 1;
                            AddItemToHotBar(ie.Value);
                            itsInInventory = true;
                            //assume that itempickup script is inner game object of any prefab
                            Object.Destroy(itemEntry.inventoryEntry.gameObject.transform.parent.gameObject);
                            break;
                        }
                        //if item does not exist already in inventory then continue here
                        else
                        {
                            itsInInventory = false;
                        }
                    }
                }
                //if item is not stackable then continue here
                else
                {
                    itsInInventory = false;

                    //if no space and item is not stackable - say inventory full
                    if(itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.inventoryEntry.gameObject.SetActive(true);
                        Debug.Log("Inventory is full");
                    }
                }

                //check if there is space in inventory - if yes, continue here
                if (!itsInInventory)
                {
                    addedItem = AddItemToInventory(addedItem);
                    itsInInventory = true;
                }
            }
        }
    }

    bool AddItemToInventory(bool finishedAdding)
    {
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.inventoryEntry), itemEntry.hbSprite));
        //assume that itempickup script is inner game object of any prefab
        Object.Destroy(itemEntry.inventoryEntry.gameObject.transform.parent.gameObject);

        FillInventoryDisplay();
        AddItemToHotBar(itemsInInventory[idCount]);

        idCount = IncreaseID(idCount);

        //reset itemEntry

        finishedAdding = true;
        return finishedAdding;
    }

    //creating new id based on the old id
    int IncreaseID(int currentID)
    {
        int newID = 1;
        for(int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID += 1;
            }
            else return newID;
        }
        return newID;
    }

    private void AddItemToHotBar(InventoryEntry itemForHotBar)
    {
        int hotBarCounter = 0;
        bool increaseCount = false;

        //check for open hotbar slot
        foreach(Image images in hotBarDisplayHolders)
        {
            hotBarCounter += 1;
            if(itemForHotBar.hotBarSlot == 0)
            {
                if(images.sprite == null)
                {
                    //Add item to open hotbar slot
                    itemForHotBar.hotBarSlot = hotBarCounter;
                    //change hotbar sprite to show item
                    images.sprite = itemForHotBar.hbSprite;
                    increaseCount = true;
                    break;

                }
            }
            else if(itemForHotBar.inventoryEntry.itemDefinition.isStackable)
            {
                increaseCount = true;
            }
        }

        if (increaseCount)
        {
            hotBarDisplayHolders[itemForHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text = itemForHotBar.stackSize.ToString();
        }

        increaseCount = false;
    }

    //shows and hide ui
    void DisplayInventory()
    {
        if(InventoryDisplayHolder.activeSelf == true)
        {
            InventoryDisplayHolder.SetActive(false);
        }
        else
        {
            InventoryDisplayHolder.SetActive(true);
        }
    }

    void FillInventoryDisplay()
    {
        int slotCounter = 9;
        foreach(KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            slotCounter += 1;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotCounter - 9;
        }

        while (slotCounter < 29)
        {
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
        }
    }

    public void TriggerItemUse(int itemToUseID)
    {
        bool triggerItem = false;

        foreach(KeyValuePair<int, InventoryEntry> ie in itemsInInventory)
        {
            if(itemToUseID > 100)
            {
                itemToUseID -= 100;
                if(ie.Value.hotBarSlot == itemToUseID)
                {
                    triggerItem = true;
                }
            }
            else
            {
                if(ie.Value.inventorySlot == itemToUseID)
                {
                    triggerItem = true;
                }
            }

            if (triggerItem)
            {
                if (ie.Value.stackSize == 1)
                {
                    if (ie.Value.inventoryEntry.itemDefinition.isStackable)
                    {
                        if (ie.Value.hotBarSlot != 0)
                        {
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
                            hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
                        }
                        Debug.Log("Using " + ie.Value.inventoryEntry);
                        ie.Value.inventoryEntry.UseItem();
                        itemsInInventory.Remove(ie.Key);
                        break;
                    }
                    else
                    {
                        ie.Value.inventoryEntry.UseItem();
                        if (!ie.Value.inventoryEntry.itemDefinition.isIndestructable)
                        {
                            itemsInInventory.Remove(ie.Key);
                            break;
                        }
                        break;
                    }
                }
                else
                {
                    ie.Value.inventoryEntry.UseItem();
                    ie.Value.stackSize -= 1;
                    hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = ie.Value.stackSize.ToString();
                    break;
                }
            }
        }

        FillInventoryDisplay();
    }
}
