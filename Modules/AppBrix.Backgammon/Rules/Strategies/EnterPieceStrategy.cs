// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using AppBrix.Backgammon.Game.Impl;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Rules.Strategies;

internal class EnterPieceStrategy : GameRuleStrategyBase
{
	#region Public and overriden methods
	protected internal override IEnumerable<IMove> GetStrategyAvailableMoves(IBoard board, ITurn turn, IGameRuleStrategyContext context)
	{
		if (board.Bar.Any(x => x.Player == turn.Player))
		{
			foreach (var die in this.GetAvailableDice(turn.Dice))
			{
				if (this.IsMoveValid(board, die.Value, turn.Player))
				{
					yield return new DefaultMove(board.Bar, -1, die);
				}
			}
			context.IsDone = true;
		}
	}

	protected override bool CanStrategyMovePiece(IPlayer player, IBoard board, IMove move, IGameRuleStrategyContext context)
	{
		if (board.Bar.Any(x => x.Player == player.Name))
		{
			context.IsDone = true;
			return this.IsMoveValid(board, move.Die.Value, player.Name);
		}

		return false;
	}

	protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
	{
		if (move.Lane != board.Bar || !this.IsMoveValid(board, move.Die.Value, player.Name))
			return false;

		var lane = move.Lane;
		var pieceIndex = lane.Count - 1;
		while (lane[pieceIndex].Player != player.Name)
			pieceIndex--;
		var piece = lane[pieceIndex];
		lane.RemoveAt(pieceIndex);
		var targetLane = board.Lanes[move.Die.Value - 1];
		if (targetLane.Count == 1 && targetLane[0].Player != player.Name)
		{
			targetLane.MovePiece(lane);
		}
		targetLane.Add(piece);
		return true;
	}
	#endregion

	#region Private methods
	private bool IsMoveValid(IBoard board, int die, string player)
	{
		var targetLane = board.Lanes[die - 1];
		return targetLane.Count <= 1 || targetLane[0].Player == player;
	}
	#endregion
}
