// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//

namespace AppBrix.Backgammon.Board.Impl;

internal interface IGameBoardLane : IBoardLane
{
    #region Methods
    /// <summary>
    /// Adds a piece to the lane.
    /// </summary>
    /// <param name="piece">The piece to add to the lane.</param>
    void Add(IPiece piece);

    /// <summary>
    /// Moves a piece from the current lane to the target lane.
    /// </summary>
    /// <param name="target">The target lane which will receive the piece.</param>
    void MovePiece(IGameBoardLane target);

    /// <summary>
    /// Removes a piece specified by its index.
    /// </summary>
    /// <param name="index">The index of the piece.</param>
    void RemoveAt(int index);
    #endregion
}
