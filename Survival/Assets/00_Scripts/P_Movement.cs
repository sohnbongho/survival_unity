using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f; // RegidBody���� ó���ϴ� �߷°� ó���� �Ѵ�.

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
        float horizontal = Input.GetAxis("Horizontal"); // Ű������ �¿�(-1 ~ 1)
        float vertical = Input.GetAxis("Vertical");// Ű������ ����(-1 ~ 1)

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
