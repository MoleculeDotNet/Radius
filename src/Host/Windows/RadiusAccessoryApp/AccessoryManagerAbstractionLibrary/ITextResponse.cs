/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface ITextResponse
    {
        string Content { get; }

        uint Id { get; }
    }
}