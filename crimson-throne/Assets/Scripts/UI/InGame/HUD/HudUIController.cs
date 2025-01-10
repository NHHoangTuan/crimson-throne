using UnityEngine;

public class HudUIController : MonoBehaviour
{
    public static HudUIController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}