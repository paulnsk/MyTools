using System;
using MediaCycler2.Models;

namespace MediaCycler2.MediaDisplays
{
    public class MediaDisplayFactory
    {
        public IMediaDisplay Create(MediaContent mediaContent)
        {
            switch (mediaContent.Kind)
            {
                case MediaContentKind.Image:
                    return new ImageDisplay(mediaContent);
                    break;
                case MediaContentKind.Video:
                    return new VideoDisplay(mediaContent);
                    break;
                case MediaContentKind.WebPage:
                    return new WebPageDisplay(mediaContent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}