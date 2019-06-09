using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Camera.main != null) {
            GroundPositionFromScreenMouseDown(Input.mousePosition);
        }
    }

    private void Awake() {
        Instance = this;    
    }

    private void OnDestroy() {
        Instance = null;    
    }

    void GroundPositionFromScreenMouseDown(Vector3 pos) {
        if(Camera.main != null && !TooltipsManager.Instance.IsPointerOverActiveTooltip()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit rayHit = new RaycastHit();
			if (Physics.Raycast(ray, out rayHit))
			{
                if(rayHit.collider.name == GameboardManager.Instance.gameGridName) {
                    GameboardManager.Instance.TouchGameGrid(rayHit.point);
                }                
			}
        }
    }
}
