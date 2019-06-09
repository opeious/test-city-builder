using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public delegate void ActionResourceType(ResourceTypes resourceType);
    public static event ActionResourceType OnResourceUpdated;
    public static void RaiseResourceUpdated(ResourceTypes resourceType) { if (OnResourceUpdated != null) OnResourceUpdated(resourceType);}
    
    public static ResourceManager Instance;

    //resource name, resource reference
    Dictionary <ResourceTypes, Resource> PlayerResources;

    private void Awake() {
        PlayerResources = new Dictionary<ResourceTypes, Resource>();
        foreach(var resourceType in System.Enum.GetValues(typeof(ResourceTypes))) {
            PlayerResources.Add((ResourceTypes)resourceType, new Resource());

        }

        Instance = this;
    }

    public bool CanSpendResource(ResourceTypes checkType, int qty) {
        bool retVal = false;
        if(PlayerResources.ContainsKey(checkType)) {
            retVal = PlayerResources[checkType].CanSpendResource(qty);
        }
        return retVal;
    }

    public void AddToPlayerResources(ResourceTypes addTo, int qty) {
        if(PlayerResources.ContainsKey(addTo)) {
            PlayerResources[addTo].AddResource(qty);
            RaiseResourceUpdated(addTo);
        } else {
            Debug.LogError("Could not find resource: " + addTo.ToString() + " in player resources.");
        }
    }

    public int GetResourceCount(ResourceTypes getType) {
        int retVal = -1;
        if(PlayerResources.ContainsKey(getType)) {
            retVal = PlayerResources[getType].GetCount();
        }
        if(retVal == -1) {
            Debug.LogError("Could not find resource: " + getType.ToString() + " in player resources.");
        }
        return retVal;
    }

    public void RemoveFromPlayerResources(ResourceTypes removeFrom, int qty) {
        if(PlayerResources.ContainsKey(removeFrom)) {
            PlayerResources[removeFrom].SpendResource(qty);
            RaiseResourceUpdated(removeFrom);
        } else {
            Debug.LogError("Could not find resource: " + removeFrom.ToString() + " in player resources.");
        }
    }

    private void OnDestroy() {
        Instance = null;
    }
}
