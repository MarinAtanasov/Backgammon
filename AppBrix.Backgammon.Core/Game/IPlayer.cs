// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    public interface IPlayer
    {
        #region Properties
        Guid Id { get; }
        
        string Name { get; }
        #endregion
    }
}
