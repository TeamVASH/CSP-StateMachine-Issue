using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //set our vertical velocit to 4 so we move the player up
        playerMovementStateMachine.ReusableData.VerticalVelocity = 4;    
    }
}
