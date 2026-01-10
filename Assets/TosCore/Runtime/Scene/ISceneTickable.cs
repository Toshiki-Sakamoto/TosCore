namespace TosCore.Scene
{
    /// <summary>
    /// シーンの更新処理を1フレームごとに実行するためのインターフェース。
    /// </summary>
    public interface ISceneTickable
    {
        void Tick(float deltaTime);
    }
}
