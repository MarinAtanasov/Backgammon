// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    public interface ITurn
    {
        #region Properties
        bool AreDiceRolled { get; }

        IReadOnlyList<IDie> Dice { get; }

        string Player { get; }
        #endregion
    }
}
