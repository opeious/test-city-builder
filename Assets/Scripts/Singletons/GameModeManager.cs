using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
    REGULAR,
    BUILD
}

public class GameModeManager : MonoBehaviour
{
    public delegate void Action();
    public static event Action OnGameModeChanged;
    public static void RaiseGameModeChanged() { if (OnGameModeChanged != null) OnGameModeChanged();}
    

    public static GameModeManager Instance;

    private GameMode _currentMode = GameMode.BUILD;

    public GameMode CurrentMode {
        get {
            return _currentMode;
        }
        set {
            _currentMode = value;
            RaiseGameModeChanged();
        }
    }
    


    public void SetGameMode(GameMode val) {
        CurrentMode = val;
    }

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        Instance = null;
    }


}
