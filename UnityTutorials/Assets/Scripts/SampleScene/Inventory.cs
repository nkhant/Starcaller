using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public class Equips
    {
        // Class Fields
        public string head;
        public string body;
        public string boots;

        // Constructors
        public Equips()
        {
            this.head = "Bronze Helmet";
            this.body = "Bronze Armor";
            this.boots = "Bronze Boots";
        }

        public Equips(string head, string body, string boots)
        {
            this.head = head;
            this.body = body;
            this.boots = boots;
        }
    }
    
    public class Use
    {
        // Class Fields
        public int potions;
        public int ninjaStars;
        public int arrows;

        // Constructors
        public Use()
        {
            this.potions = 0;
            this.ninjaStars = 0;
            this.arrows = 0;
        }

        public Use(int potions, int ninjaStars, int arrows)
        {
            this.potions = potions;
            this.ninjaStars = ninjaStars;
            this.arrows = arrows;
        }
    }
}
