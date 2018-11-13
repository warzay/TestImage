using System.Net.Http;

namespace ImageTestGallery
{
    public class UserData
    {
       public string username { get; set; }
       public string email { get; set; }
       public string password { get; set; }
       public byte[] avatar { get; set; }
    }
}