using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController manager = null;
    public SoldierManager soldierMan = new SoldierManager();
    public LiveManager liveManager = new LiveManager();
    public int roomId;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        } else if(manager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soldierMan.Init();
        liveManager.Init();
    }

    private void OnDestroy()
    {
        liveManager.Destroy();
    }
}
