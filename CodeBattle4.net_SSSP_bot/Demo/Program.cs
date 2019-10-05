/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using Loderunner.Api;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Demo
{
    class Program
    {
        const string ServerAddress = "localhost:8080";
        const string PlayerName = "<player-id>";
        const string AuthCode = "<code>";

        static void Main(string[] args)
        {
            // creating custom AI client
            var client = new LodeRunnerClient(ServerAddress, PlayerName, AuthCode);

            // starting thread with playing game
            client.Run(gameBoard =>
            {
                var root = new Node();


                Random random = new Random(Environment.TickCount);
                return (LoderunnerAction)random.Next(3);

            });

            // waiting for any key
            Console.ReadKey();

            // on any key - asking AI client to stop.
            client.InitiateExit();
        }
    }

    class Paths
    {
        public HashSet<Node> UsedNodes = new HashSet<Node>();
        public Node Root;
        GameBoard gameBoard;

        public Paths(GameBoard _gameBoard, Node root)
        {
            Root = root;
            gameBoard = _gameBoard;
        }

        void FindBestPath()
        {
            DoMove(Root);

        }

        public void DoMove(Node from)
        {            
            if(from.PositionElement == BoardElement.None)
            {
                DoMove(newMove);
                var shiftRight = from.Position.ShiftRight();
                DoMove(new Node(from, shiftLeft, gameBoard.GetElementAt(shiftLeft)));
            }
            if(from.)
        }

        private void MoveLift(Node from)
        {
            var shiftLeft = from.Position.ShiftLeft();
            var newMove = new Node(from, shiftLeft, gameBoard.GetElementAt(shiftLeft));
            from.To.Add(newMove);
            DoMove(newMove);
        }

        private void MoveRight(Node from)
        {
            var shiftRight = from.Position.ShiftRight();
            var newMove = new Node(from, shiftRight, gameBoard.GetElementAt(shiftRight));
            from.To.Add(newMove);
            DoMove(newMove);
        }

        private void Move(Node node, BoardPoint shift)
        {
            var newMove = new Node(node, shiftLeft, gameBoard.GetElementAt(shiftLeft));
            from.To.Add(newMove);
            DoMove(newMove);
        }

        private void MoveLift(Node from)
        {
            var shiftLeft = from.Position.ShiftLeft();
            var newMove = new Node(from, shiftLeft, gameBoard.GetElementAt(shiftLeft));
            from.To.Add(newMove);
            DoMove(newMove);
        }

        private void MoveLift(Node from)
        {
            var shiftLeft = from.Position.ShiftLeft();
            var newMove = new Node(from, shiftLeft, gameBoard.GetElementAt(shiftLeft));
            from.To.Add(newMove);
            DoMove(newMove);
        }
    }

    class Node
    {
        public Node From;
        public BoardPoint Position;
        public BoardElement PositionElement;
        public List<Node> To = new List<Node>();

        public Node(Node from, BoardPoint position)
        {
            From = from;
            Position = position;
        }

        public Node(Node from, BoardPoint position, BoardElement positionElement)
        {
            From = from;
            Position = position;
            PositionElement = positionElement;
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null &&
                   EqualityComparer<BoardPoint>.Default.Equals(Position, node.Position);
        }
        public override int GetHashCode()
        {
            return -425505606 + EqualityComparer<BoardPoint>.Default.GetHashCode(Position);
        }
    }
}
