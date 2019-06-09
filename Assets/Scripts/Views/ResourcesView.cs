using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesView : MonoBehaviour
{
    public UnityEngine.UI.Text WoodCounter, GoldCounter, SteelCounter;

    private Dictionary<ResourceTypes, UnityEngine.UI.Text> ResourceCounters;

    private void Awake() {
        ResourceCounters = new Dictionary<ResourceTypes, UnityEngine.UI.Text>();
        ResourceCounters.Add(ResourceTypes.WOOD, WoodCounter);
        ResourceCounters.Add(ResourceTypes.GOLD, GoldCounter);
        ResourceCounters.Add(ResourceTypes.STEEL, SteelCounter);

        ResourceManager.OnResourceUpdated += UpdateResourceCount;
    }

    private void OnDestroy() {
        ResourceManager.OnResourceUpdated -= UpdateResourceCount;
    }

    private void Start() {
        UpdateAllResourceCounts();
    }

    public void UpdateAllResourceCounts() {
        if(ResourceManager.Instance != null) {
            foreach (var kvp in ResourceCounters) {
                kvp.Value.text = ResourceManager.Instance.GetResourceCount(kvp.Key) + "";
            }
        } else {
            Debug.LogError("Failed to update resources");
        }
    }

    public void UpdateResourceCount(ResourceTypes resourceType) {
        if(ResourceCounters.ContainsKey(resourceType) && ResourceManager.Instance != null) {
            ResourceCounters[resourceType].text = ResourceManager.Instance.GetResourceCount(resourceType) + "";
        } else {
            Debug.LogError("Failed to update resource counter for resource: " + resourceType.ToString());
        }
    }
}
