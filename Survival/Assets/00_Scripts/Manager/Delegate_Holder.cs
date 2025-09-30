using UnityEngine;

public delegate void Interaction();
public delegate void Stamina(int value);
public delegate void HP(int hp);
public delegate void OnRainIntensityChanged(float intensity);

public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteraction;
    public static event Interaction OnInteractionOut;
    public static event Stamina OnStamina;
    public static event HP OnHP;
    public static event OnRainIntensityChanged RainIntensityChanged;

    public static void ChangeRainIntensity(float intensity)=>RainIntensityChanged?.Invoke(intensity);
    

    public static void OnStaminaChange(int value) => OnStamina?.Invoke(value);
    public static void OnHPChange(int value) => OnHP?.Invoke(value);

    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnOutInteraction() => OnInteractionOut?.Invoke();
}
