using UnityEngine;

public class BonFire : M_Object
{
    public override void Interaction()
    {
        base.Interaction();
        P_Movement.instance.AnimationChange("Sitting");
    }
}
