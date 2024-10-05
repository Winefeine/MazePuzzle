public class BasePhase
{

    public PhaseType CurPhase { get; protected set; }

    public BasePhase()
    {
        PhaseInitialization();
    }
    
    protected virtual void PhaseInitialization() { }

    public virtual void OnEnter() { }

    public virtual void OnPause() { }

    public virtual void OnResume() { }

    public virtual void OnExit() { }

    public virtual void OnRun() { }
}

public enum PhaseType
{
    
        
}
