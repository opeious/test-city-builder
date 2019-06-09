using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeView : MonoBehaviour
{
    public Button ReuglarModeButton;

    public Button BuildModeButton;


    private void Awake() {
        GameModeManager.OnGameModeChanged += RefreshButtonStates;
    }

    private void OnDestroy() {
        GameModeManager.OnGameModeChanged -= RefreshButtonStates;
    }

    public void Start() {
        RefreshButtonStates();
    }
    
    public void OnRegularModePressed() {
        GameModeManager.Instance.SetGameMode(GameMode.REGULAR);
    }

    public void OnBuildModePressed() {
        GameModeManager.Instance.SetGameMode(GameMode.BUILD);
    }

    public void RefreshButtonStates() {
        if(GameModeManager.Instance != null) {
            var gmManager = GameModeManager.Instance;
            
            if(gmManager.CurrentMode == GameMode.REGULAR) {
                if(null != ReuglarModeButton) {
                    ReuglarModeButton.interactable = false;
                    BuildModeButton.interactable = true;
                }
            } else {
                if(null != BuildModeButton) {
                    ReuglarModeButton.interactable = true;
                    BuildModeButton.interactable = false;
                }
            }


        }
    }
}
