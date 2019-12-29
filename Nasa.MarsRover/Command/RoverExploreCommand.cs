using System.Collections.Generic;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Command
{
    public class RoverExploreCommand : IRoverExploreCommand
    {
        /// <summary>
        /// Rover a hareket etmesi ile alakalı komutları veren sınıftır.
        /// </summary>
        public IList<Movement> Movements { get; private set; }
        private IRover rover;
        
        public RoverExploreCommand(IList<Movement> someMovements)
        {
            Movements = someMovements;
        }

        public CommandType GetCommandType()
        {
            return CommandType.RoverExploreCommand;
        }
        /// <summary>
        /// Movement listesine göre rovera keşif yaptıran metot
        /// </summary>
        public void Execute()
        {
            rover.Move(Movements);
        }

        /// <summary>
        /// rover.Move metodu için gerekli parametreleri set eder
        /// </summary>
        public void SetReceiver(IRover aRover)
        {
            rover = aRover;
        }
    }
}
