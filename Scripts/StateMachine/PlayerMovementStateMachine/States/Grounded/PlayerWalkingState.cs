using UnityEngine;
    public class PlayerWalkingState : PlayerGroundedState
{
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        public override void Enter()
        {
            base.Enter();
        //change our speed modifier to movespeedwalk variable stored in playerstate
            playerMovementStateMachine.ReusableData.MovementSpeedModifier = playerMovementStateMachine.PlayerState.moveSpeedWalk;
        }
        public override void Move(MoveData md, float delta)
        {    
            base.Move(md, delta);

        //if we're grounded and not moving, transition to idling state

            if (md.MoveInput == Vector2.zero && playerMovementStateMachine.PlayerState._characterController.isGrounded)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
            }
        }   
}