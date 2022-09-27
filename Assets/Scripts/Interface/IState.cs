namespace Interface
{
    public interface IState
    {
        void OnSetup();

        void OnEnter();

        void OnExit();
    }
}
