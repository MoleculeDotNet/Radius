/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public sealed class AppNotificationInfo: IAppNotificationInfo
    {
        internal AppNotificationInfo(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
