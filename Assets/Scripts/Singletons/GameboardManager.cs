using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardManager : MonoBehaviour
{
    public static GameboardManager Instance;
    
    public Gameboard gameGrid;
    public string gameGridName { get; set; }

    private void Start() {

        //TODO: If time permits find a better way to do this (collision detection)
        gameGridName = gameGrid.gameObject.name;
    }

    public void TouchGameGrid(Vector3 pos) {
        var gridXY = PosToGridXY(pos);

        var entityTouched = EntityManager.Instance.CheckEntityTouched(gridXY);
        if(entityTouched != null) {
            if(GameModeManager.Instance.CurrentMode == GameMode.REGULAR) {
                entityTouched.OnTouch();
            }
            else {
                EntityManager.Instance.InteractWithEntityPlacementController(entityTouched);
            }
        } else {
            if(GameModeManager.Instance.CurrentMode == GameMode.BUILD) {
                if(EntityManager.Instance.IsValidEntityPlaceControllerPosition(gridXY)) {   
                    EntityManager.Instance.MoveEntityPlaceControllerTo(gridXY);
                }
            } else {
                TooltipsManager.Instance.RemoveActiveTooltip();
            }
        }
    }

    public bool IsOutOfBounds(Vector2Int gridXY) {
        bool retVal = false;
        if(gridXY.x < 0 || gridXY.y < 0) {
            retVal = true;
        }
        if(gridXY.x >= gameGrid.GridSize || gridXY.y >= gameGrid.GridSize) {
            retVal = true;
        }
        return retVal;
    }

    float touchToGridConversionConstant = 0f;

    public Vector2Int PosToGridXY(Vector3 pos) {
        Vector2Int retVal = new Vector2Int();
        if(touchToGridConversionConstant == 0f) {
            touchToGridConversionConstant = Gameboard.SQUARE_SIZE * gameGrid.GridSize * 0.5f;
        }
        retVal.x = (int)((((pos.x - touchToGridConversionConstant) / Gameboard.SQUARE_SIZE) + (0.5f * Gameboard.SQUARE_SIZE)) + 1f);      
        retVal.y = (int)((((pos.z - touchToGridConversionConstant) / Gameboard.SQUARE_SIZE) + (0.5f * Gameboard.SQUARE_SIZE)) + 1f) * -1;
        return retVal;
    }

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        Instance = null;
    }
}
