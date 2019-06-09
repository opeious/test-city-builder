using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    
    public List<BuildingDataScriptableObject> BuildingsData;

    public List<EntityData> LoadedEntitiesData;

    private void Awake() {
        Instance = this;

        LoadedEntitiesData = new List<EntityData>();
        foreach (var singleBuildingData in BuildingsData)
        {
            var tempData = new BuildingData();
            
            tempData.entityName = singleBuildingData.entityName;
            tempData.footprint = singleBuildingData.footprint;
            tempData.visualPrefab = singleBuildingData.visualPrefab;

            tempData.type = singleBuildingData.type;
            tempData.state = singleBuildingData.state;
            tempData.producerType = singleBuildingData.producerType;
            tempData.productionQuantity = singleBuildingData.productionQuantity;
            tempData.productionTime = singleBuildingData.productionTime;
            tempData.constructionTime = singleBuildingData.constructionTime;
            tempData.goldCost = singleBuildingData.goldCost;
            tempData.woodCost = singleBuildingData.woodCost;
            tempData.steelCost = singleBuildingData.steelCost;

            LoadedEntitiesData.Add(tempData);
        }
    }

    private void OnDestroy() {
        Instance = null;
    }
}
