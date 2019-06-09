
public enum ResourceTypes {
    WOOD,
    STEEL,
    GOLD
}

public class Resource {
    int count = 100;

    public void SpendResource(int qty) {
        count -= qty;
    }

    public bool CanSpendResource(int qty) {
        return qty <= count;
    }

    public void AddResource(int qty) {
        count += qty;
    }

    public int GetCount() {
        return count;
    }
}