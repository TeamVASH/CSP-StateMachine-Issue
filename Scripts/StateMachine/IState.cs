
    //Any script that implements this interface must include these methods. e.g PlayerMovementBaseState and PlayerCombatBaseState will implement this - we don't have a body with these methods, only abstract classes do
    public interface IState
    {
        //Runs whenever a state becomes the current state
        public void Enter();

        //Runs whenever a state becomes the previous state
        public void Exit();

        public void CheckInput(out MoveData md);

        public void Move(MoveData md, float delta);

        public void Reconcile(out ReconcileData rd);

        public void Reconciliation(ReconcileData rd);

    }

