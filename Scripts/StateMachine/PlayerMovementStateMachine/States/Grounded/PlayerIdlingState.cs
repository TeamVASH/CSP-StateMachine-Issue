using UnityEngine;
    public class PlayerIdlingState : PlayerGroundedState
{
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        public override void Enter()
        {
            base.Enter();
            playerMovementStateMachine.ReusableData.MovementSpeedModifier = 0;
        }
    public override void Move(MoveData md, float delta)
    {
        base.Move(md, delta);

        //if we're grounded and moving, go to walking state

        if (md.MoveInput != Vector2.zero && playerMovementStateMachine.PlayerState._characterController.isGrounded)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);
        }
    }    
}