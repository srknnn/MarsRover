using System;
using System.Collections.Generic;
using Nasa.MarsRover.LandingSurface;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Command
{
    /// <summary>
    /// ICommand dan inherit alan ilgili interfacelerle eşleşen sınıfları dictionerydeki delegate lerde tutulan metotlardan uygun olanını bulup SetReceiver metotlarını çağıran sınıftır.
    /// </summary>
    public class CommandInvoker : ICommandInvoker
    {
        private readonly Func<IRover> roverFactory;
        private readonly IDictionary<CommandType, Action<ICommand>> setReceiversMethodDictionary;

        private ILandingSurface landingSurface;
        private IList<IRover> rovers;
        private IEnumerable<ICommand> commandList;

        public CommandInvoker(Func<IRover> aRoverFactory)
        {
            roverFactory = aRoverFactory;
            
            setReceiversMethodDictionary = new Dictionary<CommandType, Action<ICommand>>
            {
                {CommandType.LandingSurfaceSizeCommand, SetReceiversOnLandingSurfaceSizeCommand},
                {CommandType.RoverInitialLocationCommand, SetReceiversOnRoverInitialLocationCommand},
                {CommandType.RoverExploreCommand, SetReceiversOnRoverExploreCommand}
            };
        }

        public void SetLandingSurface(ILandingSurface aLandingSurface)
        {
            landingSurface = aLandingSurface;
        }

        public void SetRovers(IList<IRover> someRovers)
        {
            rovers = someRovers;
        }
        /// <summary>
        /// ICommandListesi atanır 
        /// </summary>
        public void Assign(IEnumerable<ICommand> aCommandList)
        {
            commandList = aCommandList;
        }

        /// <summary>
        /// ICommand Listesi içerisinden Command Sınıfından inherit almış sınıfların setReveivers metodlarını ve Execute metotlarını ayrı ayrı çalıştırır.
        /// </summary>
        public void InvokeAll()
        {
            foreach (var command in commandList)
            {
                setReceivers(command);
                command.Execute();
            }
        }
        /// <summary>
        /// InvokeAll sınıfından gelen ICommand interface ini CommandType dan ayırıp  setReceiversMethodDictionary deki 3 farklı metottan uygun olanını çağırır. 
        /// </summary>
        private void setReceivers(ICommand command)
        {
            setReceiversMethodDictionary[command.GetCommandType()]
                .Invoke(command);
        }

        private void SetReceiversOnLandingSurfaceSizeCommand(ICommand command)
        {
            var landingSurfaceSizeCommand = (ILandingSurfaceSizeCommand) command;
            landingSurfaceSizeCommand.SetReceiver(landingSurface);
        }

        private void SetReceiversOnRoverInitialLocationCommand(ICommand command)
        {
            var roverDeployCommand = (IRoverInitialLocationCommand) command;
            var newRover = roverFactory();
            rovers.Add(newRover);
            roverDeployCommand.SetReceivers(newRover, landingSurface);
        }

        private void SetReceiversOnRoverExploreCommand(ICommand command)
        {
            var roverExploreCommand = (IRoverExploreCommand) command;
            var latestRover = rovers[rovers.Count - 1];
            roverExploreCommand.SetReceiver(latestRover);
        }
    }
}