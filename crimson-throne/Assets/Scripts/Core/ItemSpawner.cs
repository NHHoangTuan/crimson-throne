using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance { get; private set; }
    [SerializeField] private GameObject expItem;
    [SerializeField] private GameObject coinItem;
    [SerializeField] private GameObject healthItem;

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
        if (value >= 3)
        {
            item.GetComponent<SpriteRenderer>().color = Color.red;
        }
        item.GetComponent<ExpItem>().SetExpValue(value);
    }
}
