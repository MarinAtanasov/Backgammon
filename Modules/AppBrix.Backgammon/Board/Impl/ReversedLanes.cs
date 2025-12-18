// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System.Collections;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Board.Impl;

internal class ReversedLanes : IReadOnlyList<IGameBoardLane>
{
	#region Construction
	public ReversedLanes(IReadOnlyList<IGameBoardLane> original)
	{
		this.original = original;
	}
	#endregion

	#region Properties
	public int Count => this.original.Count;

	public IGameBoardLane this[int index] => this.original[this.original.Count - 1 - index];
	#endregion

	#region Public and overriden methods
	public IEnumerator<IGameBoardLane> GetEnumerator() => this.original.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	#endregion

	#region Private fields and constants
	private readonly IReadOnlyList<IGameBoardLane> original;
	#endregion
}
