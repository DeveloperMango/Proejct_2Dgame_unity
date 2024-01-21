using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public static bool timeOut;

    public Image redBar; // 빨간색 바를 나타내는 Image 컴포넌트
    public float timeLeft; // 바가 줄어드는 전체 시간 (예: 60초)

    private float totalTime; 

    private void Start()
    {
        totalTime = timeLeft;
        timeOut = false;
    }

    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime; // 시간 감소
            redBar.fillAmount = timeLeft / totalTime; // 바의 길이 조정
        }
        else
        {
            timeOut = true;
        }

    }
}