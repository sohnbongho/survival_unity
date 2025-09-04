using UnityEngine;

public class Canvas_Holder : MonoBehaviour
{
    [SerializeField] private GameObject Board;

    private void Start()
    {
        Delegate_Holder.OnInteractive += GetBoard;
    }

    public void GetBoard()
    {
        Board.SetActive(true);
    }
}
