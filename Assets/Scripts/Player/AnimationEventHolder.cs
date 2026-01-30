using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHolder : MonoBehaviour
{
    public ThirdPersonMovement MovementScript;

    public void Anim_SwordToBackPlacesTarget()
    {
        MovementScript.Anim_SwordToBackPlacesTarget();
    }

    public void Anim_SwordToBackHandTarget()
    {
        MovementScript.Anim_SwordToBackHandTarget();
    }
}
