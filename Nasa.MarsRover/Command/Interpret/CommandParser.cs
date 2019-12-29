using System;
using System.Collections.Generic;
using System.Linq;
using Nasa.MarsRover.LandingSurface;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Command.Interpret
{
    /// <summary>
    /// 3 farklı komut tipine ait sınıfların tiplerini belirleyerek ilgili fonksiyonlarla ilgili sınıfların Constructorları çağrılarak ilgili üyelerin değerleri atanır.
    /// </summary>
    public class CommandParser : ICommandParser
    {
        private readonly Func<Size, ILandingSurfaceSizeCommand> landingSurfaceSizeCommandFactory;
        private readonly Func<Point, CardinalDirection, IRoverInitialLocationCommand> roverInitialLocationCommandFactory;
        private readonly Func<IList<Movement>, IRoverExploreCommand> roverExploreCommandFactory;

        private readonly ICommandMatcher commandMatcher;
        private readonly IDictionary<CommandType, Func<string, ICommand>> commandParserDictionary;
        private readonly IDictionary<char, CardinalDirection> cardinalDirectionDictionary;
        private readonly IDictionary<char, Movement> movementDictionary;

        public CommandParser(ICommandMatcher aCommandMatcher, 
            Func<Size, ILandingSurfaceSizeCommand> aLandingSurfaceSizeCommandFactory, 
            Func<Point, CardinalDirection, IRoverInitialLocationCommand> aRoverInitialLocationCommandFactory, 
            Func<IList<Movement>, IRoverExploreCommand> aRoverExploreCommandFactory)
        {
            commandMatcher = aCommandMatcher;
            landingSurfaceSizeCommandFactory = aLandingSurfaceSizeCommandFactory;
            roverInitialLocationCommandFactory = aRoverInitialLocationCommandFactory;
            roverExploreCommandFactory = aRoverExploreCommandFactory;

            /// <summary>
            /// Func delegate i ile dictionary de function tutuyoruz.
            /// </summary>
            commandParserDictionary = new Dictionary<CommandType, Func<string, ICommand>>
            {
                 {CommandType.LandingSurfaceSizeCommand, ParseLandingSurfaceSizeCommand},
                 {CommandType.RoverInitialLocationCommand, ParseRoverInitialLocationCommand},
                 {CommandType.RoverExploreCommand, ParseRoverExploreCommand}
            };

            /// <summary>
            /// Yön dictionary
            /// </summary>
            cardinalDirectionDictionary = new Dictionary<char, CardinalDirection>
            {
                 {'N', CardinalDirection.North},
                 {'S', CardinalDirection.South},
                 {'E', CardinalDirection.East},
                 {'W', CardinalDirection.West}
            };
            
            movementDictionary = new Dictionary<char, Movement>
            {
                 {'L', Movement.Left},
                 {'R', Movement.Right},
                 {'M', Movement.Forward}
            };
        }
        /// <summary>
        /// Komut string ifadesini boşluk karakterlerine göre ayırır elde edilen arrayi commandMatcher sınıfının GetCommandType metodundan dönen komut tipini key olarak vererek
        /// commandParserDictionaryden eşleşen metot çağrılır.
        /// </summary>
        public IEnumerable<ICommand> Parse(string commandString)
        {
            var commands = commandString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return commands.Select(
                a => commandParserDictionary[commandMatcher.GetCommandType(a)]
                    .Invoke(a)).ToList();
        }
        /// <summary>
        /// gelen stringe göre Yüzey alanı kordinat uzunluğu bilgisi LandingSurfaceSizeCommand Size üyesine atanır.
        /// </summary>
        private ICommand ParseLandingSurfaceSizeCommand(string toParse)
        {
            var arguments = toParse.Split(' ');
            var width = int.Parse(arguments[0]);
            var height = int.Parse(arguments[1]);
            var size = new Size(width, height);

            var populatedCommand = landingSurfaceSizeCommandFactory(size);
            return populatedCommand;
        }
        /// <summary>
        /// Kordinat değerleri ve yön bilgisi parametreden split edilerek ayrıştırılır ve point sınıfı değerleri atanır. Daha sonra RoverInitialLocationCommand Sınıfı kordinat ve yön üyeleri atanır. 
        /// </summary>
        private ICommand ParseRoverInitialLocationCommand(string toParse)
        {
            var arguments = toParse.Split(' ');
            
            var deployX = int.Parse(arguments[0]);
            var deployY = int.Parse(arguments[1]);

            var directionSignifier = arguments[2][0];
            var deployDirection = cardinalDirectionDictionary[directionSignifier];

            var deployPoint = new Point(deployX, deployY);

            var populatedCommand = roverInitialLocationCommandFactory(deployPoint, deployDirection);
            return populatedCommand;
        }
        /// <summary>
        /// gelen stringden keşif adımları ayrıştırılarak dictionaryden eşleşen değerleri getirilir. movement listesi elde edildikten sonra  RoverExplore command sınıfına atanır. 
        /// </summary>
        private ICommand ParseRoverExploreCommand(string toParse)
        {
            var arguments = toParse.ToCharArray();
            var movements = arguments.Select(argument => movementDictionary[argument]).ToList();
            var populatedCommand = roverExploreCommandFactory(movements);
            return populatedCommand;
        }
    }
}