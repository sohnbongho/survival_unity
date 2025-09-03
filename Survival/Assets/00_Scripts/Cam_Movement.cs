using UnityEngine;

public class Cam_Movement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float PosX;
    [SerializeField] private float PosY;
    [SerializeField] private float PosZ;
    [SerializeField] private float m_Speed = 2.0f;

    private void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            player.transform.position.x + PosX,
            player.transform.position.y + PosY,
            player.transform.position.z + PosZ), Time.deltaTime * m_Speed);
    }
}
