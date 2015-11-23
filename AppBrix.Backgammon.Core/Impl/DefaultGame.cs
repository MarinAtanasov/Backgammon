// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultGame : IGame
    {
        public DefaultGame(IApp app, IReadOnlyCollection<IPlayer> players)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (players.Count != 2)
                throw new ArgumentException("There should be 2 players, found " + players.Count);

            this.app = app;
            this.Players = players;
            this.Turn = this.CreateNewTurn(this.Players.First());
        }

        #region Properties
        public IReadOnlyCollection<IPlayer> Players { get; private set; }

        public ITurn Turn { get; private set; }
        #endregion

        #region Public and overriden methods
        public ITurn PlayDie(IPlayer player, IBoardLane lane, IDie die)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.Turn.Player != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);
            if (lane == null)
                throw new ArgumentNullException("lane");
            if (!player.Board.Lanes.Contains(lane))
                throw new ArgumentException("Lane not found: " + lane);
            if (die == null)
                throw new ArgumentNullException("die");
            if (!this.Turn.Dice.Contains(die))
                throw new ArgumentException("Die not found: " + die);
            if (die.IsUsed)
                throw new ArgumentException("This die has already been used: " + die);

            this.Turn = this.UseDie(die);
            return this.Turn;
        }

        public ITurn RollDice(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.Turn.Player != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);

            if (this.Turn.AreDiceRolled)
                throw new InvalidOperationException("Dice have already been rolled this turn.");

            this.Turn = this.RollDice();
            return this.Turn;
        }
        #endregion

        #region Private methods
        private ITurn CreateNewTurn(IPlayer player)
        {
            return new DefaultTurn(player, new IDie[0]);
        }

        private ITurn RollDice()
        {
            var dice = new List<IDie>(2);
            dice.Add(new DefaultDie(false, this.RollDie()));
            dice.Add(new DefaultDie(false, this.RollDie()));
            if (dice[0].Value == dice[1].Value)
            {
                dice.Add(new DefaultDie(false, dice[0].Value));
                dice.Add(new DefaultDie(false, dice[0].Value));
            }
            return new DefaultTurn(this.Turn.Player, dice);
        }

        private int RollDie()
        {
            return app.GetDiceRoller().RollDie();
        }

        private ITurn UseDie(IDie usedDie)
        {
            if (this.Turn.Dice.Count(x => x.IsUsed) < this.Turn.Dice.Count - 1)
                return new DefaultTurn(this.Turn.Player, this.Turn.Dice.Select(x => new DefaultDie(x.IsUsed || x == usedDie, x.Value)).ToArray());
            else if (this.Turn.Dice.Count == 4)
                return this.CreateNewTurn(this.Turn.Player);
            else
                return this.CreateNewTurn(this.Players.First(x => x != this.Turn.Player));
        }
        #endregion

        #region private fields and constants
        private readonly IApp app;
        #endregion
    }
}
