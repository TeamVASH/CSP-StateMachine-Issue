using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerMovementStateMachine : StateMachine
    {        
        //we're going to be in these states a lot so instead of instantiating them often at runtime, we cache them instead
    
        //only need to cache states we'll actually be in, not group states like grounded or airborne, groups states only exist to group common logic

        //get; only properties are settable from the constructor, everywhere else it is read only // otherwise private set; can be set from everywhere inside that class
        //make it private for all properties that don't use a public set
        //C# 6.0 adds readonly auto properties - private readonly is automatically created behind scenes // can assign default value here too for a constructor with no parameters
        //cannot assign this variable to anything, can only make it a default or assign it in the constructor
        //we pass in this script into the state's constructors so we have access to all these states, but they can't be changed to = anything else becausew they're read only. 

        public PlayerState PlayerState { get; }
        public PlayerStateReusableData ReusableData { get; }
        public PlayerIdlingState IdlingState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerJumpingState JumpingState { get; }     

        // using { get; } means these concrete states can only be initialized with an inliner or with the playermovementstatemachine's constructor, no where else
        public PlayerMovementStateMachine(PlayerState playerState)
        {
            //the states needs access to the player state so they can get the character controller etc.

            PlayerState = playerState;

        //we need to use ReusableData for changing variables, otherwise when you enter new states, the variables are reset/only change within the state, this maybe the issue with MoveData and ReconcileData?

            ReusableData = new PlayerStateReusableData();

            IdlingState = new PlayerIdlingState(this);
           
            WalkingState = new PlayerWalkingState(this);       

            JumpingState = new PlayerJumpingState(this);           

        }
    }

