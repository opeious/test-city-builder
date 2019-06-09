using UnityEngine;
using System.Collections.Generic;

public class MarketView : MonoBehaviour {
    
    public GameObject MarketCardPrefab;

    public GameObject ListContainer;

    private List<MarketCard> marketCards;

    public GameObject marketGameObject;


    private MarketCard _selectedMarketCard;
    public MarketCard SelectedMarketCard {
        get {
            return _selectedMarketCard;
        } set {
            _selectedMarketCard = value;
            EntityManager.Instance.OnSelectMarketCard(_selectedMarketCard);
        }
    }

    private void Awake() {
        ResourceManager.OnResourceUpdated += DoOnResourceUpdated;
        GameModeManager.OnGameModeChanged += DoModeChanged;
        EntityManager.OnEPCModeChanged += DoModeChanged;
        marketCards = new List<MarketCard>();
    }

    private void DoModeChanged() {
        if(GameModeManager.Instance.CurrentMode == GameMode.BUILD && EntityManager.Instance.EPCMode == EntityPlacementControllerMode.BUY) {
            if(!marketGameObject.activeSelf) {
                marketGameObject.SetActive(true);
            }
        } else {
            if(marketGameObject.activeSelf) {
                marketGameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy() {
        ResourceManager.OnResourceUpdated -= DoOnResourceUpdated;
    }

    private void Start() {
        DoModeChanged();

        var MarketData = DataManager.Instance.LoadedEntitiesData;
        foreach (var MarketCardData in MarketData)
        {
            if(MarketCardData is BuildingData) {
                var data = MarketCardData as BuildingData;
                var newCard = Instantiate(MarketCardPrefab);
                newCard.transform.SetParent(ListContainer.transform);
                var cardComp = newCard.GetComponent<MarketCard>();
                cardComp.Setup(data);    
                cardComp.parentView = this;
                marketCards.Add(cardComp);
            }
        }
    }

    private void DoOnResourceUpdated(ResourceTypes type) {
        foreach (var marketCard in marketCards)
        {
            marketCard.RefreshAffordable();
        }
    }
}