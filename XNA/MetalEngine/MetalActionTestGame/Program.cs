#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MetalActionEngine.MetalActionTestGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using ( var game = MetalGame.CreateFromXaml("Game") )
            {
                game.Run();
            }
        }
    }
}
