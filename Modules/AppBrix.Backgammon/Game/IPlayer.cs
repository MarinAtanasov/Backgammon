// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;

namespace AppBrix.Backgammon.Game;

/// <summary>
/// A player in the game.
/// </summary>
public interface IPlayer
{
	#region Properties
	/// <summary>
	/// Gets the id of the player.
	/// </summary>
	Guid Id { get; }

	/// <summary>
	/// Gets the name of the player.
	/// </summary>
	string Name { get; }
	#endregion
}
