using Nasa.MarsRover.LandingSurface;

namespace Nasa.MarsRover.Command
{
    /// <summary>
    /// Yüzey alanı uzunluğu bilgisi bu sınıfta tutulur.
    /// </summary>
    public class LandingSurfaceSizeCommand : ILandingSurfaceSizeCommand
    {
        public Size Size { get; private set; }
        private ILandingSurface landingSurface;

        public LandingSurfaceSizeCommand(Size aSize)
        {
            Size = aSize;
        }

        public CommandType GetCommandType()
        {
            return CommandType.LandingSurfaceSizeCommand;
        }
        /// <summary>
        /// yüzey alanı uzunluğunu belirleyen metot
        /// </summary>
        public void Execute()
        {
            landingSurface.SetSize(Size);
        }

        /// <summary>
        ///  landingSurface.SetSize metodu için gerekli parametreleri set eden sınıftır.
        /// </summary>
        public void SetReceiver(ILandingSurface aLandingSurface)
        {
            landingSurface = aLandingSurface;
        }
    }
}
