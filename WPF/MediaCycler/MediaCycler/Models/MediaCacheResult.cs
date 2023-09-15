namespace MediaCycler.Models
{
    public class MediaCacheResult
    {
        public string LocalFilePath { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsInitializing { get; set; }

    }
}
