using UnityEngine;
public class PlayerAirborneState : PlayerMovementBaseState
{
    public PlayerAirborneState(PlayerMovementStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
    } 
    public override void Move(MoveData md, float delta)
    {
        base.Move(md, delta);
        IdlingWalking(md);
    }
    protected virtual void IdlingWalking(MoveData md)
    {       
        //if we're grounded, go back to idling or walking state
        if (playerMovementStateMachine.ReusableData.Grounded)
        {          
            if (md.MoveInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
            }
            else
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);
            }
        }       
    }
}
