using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTooltip : BuildingTooltip
{
    public UnityEngine.UI.Text nameTextField;

    public UnityEngine.UI.Button startProductionButton;

    public override void Setup() {
        if(building != null) {
            nameTextField.text = building.data.entityName;

            if(building.data.type == BuildingType.DECORATION) {
                progressBar.gameObject.SetActive(false);
                startProductionButton.gameObject.SetActive(false);
            } else if (building.data.type == BuildingType.PRODUCER_AUTOMATIC) {
                StartProductionRefresh();
            } else if (building.data.type == BuildingType.PRODUCER_ACTIVATIVE) {
                if(building.ProducingAtTheMoment) {
                    StartProductionRefresh();
                    building.OnBuildingProductionComplete += OnProductionComplete;
                }
                startProductionButton.onClick.AddListener(() => {
                    if(!building.ProducingAtTheMoment) {
                        building.OnBuildingProductionComplete += OnProductionComplete;
                        building.StartActiveProduction();
                        StartProductionRefresh();
                    }
                });
            }
        }
    }

    private void OnProductionComplete() {
        building.OnBuildingProductionComplete -= OnProductionComplete;
        if(!startProductionButton.gameObject.activeSelf) {
            startProductionButton.gameObject.SetActive(true);
        }
    }

    private void StartProductionRefresh() {
        if(startProductionButton.gameObject.activeSelf) {
            startProductionButton.gameObject.SetActive(false);
        }
        TimeManager.PeriodicUpdate1s += Refresh;
    }

    private void Refresh() {
        progressBar.Refresh(building.ProductionProgressTime / building.data.productionTime);
    }

    private void OnDestroy() {
        TimeManager.PeriodicUpdate1s -= Refresh;
        building.OnBuildingProductionComplete -= OnProductionComplete;
    }
}
