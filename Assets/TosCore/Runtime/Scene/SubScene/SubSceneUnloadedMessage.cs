namespace TosCore.Scene
{
    /// <summary>
    /// サブシーンがアンロードされたことを通知する MessagePipe メッセージ
    /// </summary>
    public readonly struct SubSceneUnloadedMessage
    {
        public SceneReference Scene { get; }

        public SubSceneUnloadedMessage(SceneReference scene) => Scene = scene;
    }
}
