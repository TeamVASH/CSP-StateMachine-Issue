using UnityEngine;

/// <summary>
/// Data on how to move.
/// This is processed locally as well sent to the server for processing.
/// Any inputs or values which may affect your move should be placed in your own MoveData.
/// The structure type may be named anything. Classes can also be used but will generate garbage, so structures
/// are recommended.
/// </summary>
public struct MoveData
{
    public Vector2 MoveInput;
    public bool IsJumpInputPressed;
}

/// <summary>
/// Data on how to reconcile.
/// Server sends this back to the client. Once the client receives this they
/// will reset their object using this information. Like with MoveData anything that may
/// affect your movement should be reset. Since this is just a transform only position and
/// rotation would be reset. But a rigidbody would include velocities as well. If you are using
/// an asset it's important to know what systems in that asset affect movement and need
/// to be reset as well.
/// </summary>
/// 
public struct ReconcileData
{
    public Vector3 Position;
    public float VerticalVelocity;
    public ReconcileData(Vector3 position, float verticalVelocity)
    {
        Position = position;
        VerticalVelocity = verticalVelocity;
    }
}
public abstract class StateMachine
    {
    // using an interface as a datatype: it means the data type can reference any object of a class implementing the interface 
    //You are not creating an instance of the interface - you are creating an instance of something that implements the interface.
    //if you have a variable of type IMyInterface someVar then that basically ... can be switched out for any type that implements IMyInterface because that concrete type is a IMyInterface
    //This allows you to write code that will apply to a variety of classes that have common operations that can be performed on them.
    // you use interface variables for the parameter in the method used ChangeState - the parameter can take any that implements the variable
    //we don't know at compile time whether we're gonna get idle, running state etc and we don't care, all we care about is that we can pass it in
    //Dependency injection via constructor injection and interface 
    //3 types of DI: Constructor, property and method // we're using interface method injection

    //Holds the current state. Every state machine we make will inherit from this abstract class (e.g playercombatstatemachine),
    //so we don't want to call the variable "currentPlayerState" but "currentState" of the context we're getting it from.

        public IState currentState;

    //initialize the first state, called in Start() in PlayerState
    public void Initialize(IState newState)
        {
            currentState = newState;
            currentState?.Enter();
        }

        //lets us change states
    public void ChangeState(IState newState)
        {
            currentState?.Exit(); //resets any data that needs to be reset before changing states // ? menas if this is not null, then continue - it won't be null because we will call Initialize(), I took off the ? as it won't be null
            currentState = newState; //set current state to new state that was passed in the method as a parameter from the concrete state
            currentState?.Enter(); //sets any data that needs to be set in the new state
        }

    //Here we access the state's methods with currentState. - the overall method is called in the PlayerState script
    public void CheckInput(out MoveData md)
        {
            md = default;
            currentState?.CheckInput(out md);
        }
    public void Move(MoveData md, float delta)
        {
            currentState?.Move(md, delta);
        }

    public void Reconcile(out ReconcileData rd)
        {
            rd = default;
            currentState?.Reconcile(out rd);
        }
    public void Reconciliation(ReconcileData rd)
        {
            currentState?.Reconciliation(rd);
        }
}

