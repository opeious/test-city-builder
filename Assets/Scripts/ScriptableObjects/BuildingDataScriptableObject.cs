using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingDataScriptableObject : EntityDataScriptableObject
{
    public BuildingType type;

    public BuildingState state;

    public ResourceTypes producerType;
    
    public int productionQuantity = 10;

    public float productionTime = 10f;

    public float constructionTime = 2f;

    public int goldCost;
    public int woodCost;
    public int steelCost;
}
