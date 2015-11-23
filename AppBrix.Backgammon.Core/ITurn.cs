// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core
{
    public interface ITurn
    {
        #region Properties
        bool AreDiceRolled { get; }

        IReadOnlyList<IDie> Dice { get; }

        IPlayer Player { get; }
        #endregion
    }
}
