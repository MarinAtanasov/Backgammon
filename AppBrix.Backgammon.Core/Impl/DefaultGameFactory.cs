// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    public class DefaultGameFactory : IGameFactory
    {
        #region Construction
        public DefaultGameFactory(IApp app)
        {
            this.app = app;
        }
        #endregion

        #region Public and overriden methods
        public IGame CreateGame(IReadOnlyList<string> playerNames)
        {
            if (playerNames == null)
                throw new ArgumentNullException("playerNames");
            if (playerNames.Count != 2)
                throw new ArgumentException("Players should be 2. Current players: " + playerNames.Count);
            if (playerNames[0] == playerNames[1])
                throw new ArgumentException("Players should have different names. Current names: " + playerNames[0]);

            var board = this.CreateBoard();
            var reversedBoard = new DefaultBoard(new ReversedLanes(board.Lanes));
            var players = new IPlayer[]
            {
                new DefaultPlayer(board, playerNames[0], Guid.NewGuid()),
                new DefaultPlayer(reversedBoard, playerNames[1], Guid.NewGuid())
            };
            this.SetBoard(board, players);

            return new DefaultGame(this.app, players);
        }
        #endregion

        #region Private methods
        private IBoard CreateBoard()
        {
            var lanes = new List<IBoardLane>(24);

            for (int i = 0; i < 24; i++)
            {
                lanes.Add(new DefaultBoardLane(new IPiece[0]));
            }

            return new DefaultBoard(lanes);
        }

        private void SetBoard(IBoard board, IReadOnlyList<IPlayer> players)
        {
            var lanes = (IList<IBoardLane>)board.Lanes;
            lanes[0] = new DefaultBoardLane(this.CreatePieces(2, players[0]));
            lanes[5] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[7] = new DefaultBoardLane(this.CreatePieces(3, players[1]));
            lanes[11] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[12] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[16] = new DefaultBoardLane(this.CreatePieces(3, players[0]));
            lanes[18] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[23] = new DefaultBoardLane(this.CreatePieces(2, players[1]));
        }

        private IPiece[] CreatePieces(int count, IPlayer owner = null)
        {
            var pieces = new IPiece[count];
            for (int i = 0; i < count; i++)
            {
                pieces[i] = new DefaultPiece(owner);
            }
            return pieces;
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
