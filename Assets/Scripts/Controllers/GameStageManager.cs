using UnityEngine;

public enum GameStage
{
    Battle,
    Build,
    GameOver
}

public class GameStageManager : MonoBehaviour
{
    public static GameStageManager Instance { get; private set; }

    public GameStage CurrentGameStage { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetGameStage(GameStage newStage)
    {
        CurrentGameStage = newStage;
    }
}
