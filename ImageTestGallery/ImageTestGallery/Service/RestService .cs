using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageTestGallery
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<bool> Create(string username, string email, string password, byte[] avatar)
        {
            bool res = false;

            var uri = new Uri(string.Format(Constants.RestUrl + Constants.RegisterAction, string.Empty));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            var formContent = new MultipartFormDataContent
                {
                         {new StreamContent(new MemoryStream(avatar)),"avatar","C:\avatar.jpg"}
                };
            formContent.Add(new StringContent(username), "username");
            formContent.Add(new StringContent(email), "email");
            formContent.Add(new StringContent(password), "password");

            try
            {
                var response = await client.PostAsync(uri, formContent);

                var rcontent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                   

                    UserResponse r = JsonConvert.DeserializeObject<UserResponse>(rcontent);

                    App.token = r.token;
                    App.avatar = r.avatar;

                    res = true;
                    // handle response here
                }
            } catch(Exception ex)
            {
                var txt = ex.Message;
            }

            return res;
        }

        public async Task<bool> Login(string email, string password)
        {
            bool res = false;

            var uri = new Uri(string.Format(Constants.RestUrl + Constants.LoginAction, string.Empty));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            var formContent = new MultipartFormDataContent();
        
            formContent.Add(new StringContent(email), "email");
            formContent.Add(new StringContent(password), "password");

            try
            {
                var response = await client.PostAsync(uri, formContent);

                var rcontent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    
                    UserResponse r = JsonConvert.DeserializeObject<UserResponse>(rcontent);

                    App.token = r.token;
                    App.avatar = r.avatar;

                    res = true;
                    // handle response here
                }
            }
            catch (Exception ex)
            {
                var txt = ex.Message;
            }

            return res;
        }


        public async Task<string> GetGif(string weather)
        {
           // bool res = false;
            string gif = string.Empty;

            var uri = new Uri(string.Format(Constants.RestUrl + Constants.GifAction + "?weather={0}", weather));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            client.DefaultRequestHeaders.Add("token", App.token);

            try
            {
                var response = await client.GetAsync(uri);
                
                var rcontent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {

                  var tmp = JsonConvert.DeserializeObject<GifResponse>(rcontent);
                    gif = tmp.gif;
                   
                    // handle response here
                }
            }
            catch (Exception ex)
            {
                var txt = ex.Message;
            }
            return gif;
           // return res;
        }

        public class GifResponse
        {
            public string gif { get; set; }
        }

        public async Task<bool> UploadImage(string description, string hashtag, float latitude, float longitude, byte[] image)
        {
            bool res = false;

            var uri = new Uri(string.Format(Constants.RestUrl + Constants.ImageAction, string.Empty));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            client.DefaultRequestHeaders.Add("token", App.token);

            var formContent = new MultipartFormDataContent
                {
                         {new StreamContent(new MemoryStream(image)),"image","C:\\image.jpg"}
                };
            formContent.Add(new StringContent(description), "description");
            formContent.Add(new StringContent(hashtag), "hashtag");
            formContent.Add(new StringContent(latitude.ToString()), "latitude");
            formContent.Add(new StringContent(longitude.ToString()), "longitude");

            try
            {
                var response = await client.PostAsync(uri, formContent);

                var rcontent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {


                   
                    res = true;
                    // handle response here
                }
            }
            catch (Exception ex)
            {
                var txt = ex.Message;
            }

            return res;
        }


        public async Task<ImageList> GetImages()
        {
            // bool res = false;
            

            var uri = new Uri(string.Format(Constants.RestUrl + Constants.AllAction));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            client.DefaultRequestHeaders.Add("token", App.token);

            try
            {
                var response = await client.GetAsync(uri);

                var rcontent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {

                    var tmp = JsonConvert.DeserializeObject<ImageList>(rcontent);
                    return tmp;

                    // handle response here
                }
            }
            catch (Exception ex)
            {
                var txt = ex.Message;
            }
            return null;
            // return res;
        }
    }
}
