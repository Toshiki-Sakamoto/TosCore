namespace TosCore.Scene
{
    /// <summary>
    /// シーン遷移要求イベント
    /// </summary>
//    public readonly record struct SceneTransitionRequestEvent(string SceneName);
    public readonly struct  SceneTransitionRequestEvent
    {
        public string SceneName { get; }
        
        public SceneTransitionRequestEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
