using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipsManager : MonoBehaviour
{
    public static TooltipsManager Instance;

    private GameObject CurrentTooltip;

    public GameObject ConstructionTooltipPrefab;

    public GameObject ActiveTooltipPrefab;
    
    public GameObject MoveTooltipPrefab;

    public GameObject UICanvas;

    public float yOffset;
    public float xOffset;

    private void Awake() {
        Instance = this;
        GameModeManager.OnGameModeChanged += RemoveActiveTooltip;
    }

    private void OnDestroy() {
        Instance = null;
        GameModeManager.OnGameModeChanged -= RemoveActiveTooltip;
    }

    public void ShowConstructionTooltip(Building building) {
        var tooltipGO = Instantiate(ConstructionTooltipPrefab);
        tooltipGO.transform.SetParent(UICanvas.transform);

        Vector2 localPoint;
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, building.VisualPrefab.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas.GetComponent<RectTransform>(), screenPos, null, out localPoint);
        localPoint.y += yOffset;
        localPoint.x += xOffset;
        tooltipGO.transform.localPosition = localPoint;

        var toolTipComponent = tooltipGO.GetComponent<ConstructionTooltip>();
        toolTipComponent.building = building;
        toolTipComponent.Setup();
    }

    public void RemoveActiveTooltip() {
        if(CurrentTooltip != null) {
            Destroy(CurrentTooltip);
        }
    }

    public void ShowMoveTooltip(Vector3 worldPos) {
        if(CurrentTooltip != null) {
            Destroy(CurrentTooltip);
        }

        CurrentTooltip = Instantiate(MoveTooltipPrefab);
        CurrentTooltip.transform.SetParent(UICanvas.transform);

        Vector2 localPoint;
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas.GetComponent<RectTransform>(), screenPos, null, out localPoint);
        localPoint.y += yOffset;
        localPoint.x += xOffset;
        CurrentTooltip.transform.localPosition = localPoint;

        var toolTipComponent = CurrentTooltip.GetComponent<MoveTooltip>();
        toolTipComponent.Setup();
    }

    public bool IsPointerOverActiveTooltip() {
        bool retVal = false;
        if(CurrentTooltip != null) {
            var comp = CurrentTooltip.GetComponent<BuildingTooltip>();
            retVal = comp.IsPointerOverTooltip();
        }
        return retVal;
    }

    public void ShowActiveTooltip(Building building) {
        Debug.Log("Touched building: " + building.VisualPrefab.name);

        if(building.data.state == BuildingState.CONSTRUCTION) {
            Debug.Log("Building under construction: " + building.VisualPrefab.name);
            return;
        }

        if(CurrentTooltip != null) {
            Destroy(CurrentTooltip);
        }

        CurrentTooltip = Instantiate(ActiveTooltipPrefab);
        CurrentTooltip.transform.SetParent(UICanvas.transform);


        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        localPoint.y += yOffset;
        localPoint.x += xOffset;
        CurrentTooltip.transform.localPosition = localPoint;

        var toolTipComponent = CurrentTooltip.GetComponent<ActiveTooltip>();
        toolTipComponent.building = building;
        toolTipComponent.Setup();
    }
}
