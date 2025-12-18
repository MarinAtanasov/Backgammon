// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Rules;
using AppBrix.Lifecycle;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Game.Impl;

internal class DefaultGameFactory : IGameFactory, IApplicationLifecycle
{
	#region Public and overriden methods
	public void Initialize(IInitializeContext context)
	{
		this.app = context.App;
		this.gameRules = new DefaultGameRules();
	}

	public void Uninitialize()
	{
		this.app = null!;
		this.gameRules = null!;
	}

	public IPlayer CreatePlayer(string name, Guid id)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentNullException(nameof(name));

		if (id == Guid.Empty)
			id = Guid.NewGuid();

		return new DefaultPlayer(name, id);
	}

	public IGame CreateGame(IReadOnlyList<IPlayer> players)
	{
		if (players is null)
			throw new ArgumentNullException(nameof(players));
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

	private IPiece[] CreatePieces(int count, IPlayer owner)
	{
		var pieces = new IPiece[count];
		for (var i = 0; i < count; i++)
		{
			pieces[i] = new DefaultPiece(owner);
		}
		return pieces;
	}
	#endregion

	#region Private fields and constants
	private IApp app = null!;
	private IGameRules gameRules = null!;
	#endregion
}
