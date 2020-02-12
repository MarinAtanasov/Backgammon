// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//

namespace AppBrix.Backgammon.Game
{
    /// <summary>
    /// Generator used for rolling dice.
    /// </summary>
    public interface IDiceRoller
    {
        /// <summary>
        /// Rolls a die and returns its value.
        /// Value is between 1 and 6.
        /// </summary>
        /// <returns>The rolled value.</returns>
        int RollDie();
    }
}
