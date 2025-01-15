using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.coinCollect);
        GameManager.instance?.UpdateCoinsCount(coinValue);
        Destroy(gameObject);
    }

    public void SetCoinValue(int value)
    {
        coinValue = value;
    }
}
