namespace GameJamKit.Pause
{
    public interface IPauseService
    {
        bool IsPaused { get; }

        delegate void PauseStateChangedEvent();
        event PauseStateChangedEvent? Paused;
        event PauseStateChangedEvent? Resumed;

        void Pause();
        void Unpause();
        void Toggle();
    }
}
