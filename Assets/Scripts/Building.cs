using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType {
    DECORATION,
    PRODUCER_AUTOMATIC,
    PRODUCER_ACTIVATIVE
}

public enum BuildingState {
    CONSTRUCTION,
    ACTIVE
}

public class BuildingData : EntityData
{
    public BuildingType type;
    public BuildingState state = BuildingState.CONSTRUCTION;

    public ResourceTypes producerType;

    public int productionQuantity = 10;

    public float productionTime = 10f;

    public float constructionTime = 2f;
    
    public int goldCost;
    public int woodCost;
    public int steelCost;
}

public class Building : Entity {
    public new BuildingData data;

    public float ConstructionTimeSpent = 0f;

    public float ProductionProgressTime = 0f;

    public bool ProducingAtTheMoment;

    public delegate void Action();
    
    public event Action OnBuildingConstructionComplete;
    public void RaiseBuildingConstructionComplete() { if (OnBuildingConstructionComplete != null) OnBuildingConstructionComplete();}

    public event Action OnBuildingProductionComplete;
    public void RaiseBuildingProductionComplete() { if (OnBuildingProductionComplete != null) OnBuildingProductionComplete();}

    public void Setup() {
        if(data.state == BuildingState.CONSTRUCTION) {
            TimeManager.PeriodicUpdate1s += AddPeriodicToConstructionProgress;
            TooltipsManager.Instance.ShowConstructionTooltip(this);
        }
        StartProductionIfPossible();
    }

    public void StartActiveProduction() {
        if(data.state == BuildingState.ACTIVE) {
            if(!ProducingAtTheMoment && data.type == BuildingType.PRODUCER_ACTIVATIVE) {
                ProducingAtTheMoment = true;
                TimeManager.PeriodicUpdate1s += AddPeriodicToProductionProgress;
            }
        }
    }

    public void StartProductionIfPossible() {
        if(data.state == BuildingState.ACTIVE) {
            if(!ProducingAtTheMoment && data.type == BuildingType.PRODUCER_AUTOMATIC) {
                ProducingAtTheMoment = true;
                TimeManager.PeriodicUpdate1s += AddPeriodicToProductionProgress;
            }
        }
    }

    private void AddPeriodicToProductionProgress() {
        ProductionProgressTime += 1f;
        CheckProductionComplete();
    }

    private void CheckProductionComplete() {
        if(ProductionProgressTime >= data.productionTime) {
            ProductionProgressTime = 0f;
            if(data.type != BuildingType.PRODUCER_AUTOMATIC) {
                ProducingAtTheMoment = false;        
                TimeManager.PeriodicUpdate1s -= AddPeriodicToProductionProgress;
            }
            RaiseBuildingProductionComplete();
            ResourceManager.Instance.AddToPlayerResources(data.producerType, data.productionQuantity);
        }
    }

    private void AddPeriodicToConstructionProgress() {
        ConstructionTimeSpent += 1f;
        CheckConstructionComplete();
    }

    private void CheckConstructionComplete() {
        if(ConstructionTimeSpent >= data.constructionTime) {
            data.state = BuildingState.ACTIVE;
            RaiseBuildingConstructionComplete();
            StartProductionIfPossible();
            TimeManager.PeriodicUpdate1s -= AddPeriodicToConstructionProgress;
            Debug.Log("Completed building: " + data.visualPrefab.name);
        }
    }

    private void OnDestroy() {
        TimeManager.PeriodicUpdate1s -= AddPeriodicToConstructionProgress;
        TimeManager.PeriodicUpdate1s -= AddPeriodicToProductionProgress;
    }

    public override void OnTouch() {
        if(GameModeManager.Instance.CurrentMode == GameMode.REGULAR) {
            TooltipsManager.Instance.ShowActiveTooltip(this);
        }
    }
}
