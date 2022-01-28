using System;

namespace FourCorners
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FourCorners())
                game.Run();
        }
    }
}
