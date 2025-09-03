using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f; // RegidBody에서 처리하던 중력값 처리를 한다.

    private CharacterController controller;
    public void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
    }
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 키보드의 좌우(-1 ~ 1)
        float vertical = Input.GetAxis("Vertical");// 키보드의 상하(-1 ~ 1)

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraRight * horizontal + cameraForward * vertical;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

    }
}
