using System;
using System.Collections.Generic;
using System.Text;

namespace ImageTestGallery
{
    public class Parameters
    {
        public int longitude { get; set; }
        public int latitude { get; set; }
        public string weather { get; set; }
    }

    public class Image
    {
        public int id { get; set; }
        public string description { get; set; }
        public string hashtag { get; set; }
        public Parameters parameters { get; set; }
        public string smallImagePath { get; set; }
        public string bigImagePath { get; set; }
        public string created { get; set; }
    }

    public class Gif
    {
        public int id { get; set; }
        public string weather { get; set; }
        public string path { get; set; }
        public string created { get; set; }
    }

    public class ImageList
    {
        public List<Image> images { get; set; }
        public List<Gif> gif { get; set; }
    }
}
