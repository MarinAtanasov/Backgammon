// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// A game of Backgammon.
    /// You can subscribe to <see cref="ITurnChanged"/> and <see cref="IGameEnded"/>.
    /// </summary>
    public interface IGame
    {
        #region Properties
        /// <summary>
        /// Gets whether the game is still running or has ended.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the current turn.
        /// It can be used to check which player must play next.
        /// </summary>
        ITurn Turn { get; }

        /// <summary>
        /// Gets a collection of the allowed moves for this turn.
        /// </summary>
        IReadOnlyCollection<IMove> AllowedMoves { get; }

        /// <summary>
        /// Gets the name of player who has won the game.
        /// </summary>
        string Winner { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the game with the selected first player.
        /// </summary>
        /// <param name="player">The player to play first</param>
        void Start(IPlayer player);
        
        /// <summary>
        /// Gets the board for the player.
        /// Both players use different representations of the same board.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The player's representation of the board.</returns>
        IBoard GetBoard(IPlayer player);

        /// <summary>
        /// Rolls the dice.
        /// </summary>
        /// <param name="player">The player.</param>
        void RollDice(IPlayer player);

        /// <summary>
        /// Plays an unused <see cref="IDie"/>.
        /// </summary>
        /// <param name="player">The player to play the die.</param>
        /// <param name="move">The (piece/die) move to be played.</param>
        void PlayMove(IPlayer player, IMove move);
        
        /// <summary>
        /// Ends the current turn, allowing the other player to roll the dice.
        /// </summary>
        /// <param name="player"></param>
        void EndTurn(IPlayer player);
        #endregion
    }
}
