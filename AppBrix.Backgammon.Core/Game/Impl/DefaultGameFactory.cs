// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGameFactory : IGameFactory
    {
        #region Construction
        public DefaultGameFactory(IApp app)
        {
            this.app = app;
            this.gameRules = new BasicGameRules();
        }
        #endregion

        #region Public and overriden methods
        public IPlayer CreatePlayer(string name, Guid id)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (id == Guid.Empty)
                id = Guid.NewGuid();

            return new DefaultPlayer(name, id);
        }

        public IGame CreateGame(IReadOnlyList<IPlayer> players)
        {
            if (players == null)
                throw new ArgumentNullException("players");
            if (players.Count != 2)
                throw new ArgumentException("There should be exactly 2 players. Found: " + players.Count);
            
            return new DefaultGame(this.app, this.CreateBoard(players), players[0], players[1], this.gameRules);
        }
        #endregion

        #region Board initialization
        private IGameBoard CreateBoard(IReadOnlyList<IPlayer> players)
        {
            var board = new DefaultBoard();

            var lanes = (IList<IGameBoardLane>)board.Lanes;
            lanes[0] = new DefaultBoardLane(this.CreatePieces(2, players[0]));
            lanes[5] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[7] = new DefaultBoardLane(this.CreatePieces(3, players[1]));
            lanes[11] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[12] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[16] = new DefaultBoardLane(this.CreatePieces(3, players[0]));
            lanes[18] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[23] = new DefaultBoardLane(this.CreatePieces(2, players[1]));

            return board;
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
        private readonly IGameRules gameRules;
        #endregion
    }
}
