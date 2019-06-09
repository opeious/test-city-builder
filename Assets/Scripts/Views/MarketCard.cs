using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MarketCard : MonoBehaviour {
    
    public Text EntityName;

    public Text GoldCost;
    public Text WoodCost;
    public Text SteelCost;
    public Text WoodProduce;
    public Text GoldProduce;
    public Text SteelProduce;
    public Text BuildTime;
    public Text ProductionTime;
    public Text ProductionGoldQty;
    public Text ProductionSteelQty;
    public Text ProductionWoodQty;

    public GameObject ProductionContainer;
    public GameObject ProductionTimeContainer;
    public GameObject ProductionAutomaticProducerImage;

    public MarketView parentView;
    
    private Dictionary<ResourceTypes, int> ResourceCosts;

    public EntityData CardEntityData;

    public void RefreshAffordable() {
        bool affordable = true;
        if(ResourceCosts != null && ResourceCosts.Count > 0) {
            foreach (var resourceCost in ResourceCosts)
            {
                if(!ResourceManager.Instance.CanSpendResource(resourceCost.Key, resourceCost.Value))
                {
                    affordable = false;
                    break;
                }
            }
        }
        
        var buttonComp = gameObject.GetComponent<Button>();
        buttonComp.interactable = affordable;
    }

    public void OnSelectThisCard() {
        parentView.SelectedMarketCard = this;
    }

    public void Setup(EntityData entityData) {
        CardEntityData = entityData;
        ResourceCosts = new Dictionary<ResourceTypes, int>();
        
        if(entityData is BuildingData) {
            var data = entityData as BuildingData;

            EntityName.text = data.entityName;

            ResourceCosts.Add(ResourceTypes.GOLD, data.goldCost);
            ResourceCosts.Add(ResourceTypes.WOOD, data.woodCost);
            ResourceCosts.Add(ResourceTypes.STEEL, data.steelCost);

            GoldCost.text = data.goldCost + "";
            WoodCost.text = data.woodCost + "";
            SteelCost.text = data.steelCost + "";

            BuildTime.text = data.constructionTime + "";

            GoldProduce.text = 0 + "";
            WoodProduce.text = 0 + "";
            SteelProduce.text = 0 + "";

            if(data.type == BuildingType.DECORATION) {
                ProductionContainer.SetActive(false);
                ProductionTimeContainer.SetActive(false);
            } else {
                ProductionTime.text = data.productionTime + "";
                if(data.type == BuildingType.PRODUCER_ACTIVATIVE) {
                    ProductionAutomaticProducerImage.SetActive(false);
                } else if (data.type == BuildingType.PRODUCER_AUTOMATIC) {

                }

                if(data.producerType == ResourceTypes.GOLD) {
                    GoldProduce.text = data.productionQuantity + "";
                } else if(data.producerType == ResourceTypes.STEEL) {
                    SteelProduce.text = data.productionQuantity + "";
                } else if(data.producerType == ResourceTypes.WOOD) {
                    WoodProduce.text = data.productionQuantity + "";
                }
            }
            RefreshAffordable();
        }
    }
}