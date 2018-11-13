using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageTestGallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {

        byte[] avatar;
        public SignUpPage()
        {
            InitializeComponent();
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var user = new User()
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text,
                Email = emailEntry.Text
            };

            // Sign up logic goes here

            var signUpSucceeded = AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    // Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    //await Navigation.PopToRootAsync();
                    RestService serv = new RestService();
                    StreamImageSource streamImageSource = (StreamImageSource)imgAvatar.Source;
                    System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                    Task<Stream> task = streamImageSource.Stream(cancellationToken);
                    Stream stream = task.Result;
                    avatar = ReadFully(stream);
                   var res = await  serv.Create(user.Username,user.Email,user.Password, avatar);
                    if (res)
                    {
                         Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                        await Navigation.PopToRootAsync();
                    }

                }
            }
            else
            {
                messageLabel.Text = "Sign up failed";
            }
        }

        async void OnImageButtonClicked(object sender, EventArgs e)
        {
            //initialize our media plugin
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }


            // wait until the file is written
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            imgAvatar.Source = ImageSource.FromStream(() =>
        {
            var stream = file.GetStream();
          //  avatar = ReadFully(stream);
            return stream;
        });
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }
    }
}