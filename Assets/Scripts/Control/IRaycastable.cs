namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool TryHandleRaycast(PlayerController callingController);
    }
}
