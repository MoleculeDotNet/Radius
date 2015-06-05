/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public class MediaMetadata :IMediaMetadata
    {
        internal MediaMetadata(string album, string artist, TimeSpan duration, string subtitle, IRandomAccessStreamReference thumbnail,
            string title, uint track)
        {
            this.Album = album;
            this.Artist = artist;
            this.Duration = duration;
            this.Subtitle = subtitle;
            this.Thumbnail = thumbnail;
            this.Title = title;
            this.Track = track;
        }

        public string Album { get; private set; }

        public string Artist { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string Subtitle { get; private set; }

        public IRandomAccessStreamReference Thumbnail  { get; private set; }

        public string Title { get; private set; }

        public uint Track { get; private set; }
        
        public bool Equals(MediaMetadata other)
        {
            // Use thumbnail size as comparison
            bool thumbnailEqual = true;
            if((null == this.Thumbnail && null != other.Thumbnail)
                || (null != this.Thumbnail && null == other.Thumbnail))
            {
                thumbnailEqual = false;
            }
            else
            {
                thumbnailEqual = (this.GetThumbnailSize() == other.GetThumbnailSize());
            }

            return this.Title == other.Title &&
                this.Subtitle == other.Subtitle &&
                this.Artist == other.Artist &&
                this.Album == other.Album &&
                this.Duration.TotalMilliseconds == other.Duration.TotalMilliseconds &&
                thumbnailEqual &&
                this.Track == other.Track;
        }

        /// <summary>
        /// Returns a ulong representing the size in bytes of the thumbnail.
        /// If Thumbnail is null, it returns null.
        /// </summary>
        /// <returns></returns>
        public ulong? GetThumbnailSize()
        {
            if (this.Thumbnail == null)
                return null;

            IRandomAccessStreamWithContentType task = this.Thumbnail.OpenReadAsync().GetResults();
            return task.Size;
        }

    }
}
