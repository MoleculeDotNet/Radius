/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using Windows.Storage.Streams;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IMediaMetadata
    {
        string Album { get; }

        string Artist { get; }

        TimeSpan Duration { get; }

        string Subtitle { get; }

        IRandomAccessStreamReference Thumbnail { get; }

        string Title { get; }

        uint Track { get; }
    }
}