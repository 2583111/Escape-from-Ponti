public abstract class BaseState
{
    public Zombie zombie;
    //instance of zombie class
    public StateMachine stateMachine;
    //instance of statemachine class

    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();

}