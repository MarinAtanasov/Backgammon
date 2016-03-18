// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Bots
{
    public abstract class BotBase : IBot
    {
        #region Construction
        /// <summary>
        /// Creates a new instance of <see cref="BotBase"/>.
        /// </summary>
        /// <param name="player">The player whose turn the bot will control.</param>
        public BotBase(IPlayer player)
        {
            this.Player = player;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the player whose turn the bot will control.
        /// </summary>
        public IPlayer Player { get; private set; }
        #endregion

        #region Public and abstract methods
        /// <summary>
        /// Executes when a game's turn is changed.
        /// </summary>
        /// <param name="game">The game.</param>
        public void PlayTurn(IGame game)
        {
            var moves = game.Rules.GetAvailableMoves(game.GetBoard(this.Player), game.Turn).ToList();
            if (!game.Turn.AreDiceRolled)
            {
                game.RollDice(this.Player);
            }
            else if (moves.Count == 0)
            {
                game.EndTurn(this.Player);
            }
            else
            {
                this.MakeMove(game, moves);
            }
        }

        /// <summary>
        /// Called by <see cref="PlayTurn(IGame)"/> when a piece must be moved.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="moves">Available moves.</param>
        protected abstract void MakeMove(IGame game, IReadOnlyList<IMove> moves);
        #endregion
    }
}
