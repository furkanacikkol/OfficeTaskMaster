public interface IStageManager
{
    event System.Action StageCompleted;

    void Initialize();
    void Cleanup();
}