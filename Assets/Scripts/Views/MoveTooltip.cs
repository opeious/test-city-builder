using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTooltip : BuildingTooltip
{
    public override void Setup() {

    }

    public void OnPressPlace() {
        EntityManager.Instance.PlaceEntityPlacementController();
        TooltipsManager.Instance.RemoveActiveTooltip();
    }

    public void OnPressCancel() {
        EntityManager.Instance.CancelEntityPlacementController();
        TooltipsManager.Instance.RemoveActiveTooltip();
    }
}
