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

    [SerializeField] BuildManager buildManager;
    [SerializeField] Transform gameOverUI;
    [SerializeField] EnemyController enemyController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    private void Start()
    {
        // Initialize the game stage to Build at the start of the game
        SetGameStage(GameStage.Battle);
    }

    private void UpdateObjectsForCurrentStage()
    {
        switch (CurrentGameStage)
        {
            case GameStage.Battle:
                buildManager.EnableBuildingMode(false);
                enemyController.StartBattleStage();
                break;
            case GameStage.Build:
                buildManager.EnableBuildingMode(true);
                enemyController.StopBattleStage();
                break;
            case GameStage.GameOver:
                if (gameOverUI != null)
                {
                    gameOverUI.gameObject.SetActive(true);
                }
                enemyController.StopBattleStage();
                buildManager.EnableBuildingMode(false);
                break;
        }
    }

    public void SetGameStage(GameStage newStage)
    {
        CurrentGameStage = newStage;
        UpdateObjectsForCurrentStage();
    }
}
