// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//

namespace AppBrix.Backgammon.Board
{
    /// <summary>
    /// One of the dice being used while playing Backgammon.
    /// </summary>
    public interface IDie
    {
        #region Properties
        /// <summary>
        /// Gets whether the die has been used.
        /// </summary>
        bool IsUsed { get; }

        /// <summary>
        /// Gets the value of the die. Value is between 1 and 6.
        /// </summary>
        int Value { get; }
        #endregion
    }
}
