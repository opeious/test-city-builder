using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    public int GridSize = 12;

    public const float SQUARE_SIZE = 10f;

    public Renderer GridRenderer;

    private void Awake() {
        gameObject.transform.localScale = new Vector3(GridSize, 1, GridSize);
        gameObject.transform.localPosition = new Vector3(GridSize * 5, 0, GridSize * -5);
        GridRenderer.sharedMaterial.SetInt("_GridSize", GridSize);
    }
}
