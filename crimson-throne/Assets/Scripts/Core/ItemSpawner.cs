using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    #region Singleton
    public static ItemSpawner instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private GameObject expItem;
    [SerializeField] private GameObject coinItem;
    [SerializeField] private GameObject healthItem;
    [SerializeField] private GameObject keyItem;
    [SerializeField] private GameObject gateItem;
    #endregion

    #region Spawn Controls
    public void SpawnExp(float value, Vector2 position)
    {
        if (ValidatePrefab(expItem))
        {
            GameObject item = SpawnItem(expItem, position);
            if (value >= 10)
            {
                item.GetComponent<SpriteRenderer>().color = Color.red;
            }
            item.GetComponent<ExpItem>().SetExpValue(value);
        }
    }

    public void SpawnCoin(Vector2 position)
    {
        if (ValidatePrefab(coinItem))
        {
            SpawnItem(coinItem, position);
        }
    }

    public void SpawnHealthItem(Vector2 position)
    {
        if (ValidatePrefab(healthItem))
        {
            SpawnItem(healthItem, position);
        }
    }

    public void SpawnKey(Vector2 position)
    {
        if (ValidatePrefab(keyItem))
        {
            SpawnItem(keyItem, position);
        }
    }

    public void SpawnGate(Vector2 position)
    {
        if (ValidatePrefab(gateItem))
        {
            SpawnItem(gateItem, position);
        }
    }
    #endregion

    #region Utility Methods
    private bool ValidatePrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Attempted to spawn an item, but the prefab is null.");
            return false;
        }
        return true;
    }

    private GameObject SpawnItem(GameObject prefab, Vector2 position)
    {
        return Instantiate(prefab, position, Quaternion.identity);
    }
    #endregion
}