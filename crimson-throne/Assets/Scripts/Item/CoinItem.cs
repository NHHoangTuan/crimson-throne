using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        GameManager.instance.UpdateCoinsCount(coinValue);
        Destroy(gameObject);
    }

    public void SetCoinValue(int value)
    {
        coinValue = value;
    }
}
