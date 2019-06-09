using UnityEngine;

public class MarketView : MonoBehaviour {
    
    public GameObject MarketCardPrefab;

    public GameObject ListContainer;

    private void Start() {
        var MarketData = DataManager.Instance.LoadedEntitiesData;
        foreach (var MarketCardData in MarketData)
        {
            if(MarketCardData is BuildingData) {
                var data = MarketCardData as BuildingData;
                var newCard = Instantiate(MarketCardPrefab);
                newCard.transform.SetParent(ListContainer.transform);
                newCard.GetComponent<MarketCard>().Setup(data);
            }
        }
    }
}