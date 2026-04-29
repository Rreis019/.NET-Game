namespace TheAdventure;

public static class Program
{
    public static void Main()
    {
        var game = Game.Instance;
        game.Run();
    }
}