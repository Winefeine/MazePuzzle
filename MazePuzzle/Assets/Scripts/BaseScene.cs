
public abstract class BaseScene
{
    public SceneType SceneType { get; private set; }

    public abstract void OnEnter();

    public virtual void OnPause() { }

    public virtual void OnResume() { }

    public abstract void OnExit();
}
