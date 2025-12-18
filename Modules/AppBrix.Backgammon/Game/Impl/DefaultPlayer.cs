// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;

namespace AppBrix.Backgammon.Game.Impl;

internal class DefaultPlayer : IPlayer
{
	#region Construction
	public DefaultPlayer(string name, Guid id)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentNullException(nameof(name));
		if (id == Guid.Empty)
			throw new ArgumentException($"{nameof(id)} cannot be an empty GUID");

		this.Id = id;
		this.Name = name;
	}
	#endregion

	#region Properties
	public Guid Id { get; }

	public string Name { get; }
	#endregion

	#region Public and overriden methods
	public override bool Equals(object? obj) => obj is DefaultPlayer player && this.Id == player.Id && this.Name == player.Name;

	public override int GetHashCode() => this.Id.GetHashCode();

	public override string ToString() => string.Format("{0} ({1})", this.Name, this.Id);
	#endregion
}
