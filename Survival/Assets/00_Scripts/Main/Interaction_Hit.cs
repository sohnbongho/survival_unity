using System.Collections;
using UnityEngine;

public class Interaction_Hit : M_Object
{
    float shakeAmount = 5.0f;
    float shakeDuration = 0.5f;

    private Quaternion orginalRotation;

    private void Start()
    {
        orginalRotation = transform.rotation;
        HP = m_Data.HP;
    }

    public override void Interaction()
    {
        P_Movement.instance.AnimationChange(m_Data.m_Type.ToString());
        base.Interaction();
    }

    public override void OnHit()
    {
        base.OnHit();
        ShakeTree(transform.position - P_Movement.instance.transform.position);

        if (HP <= 0)
        {
            var items = ItemFlowController.DROPITEMLIST(m_Data.Drop_Items);

            for (int i = 0; i < items.Count; i++)
            {
                Instantiate(Item_Prefab, transform.position, Quaternion.identity);
                Debug.Log(items[i].ItemID + " : " + items[i].ItemName + " : " + items[i].Description);
            }

        }
    }

    private void ShakeTree(Vector3 attackDirection)
    {
        Vector3 oppositeDirection = attackDirection.normalized;

        // Vector3의 정확한 값을 가져오기 위해 eulerAngles를 사용한다.
        Quaternion targetRotation = Quaternion.Euler(
            orginalRotation.eulerAngles.x + oppositeDirection.z * shakeAmount,
            orginalRotation.eulerAngles.y,
            orginalRotation.eulerAngles.z + oppositeDirection.x * shakeAmount);

        StopAllCoroutines();
        StartCoroutine(ShakeAnimation(targetRotation));
    }

    private IEnumerator ShakeAnimation(Quaternion targetRotation)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(orginalRotation,
                targetRotation,
                elapsedTime / (shakeDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 돌아오는 부분
        elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(targetRotation,
                orginalRotation,
                elapsedTime / (shakeDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
