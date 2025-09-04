using UnityEngine;

public delegate void Interaction();



public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteractive;

    public static void OnStartInteraction() => OnInteractive?.Invoke();
}
