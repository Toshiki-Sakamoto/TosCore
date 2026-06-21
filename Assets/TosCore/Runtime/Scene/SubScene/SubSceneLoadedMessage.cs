namespace TosCore.Scene
{
    /// <summary>
    /// サブシーンが加算ロードされたことを通知する MessagePipe メッセージ
    /// </summary>
    public readonly struct SubSceneLoadedMessage
    {
        public LoadedSubScene Scene { get; }

        public SubSceneLoadedMessage(LoadedSubScene scene) => Scene = scene;
    }
}
