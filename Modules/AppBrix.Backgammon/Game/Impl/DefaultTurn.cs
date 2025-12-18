// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Game.Impl;

internal class DefaultTurn : ITurn
{
	#region Construction
	public DefaultTurn(IPlayer player, IReadOnlyList<IDie> dice)
	{
		if (player is null)
			throw new ArgumentNullException(nameof(player));
		if (dice is null)
			throw new ArgumentNullException(nameof(dice));

		this.Player = player.Name;
		this.Dice = dice;
	}
	#endregion

	#region Properties
	public bool AreDiceRolled => this.Dice.Count > 0;

	public IReadOnlyList<IDie> Dice { get; }

	public string Player { get; }
	#endregion
}
