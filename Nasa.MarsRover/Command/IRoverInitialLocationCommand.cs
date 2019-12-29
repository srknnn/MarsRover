using Nasa.MarsRover.LandingSurface;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Command
{
    public interface IRoverInitialLocationCommand : ICommand
    {
        Point DeployPoint { get; set; }
        CardinalDirection DeployDirection { get; set; }
        void SetReceivers(IRover aRover, ILandingSurface aLandingSurface);
    }
}