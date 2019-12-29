using Nasa.MarsRover.LandingSurface;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Command
{
    /// <summary>
    /// Rover ın bulunduğu ilk koordinat komutlarıyla ilgili işlem yapan sınıftır. Rover a ilk hareketine hangi kordinatlar üzerinden başlaması gerektiğini söyler.
    /// </summary>
    public class RoverInitialLocationCommand : IRoverInitialLocationCommand
    {
        public Point DeployPoint { get; set; }
        public CardinalDirection DeployDirection { get; set; }
        private IRover rover;
        private ILandingSurface landingSurface;

        public RoverInitialLocationCommand(Point aPoint, CardinalDirection aDirection)
        {
            DeployPoint = aPoint;
            DeployDirection = aDirection;
        }

        public CommandType GetCommandType()
        {
            return CommandType.RoverInitialLocationCommand;
        }
        /// <summary>
        /// rover ın  ilk kordinat bilgilerinin verildiği metot
        /// </summary>
        public void Execute()
        {
            rover.Deploy(landingSurface, DeployPoint, DeployDirection);
        }

        /// <summary>
        /// rover.Deploy metodu için gerekli parametreleri set eden sınıftır.
        /// </summary>
        public void SetReceivers(IRover aRover, ILandingSurface aLandingSurface)
        {
            rover = aRover;
            landingSurface = aLandingSurface;
        }
    }
}
