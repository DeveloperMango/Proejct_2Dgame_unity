using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public static bool timeOut;

    public Image redBar; // ������ �ٸ� ��Ÿ���� Image ������Ʈ
    public float timeLeft; // �ٰ� �پ��� ��ü �ð� (��: 60��)

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
            timeLeft -= Time.deltaTime; // �ð� ����
            redBar.fillAmount = timeLeft / totalTime; // ���� ���� ����
        }
        else
        {
            timeOut = true;
        }

    }
}