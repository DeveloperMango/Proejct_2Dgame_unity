using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class BtnText : MonoBehaviour, IPointerClickHandler
{
    // add callbacks in the inspector like for buttons

    public static bool g_start = false;
    public UnityEvent onClick;

    public GameObject infoObejct;
    public GameObject TimerObejct;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // invoke your event
        onClick.Invoke();
 
        ClearCanvasChildren();

        infoObejct.SetActive(true);
        TimerObejct.SetActive(true);
    }

    // Ŭ�� �� Canvas�� ��� �ڽ� ��Ҹ� ����
    // ���� ��ŸƮ(�г�, ����, ����) ����
    private void ClearCanvasChildren()
    {
        GameObject canvas = transform.parent.gameObject;
        if (canvas != null)
        {
            foreach (Transform child in canvas.transform)
            {
                GameObject.Destroy(child.gameObject);
                g_start = true;
            }
        }
    }
}
