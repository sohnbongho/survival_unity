using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;

    [Header("#Mouse Rotation")]
    [Space(20f)]
    public LayerMask groundLayer;
    public float rotationSpeed = 10.0f;

    private CharacterController controller;
    private Animator animator;
    private P_Finder Finder;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Finder = GetComponent<P_Finder>();

        Delegate_Holder.OnInteraction += () => animator.SetBool("NoneInteraction", true);
        Delegate_Holder.OnInteractionOut += () => animator.SetBool("NoneInteraction", false);
    }
    private void Update()
    {
        if (Finder.OnInteraction)
        {
            if (Input.anyKeyDown)
            {
                Delegate_Holder.OnOutInteraction();
            }
            return;
        }

        Move();
        RotateTowardsMouse();
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

        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("a_Speed", currentSpeed);
    }
    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0.0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
