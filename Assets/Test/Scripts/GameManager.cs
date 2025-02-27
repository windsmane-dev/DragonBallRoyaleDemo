using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public UnitFactory UnitFactory { get; private set; }

    [SerializeField] private UnitDatabase unitDatabase;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject managerObj = new GameObject("GameManager");
                instance = managerObj.AddComponent<GameManager>();
                DontDestroyOnLoad(managerObj);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        SpawnManager spawnManager = CreateManager<SpawnManager>("SpawnManager");
        EnergySystem player1Energy = CreateManager<EnergySystem>("Player1Energy");
        player1Energy.Initialize(1);

        EnergySystem player2Energy = CreateManager<EnergySystem>("Player2Energy");
        player2Energy.Initialize(2);

        TurnManager turnManager = CreateManager<TurnManager>("TurnManager");
        ScoreManager scoreManager = CreateManager<ScoreManager>("ScoreManager");


        if (unitDatabase != null)
        {
            UnitFactory = new UnitFactory(unitDatabase.GetUnitDictionary(), unitDatabase.GetUnitDataDictionary(), 10);
        }
        else
        {
            Debug.LogError("Unit Database is missing in GameManager!");
        }

        spawnManager.Initialize(player1Energy, player2Energy, unitDatabase.ballPrefab);

        EventHolder.TriggerGameStart();
    }

    private T CreateManager<T>(string name) where T : Component
    {
        GameObject obj = new GameObject(name);
        T manager = obj.AddComponent<T>();
        DontDestroyOnLoad(obj);
        return manager;
    }

    private void OnDestroy()
    {
        if (UnitFactory != null)
        {
            UnitFactory.Destroyed();
        }
    }
}
