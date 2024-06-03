using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;
    private float elpasedTime;

    void Update()
    {
        elpasedTime += Time.deltaTime;
        UpdateTimerText();
    }

    void UpdateTimerText()
    { 
        int minutes = Mathf.FloorToInt(elpasedTime / 60F);
        int seconds = Mathf.FloorToInt(elpasedTime % 60);
        TimerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
}
