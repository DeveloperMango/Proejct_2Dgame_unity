using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform weaponTransform;
    private SpriteRenderer spriter;
    private bool isSwinging = false;
    private float swingAngle = 45f;
    private float swingDuration = 0.15f; // 스윙하는데 걸리는 총 시간

    SpriteRenderer player;

    Quaternion leftRot = Quaternion.Euler(0, 0, 26);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, 154);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
        spriter = weaponTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingWeapon());
        }
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;
        if (!isSwinging)
        {
            weaponTransform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 7 : 8;
        }
    }

    IEnumerator SwingWeapon()
    {
        isSwinging = true;

        bool isReverse = player.flipX;
        Quaternion startRotation = weaponTransform.localRotation;
        Quaternion endRotation;

        if (isReverse)
        {
            endRotation = Quaternion.Euler(0, 0, startRotation.eulerAngles.z + swingAngle);
        }
        else
        {
            endRotation = Quaternion.Euler(0, 0, startRotation.eulerAngles.z - swingAngle);
        }

        float elapsedTime = 0f;

        while (elapsedTime < swingDuration)
        {
            weaponTransform.localRotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponTransform.localRotation = endRotation;
        isSwinging = false;
    }
}
