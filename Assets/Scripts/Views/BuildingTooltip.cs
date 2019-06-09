using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingTooltip : MonoBehaviour
{
    public Building building;

    public ProgressBar progressBar;

    public virtual void Setup() {

    }

    public bool IsPointerOverTooltip() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
