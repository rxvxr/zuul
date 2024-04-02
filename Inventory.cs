class Inventory 
{
    private int maxWeight;
    private int currentWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.currentWeight = 0;
        this.items = new Dictionary<string, Item>();
    }

        public bool Put(string itemName, Item item)
    {
        // Check inv space
        if (currentWeight + item.Weight <= maxWeight && !items.ContainsKey(itemName))
        {
            // Put items in dictionary
            items[itemName] = item;
            currentWeight += item.Weight;
            return true;
        }
        return false;
    }

    public Item Get(string itemName)
    {
        // Check item in dictionary 
        if (items.ContainsKey(itemName))
        {
            Item item = items[itemName];
            items.Remove(itemName);
            //  Remove item from dictionary
            currentWeight -= item.Weight;
            //  Total weight - item weight
            return item;
        }
        return null;
    }

    public Dictionary<string, Item> RetrieveItems()
    {
        return items;
    }
}