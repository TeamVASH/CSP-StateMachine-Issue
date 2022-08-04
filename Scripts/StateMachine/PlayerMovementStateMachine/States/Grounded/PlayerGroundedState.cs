using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerMovementBaseState
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }  

    public override void Move(MoveData md, float delta)
    {
        //calling this before move seems to have less jitter?
        Jumping(md);
        base.Move(md, delta);
    }   
    protected void Jumping(MoveData md)
    {
        //if we're grounded and pressing spacebar, move to jumping state. ChangeState method calls Exit on the current state, then changes the state name, then calls Enter on the new state. See more info in the StateMachine script. 
        if (md.IsJumpInputPressed && playerMovementStateMachine.ReusableData.Grounded)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.JumpingState);
        }       
    }
}