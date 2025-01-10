using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance { get; private set; }
    [SerializeField] private GameObject expItem;
    [SerializeField] private GameObject coinItem;
    [SerializeField] private GameObject healthItem;
    [SerializeField] private GameObject keyItem;
    [SerializeField] private GameObject gateItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnExp(float value, Vector2 position)
    {
        GameObject item = Instantiate(expItem, position, Quaternion.identity);
        if (value >= 5)
        {
            item.GetComponent<SpriteRenderer>().color = Color.red;
        }
        item.GetComponent<ExpItem>().SetExpValue(value);
    }

    public void SpawnCoin(Vector2 position)
    {
        Instantiate(coinItem, position, Quaternion.identity);
    }

    public void SpawnHealthItem(Vector2 position)
    {
        Instantiate(healthItem, position, Quaternion.identity);
    }

    public void SpawnKey(Vector2 position)
    {
        Instantiate(keyItem, position, Quaternion.identity);
    }

    public void SpawnGate(Vector2 position)
    {
        Instantiate(gateItem, position, Quaternion.identity);
    }
}
