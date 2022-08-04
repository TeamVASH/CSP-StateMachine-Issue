using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet;
using UnityEngine;

public class PlayerState : NetworkBehaviour
{
    private MoveData _clientMoveData;

    [SerializeField]
    public float moveSpeedWalk = 1f;

    [SerializeField]
    private LayerMask groundLayer;

    [HideInInspector]
    public CharacterController _characterController;

    private PlayerMovementStateMachine movementStateMachine;

    private void Awake()
    {
        /* Prediction is tick based so you must
        * send datas during ticks. You can use whichever
        * tick best fits your need, such as PreTick, Tick, or PostTick.
        * In most cases you will send/move using Tick. For rigidbodies
        * you will send using PostTick. I subscribe to ticks using
        * the InstanceFinder class, which finds the first NetworkManager
        * loaded. If you are using several NetworkManagers you would want
        * to subscrube in OnStartServer/Client using base.TimeManager. */

        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;

        //Using OnUpdate for the client helps with smoothness, especially for animations
        InstanceFinder.TimeManager.OnUpdate += TimeManager_OnUpdate;

        _characterController = GetComponent<CharacterController>();

        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void OnDestroy()
    {
        //Unsubscribe as well.

        if (InstanceFinder.TimeManager != null)
        {
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            InstanceFinder.TimeManager.OnUpdate -= TimeManager_OnUpdate;
        }
    }

    //Idling state is the first state
    private void Start()
    {
        movementStateMachine.Initialize(movementStateMachine.IdlingState);
    }


    public void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
            /* Call reconcile using default, and false for
             * asServer. This will reset the client to the latest
             * values from server and replay cached inputs. */

            Reconciliation(default, false);

            /* CheckInput builds MoveData from user input. When there
             * is no input CheckInput returns default. You can handle this
            * however you like but Move should be called when default if
            * there is no input which needs to be sent to the server. */

            movementStateMachine.CheckInput(out MoveData md);

            /* Move using the input, and false for asServer.
            * Inputs are automatically sent with redundancy. How many past
            * inputs will be configurable at a later time.
            * When a default value is used the most recent past inputs
            * are sent a predetermined amount of times. It's important you
            * call Move whether your data is default or not. FishNet will
            * automatically determine how to send the data, and run the logic. */

            Move(md, false);
        }

        if (base.IsServer)
        {
            /* Move using default data with true for asServer.
            * The server will use stored data from the client automatically.
            * You may also run any sanity checks on the input as demonstrated
            * in the method. */

            Move(default, true);

            /* After the server has processed input you will want to send
            * the result back to clients. You are welcome to skip
            * a few sends if you like, eg only send every few ticks.
            * Generate data required on how the client will reset and send it by calling your Reconcile
            * method with the data, again using true for asServer. Like the
            * Replicate method (Move) this will send with redundancy a certain
            * amount of times. If there is no input to process from the client this
            * will not continue to send data. */

            movementStateMachine.Reconcile(out ReconcileData rd);
            Reconciliation(rd, true);
        }
    }

    private void TimeManager_OnUpdate()
    {
        if (base.IsOwner)
        {
            movementStateMachine.ReusableData.Grounded = GroundedCheck();
            Debug.Log(movementStateMachine.ReusableData.Grounded);
            Debug.DrawRay(transform.position + new Vector3(0, _characterController.height / 2, 0), Vector3.down, Color.red);
            movementStateMachine.Move(_clientMoveData, Time.deltaTime);
        }
    }

    //A very basic ground check, I'm using a better one outside this example, but this check does the trick. If I use a better ground check, the problem still persists with the state machine, so ground check is not the issue. 
    protected bool GroundedCheck()
    {
        float distanceToGround = _characterController.height / 2;
        bool IsGrounded = Physics.Raycast(transform.position + new Vector3(0, distanceToGround, 0), Vector3.down, distanceToGround + 0.1f);
        return IsGrounded;
    }

    //check for input in update so it's not missed

    private void Update()
    {
        if (!base.IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementStateMachine.ReusableData.ShouldJump = true;
        }

        movementStateMachine.ReusableData.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    /// <summary>
    /// Replicate attribute indicates the data is being sent from the client to the server.
    /// When Replicate is present data is automatically sent with redundancy.
    /// The replay parameter becomes true automatically when client inputs are
    /// being replayed after a reconcile. This is useful for a variety of things,
    /// such as if you only want to show effects the first time input is run you will
    /// do so when replaying is false.
    /// </summary>
    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        /* You can check if being run as server to
        * add security checks such as normalizing
        * the inputs. */
    
        /* You may also use replaying to know
         * if a client is replaying inputs rather
         * than running them for the first time. This can
         * be useful because you may only want to run
         * VFX during the first input and not during
         * replayed inputs. */

        if (asServer || replaying)
        {
            movementStateMachine.ReusableData.Grounded = GroundedCheck();
            movementStateMachine.Move(md, (float)base.TimeManager.TickDelta);

        }
        else if (!asServer)
        {
            _clientMoveData = md;
        }
    }

    /// <summary>
    /// A Reconcile attribute indicates the client will reconcile
    /// using the data and logic within the method. When asServer
    /// is true the data is sent to the client with redundancy,
    /// and the server will not run the logic.
    /// When asServer is false the client will reset using the logic
    /// you supply then replay their inputs.
    /// </summary>

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        movementStateMachine.Reconciliation(rd);
    }
}