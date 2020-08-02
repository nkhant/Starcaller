using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerStats : MonoBehaviour
{
    // Classes
    private Inventory.Equips playerEquips;
    private Inventory.Use playerUse;

    // Data Structures
    private string[] rotateEquips = {"Bronze", "Silver", "Gold", "Platinum", "Diamond", "Cat" };
    private List<GameObject> bodyParts = new List<GameObject>();
    public Material[] materials;

    // Variables
    private int currentHead = 0;
    private int currentBody = 0;
    private int currentBoots = 0;

    // References to GameObject Materials
    private Material headMaterial;
    private Material bodyMaterial;
    private Material bootsMaterial;

    // Start is called before the first frame update
    private void Awake()
    {
        playerEquips = new Inventory.Equips();
        playerUse = new Inventory.Use();
    }

    private void Start()
    {
        // Get Children (Body Parts) && Gather Material References
        foreach(Transform child in gameObject.transform)
        {
            bodyParts.Add(child.gameObject);
        }
        headMaterial = bodyParts[0].GetComponent<Renderer>().material;
        bodyMaterial = bodyParts[1].GetComponent<Renderer>().material;
        bootsMaterial = bodyParts[2].GetComponent<Renderer>().material;

        // Default Equipment
        PrintEquips();

        // Default Use
        PrintUse();
    }

    private void Update()
    {
        CheckOverflow();

        // Keep Current Equipment Updated (Can this be optimised to not check every frame?)
        playerEquips.head = rotateEquips[currentHead];
        playerEquips.body = rotateEquips[currentBody];
        playerEquips.boots = rotateEquips[currentBoots];

        // Rotate equipment upon request
        if (Input.GetButtonDown("RotateEquipHead"))
        {
            currentHead += 1;
        }
        else if (Input.GetButtonDown("RotateEquipArmor"))
        {
            currentBody += 1;
        }
        else if (Input.GetButtonDown("RotateEquipBoots"))
        {
            currentBoots += 1;
        }

        // Change look based on equipment set
        ChangeMaterial();

        // Print out equipment and use items
        if(Input.GetButtonDown("PrintEquips"))
        {
            PrintEquips();
        }
        if(Input.GetButtonDown("PrintUse"))
        {
            PrintUse();
        }
    }

    private void PrintEquips()
    {
        Debug.Log("Head: " + playerEquips.head);
        Debug.Log("Body: " + playerEquips.body);
        Debug.Log("Boots: " + playerEquips.boots);
    }

    private void PrintUse()
    {
        Debug.Log("Potion Amount: " + playerUse.potions);
        Debug.Log("Ninja Stars Amount: " + playerUse.ninjaStars);
        Debug.Log("Arrow Amount: " + playerUse.arrows);
    }

    private void CheckOverflow()
    {
        if (currentHead == rotateEquips.Length)
        {
            currentHead = 0;
        }
        if (currentBody == rotateEquips.Length)
        {
            currentBody = 0;
        }
        if (currentBoots == rotateEquips.Length)
        {
            currentBoots = 0;
        }
    }

    private void ChangeMaterial()
    {
        if(currentHead < rotateEquips.Length)
        {
            headMaterial.color = materials[currentHead].color;
        }
        if(currentBody < rotateEquips.Length)
        {
            bodyMaterial.color = materials[currentBody].color;
        }
        if(currentBoots < rotateEquips.Length)
        {
            bootsMaterial.color = materials[currentBoots].color;
        }
    }
}