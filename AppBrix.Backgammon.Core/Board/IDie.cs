// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board
{
    public interface IDie
    {
        #region Properties
        bool IsUsed { get; }

        int Value { get; }
        #endregion
    }
}
