namespace WebOTronic.Core
{
    public interface IGame
    {
        void LeftPaddle(string direction);
        void RightPaddle(string direction);
        void Run();
        World Init(string leftPaddleId, string rightPaddleId);
  }
}