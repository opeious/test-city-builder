using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    
    public List<BuildingDataScriptableObject> BuildingsData;

    //Remove this jsut for testing
    public List<EntityData> startupEntityData;

    private List<EntityData> LoadedEntitiesData;

    public GameObject benchPrefab;

    private void Awake() {
        Instance = this;

        LoadedEntitiesData = new List<EntityData>();
        foreach (var singleBuildingData in BuildingsData)
        {
            var tempData = new BuildingData();
            
            tempData.entityName = singleBuildingData.name;
            tempData.footprint = singleBuildingData.footprint;
            tempData.visualPrefab = singleBuildingData.visualPrefab;

            tempData.type = singleBuildingData.type;
            tempData.state = singleBuildingData.state;
            tempData.producerType = singleBuildingData.producerType;
            tempData.productionQuantity = singleBuildingData.productionQuantity;
            tempData.productionTime = singleBuildingData.productionTime;
            tempData.constructionTime = singleBuildingData.constructionTime;

            LoadedEntitiesData.Add(tempData);
        }

        //Fake Setup TODO: if time permits auto-save and auto-load this
        startupEntityData = new List<EntityData>();
        startupEntityData.Add(LoadedEntitiesData[1]);
    }

    private void OnDestroy() {
        Instance = null;
    }
}
