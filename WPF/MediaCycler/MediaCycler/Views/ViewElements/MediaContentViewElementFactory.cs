using System;
using MediaCycler.ViewModels;

namespace MediaCycler.Views.ViewElements
{
    public static class MediaContentViewElementFactory
    {
        public static IMediacontentViewElement Create(MediaContent mediaContent)
        {
            switch (mediaContent.Kind)
            {
                case MediaContentKind.Image:
                    return new ImageView(mediaContent);
                case MediaContentKind.Video:
                    return new VideoView(mediaContent);
                case MediaContentKind.WebPage:
                    return new WebPageView(mediaContent);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}