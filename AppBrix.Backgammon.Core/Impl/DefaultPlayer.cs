﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultPlayer : IPlayer
    {
        #region Construction
        public DefaultPlayer(string name, Guid id)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (id == Guid.Empty)
                throw new ArgumentNullException("id is empty GUID");
            
            this.Id = id;
            this.Name = name;
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        #endregion

        #region Public and overriden methods
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.Id);
        }
        #endregion
    }
}
