using UnityEngine;

//This is the base state that all states dervive from through inheritance. It implements the IState interface to make sure it has all the methods required.
public class PlayerMovementBaseState : IState
{
    protected PlayerMovementStateMachine playerMovementStateMachine;
    public PlayerMovementBaseState(PlayerMovementStateMachine playerMoveStateMachine)
    {
        playerMovementStateMachine = playerMoveStateMachine;
    }

    public virtual void Enter()
    {
        Debug.Log("Current State is: " + GetType().Name);

    }
    public virtual void Exit()
    {

    }
    public void Reconcile(out ReconcileData rd)
    {
        //this is called in PlayerState OnTick. It doesn't have to be stored here, but I liked having most of the methods here. Doesn't matter where this is stored, only where it's called. 
        rd = new ReconcileData(playerMovementStateMachine.PlayerState.transform.position, playerMovementStateMachine.ReusableData.VerticalVelocity);
    }

    //reconcile the position and vertial velocity, rotation doesn't matter right now as we're not turning. We have to reconcile vertical velocity or the jump jitters. 
    public void Reconciliation(ReconcileData rd)
    {
        playerMovementStateMachine.PlayerState.transform.position = rd.Position;

        playerMovementStateMachine.ReusableData.VerticalVelocity = rd.VerticalVelocity;
    }

    public virtual void Move(MoveData md, float delta)
    {
        //store the WASD movement into a Vector3 variable
        Vector3 movement = new Vector3(md.MoveInput.x, 0f, md.MoveInput.y).normalized;

        //change the x and z movement by the speed modifier

        movement *= playerMovementStateMachine.ReusableData.MovementSpeedModifier;

        //jump if grounded and pressing spacebar, this works fine here, but when I split this into checking in grounded and applying vertical velocity in jumping, it jitters

        //if (md.IsJumpInputPressed && playerMovementStateMachine.ReusableData.Grounded)
        //{
        //    playerMovementStateMachine.ReusableData.VerticalVelocity = 4f;
        //}

        //apply gravity
        playerMovementStateMachine.ReusableData.VerticalVelocity -= (10f * delta);
        //don't let gravity go past -20f
        playerMovementStateMachine.ReusableData.VerticalVelocity = Mathf.Max(-20f, playerMovementStateMachine.ReusableData.VerticalVelocity);

        //apply movement on y axis, this is affected by (1) gravity pushing down and (2) jumping up
        movement += new Vector3(0f, playerMovementStateMachine.ReusableData.VerticalVelocity, 0f);

        playerMovementStateMachine.PlayerState._characterController.Move(movement * delta);
    }

    /// <summary>
    /// A simple method to get input. This doesn't have any relation to the prediction.
    /// </summary>

    public void CheckInput(out MoveData md)
    {
        //done on the client and sent to the server
        md = new MoveData()
        {
            MoveInput = playerMovementStateMachine.ReusableData.MoveInput,
            IsJumpInputPressed = playerMovementStateMachine.ReusableData.ShouldJump,
        };
        //need to turn jump off
        playerMovementStateMachine.ReusableData.ShouldJump = false;

    }

}