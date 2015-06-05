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
    public sealed class TextResponse : ITextResponse
    {
        public TextResponse(uint id, String content)
        {
            this.Id = id;
            this.Content = content;
        }

        public uint Id { get; private set; }

        public String Content { get; private set; }
    }
}
