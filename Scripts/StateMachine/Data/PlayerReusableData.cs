using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData
{
    //this is data that needs to be re-used between multiple states
    //we need to use data like this, otherwise the states will reset the variables everytime we change states. Each state would hold their own version of these variables. 
    //don't want to use static variables because we're making a multipayer game, the variables would affect other players
    //not doing this for MoveData and ReconcileData in the StateMachine script may be the problem?
    public Vector2 MoveInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public bool Grounded { get; set; }
    public bool ShouldJump { get; set; }
    public float VerticalVelocity { get; set; }

}
