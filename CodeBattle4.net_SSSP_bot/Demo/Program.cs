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
        public List<Node> UsedNodes = new List<Node>();
        public Node Root;
        GameBoard GameBoard;

        public Paths(GameBoard gameBoard, Node root)
        {
            Root = root;
            GameBoard = gameBoard;
        }

        public Node DoMove(Node from)
        {
            var position = from.Position;

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
    }
}
