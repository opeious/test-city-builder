using UnityEngine;

public class ConstructionTooltip : BuildingTooltip {

    public override void Setup() {
        if(building != null) {
            if(building.data.state == BuildingState.CONSTRUCTION) {
                TimeManager.PeriodicUpdate1s += Refresh;
                building.OnBuildingConstructionComplete += OnBuildingConstructed;
            }
        }
    }

    public void OnBuildingConstructed() {
        Destroy(gameObject);
    }
    
    private void OnDestroy() {
        building.OnBuildingConstructionComplete -= OnBuildingConstructed;
        TimeManager.PeriodicUpdate1s -= Refresh;
    }

    public void Refresh() {
        progressBar.Refresh(building.ConstructionTimeSpent / building.data.constructionTime);
    }
}