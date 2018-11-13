using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageTestGallery
{
    public interface IRestService
    {
        Task<bool> Create(string username, string email, string password, byte[] avatar);
    }
}
