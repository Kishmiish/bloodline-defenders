using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killCounterText;
    private int killCounter = 0;
    void Awake()
    {
        killCounterText.text = ": " + killCounter.ToString();
    }

    public void Kill()
    {
        killCounter++;
        killCounterText.text = ": " + killCounter.ToString();
    }

    public int GetKillCount()
    {
        return killCounter;
    }
}
