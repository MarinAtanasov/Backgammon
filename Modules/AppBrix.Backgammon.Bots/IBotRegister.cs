// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;

namespace AppBrix.Backgammon.Bots;

/// <summary>
/// Defines a bot registers which can register a bot to a specified game.
/// </summary>
public interface IBotRegister
{
	/// <summary>
	/// Registers a bot
	/// </summary>
	/// <param name="game">The game</param>
	/// <param name="bot">The bot</param>
	void RegisterBot(IGame game, IBot bot);
}
