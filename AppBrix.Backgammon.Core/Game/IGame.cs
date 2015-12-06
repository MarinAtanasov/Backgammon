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
    /// You can subscribe to <see cref="ITurnChanged"/> and <see cref="IGameEnded"/>.
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
        /// Subscribes to the <see cref="IGameEnded"/> event for the current game.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        void OnGameEnded(Action<IGameEnded> handler);

        /// <summary>
        /// Subscribes to the <see cref="ITurnChanged"/> event for the current game.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        void OnTurnChanged(Action<ITurnChanged> handler);
        #endregion

        #region Methods
        /// <summary>
        /// Ends the current turn, allowing the other player to roll the dice.
        /// </summary>
        /// <param name="player"></param>
        void EndTurn(IPlayer player);

        /// <summary>
        /// Gets the board for the player.
        /// Both players use different representations of the same board.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The player's representation of the board.</returns>
        IBoard GetBoard(IPlayer player);
        
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

        /// <summary>
        /// Starts the game with the selected first player.
        /// </summary>
        /// <param name="player">The player to play first</param>
        void Start(IPlayer player);
        #endregion
    }
}
