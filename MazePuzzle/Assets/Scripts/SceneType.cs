public class SceneType
{
    
    public string Name { get; private set; }

    public string Path { get; private set; }

    public SceneType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }
}