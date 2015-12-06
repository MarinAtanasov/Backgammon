// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// A game of Backgammon.
    /// </summary>
    public interface IGame
    {
        #region Properties
        /// <summary>
        /// Gets the current turn.
        /// It can be used to check which player must play next.
        /// </summary>
        ITurn Turn { get; }
        #endregion

        #region Events
        /// <summary>
        /// Event fired after every move.
        /// </summary>
        event Action<ITurn> TurnChanged;

        /// <summary>
        /// Event fired after the game has been finished.
        /// </summary>
        event Action<IGameResult> GameFinished;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the board for the player.
        /// Both players use different representations of the same board.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The player's representation of the board.</returns>
        IBoard GetBoard(IPlayer player);

        /// <summary>
        /// Ends the current turn, allowing the other player to roll the dice.
        /// </summary>
        /// <param name="player"></param>
        void EndTurn(IPlayer player);

        /// <summary>
        /// Plays an unused <see cref="IDie"/>.
        /// </summary>
        /// <param name="player">The player to play the die.</param>
        /// <param name="lane">The lane which holds the piece to be moved.</param>
        /// <param name="die">The die to be used.</param>
        void PlayDie(IPlayer player, IBoardLane lane, IDie die);

        /// <summary>
        /// Rolls the dice.
        /// </summary>
        /// <param name="player">The player.</param>
        void RollDice(IPlayer player);
        #endregion
    }
}
