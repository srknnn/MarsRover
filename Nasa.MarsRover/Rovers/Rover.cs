using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nasa.MarsRover.LandingSurface;

namespace Nasa.MarsRover.Rovers
{
    public class Rover : IRover
    {
        public Point Position { get; set; }
        public CardinalDirection CardinalDirection { get; set; }
        private bool isDeployed;
        private readonly IDictionary<Movement, Action> movementMethodDictionary;
        private readonly IDictionary<CardinalDirection, Action> leftMoveDictionary;
        private readonly IDictionary<CardinalDirection, Action> rightMoveDictionary;
        private readonly IDictionary<CardinalDirection, Action> forwardMoveDictionary;

        public Rover()
        {
            movementMethodDictionary = new Dictionary<Movement, Action>
            {
                {Movement.Left, () => leftMoveDictionary[CardinalDirection].Invoke()},
                {Movement.Right, () => rightMoveDictionary[CardinalDirection].Invoke()},
                {Movement.Forward, () => forwardMoveDictionary[CardinalDirection].Invoke()}
            };

            leftMoveDictionary = new Dictionary<CardinalDirection, Action>
            {
                {CardinalDirection.North, () => CardinalDirection = CardinalDirection.West},
                {CardinalDirection.East, () => CardinalDirection = CardinalDirection.North},
                {CardinalDirection.South, () => CardinalDirection = CardinalDirection.East},
                {CardinalDirection.West, () => CardinalDirection = CardinalDirection.South}
            };

            rightMoveDictionary = new Dictionary<CardinalDirection, Action>
            {
                {CardinalDirection.North, () => CardinalDirection = CardinalDirection.East},
                {CardinalDirection.East, () => CardinalDirection = CardinalDirection.South},
                {CardinalDirection.South, () => CardinalDirection = CardinalDirection.West},
                {CardinalDirection.West, () => CardinalDirection = CardinalDirection.North}
            };
            
            forwardMoveDictionary = new Dictionary<CardinalDirection, Action>
            {
                {CardinalDirection.North, () => {Position = new Point(Position.X, Position.Y + 1);}},
                {CardinalDirection.East, () => {Position = new Point(Position.X + 1, Position.Y);}},
                {CardinalDirection.South, () => {Position = new Point(Position.X, Position.Y - 1);}},
                {CardinalDirection.West, () => {Position = new Point(Position.X - 1, Position.Y);}}
            };
        }

        /// <summary>
        /// Rover ın keşife başlayacağı kordinatlar ve yön set edilir.
        /// </summary>
        public void Deploy(ILandingSurface aLandingSurface, Point aPoint, CardinalDirection aDirection)
        {
            if (aLandingSurface.IsValid(aPoint))
            {
                Position = aPoint;
                CardinalDirection = aDirection;
                isDeployed = true;
                return;
            }

            throwDeployException(aLandingSurface, aPoint);
        }

        /// <summary>
        /// Rover ın hareket ettiği metottur. Hareketler sırası ile movementMethodDictionary dictionaryden ayrışıp leftMoveDictionary,rightMoveDictionary,forwardMoveDictionary den 
        /// uygun key değerleri ile yeni CardinalDirection veya Point değerleri alırlar. Bu alınan her yeni değer rover ın yönünü yada kordinatını değitiren değerdir. 
        /// </summary>
        public void Move(IEnumerable<Movement> movements)
        {
            foreach (var movement in movements)
            {
                movementMethodDictionary[movement].Invoke();
            }
        }

        public bool IsDeployed()
        {
            return isDeployed;
        }

        private static void throwDeployException(ILandingSurface aLandingSurface, Point aPoint)
        {
            var size = aLandingSurface.GetSize();
            var exceptionMessage = String.Format("Deploy failed for point ({0},{1}). Landing surface size is {2} x {3}.",
                aPoint.X, aPoint.Y, size.Width, size.Height);
            throw new RoverDeployException(exceptionMessage);
        }
    }
}
