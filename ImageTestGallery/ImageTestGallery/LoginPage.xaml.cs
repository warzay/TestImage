using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageTestGallery
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
#if DEBUG
            usernameEntry.Text = "zzay@zzay.zzay";
            passwordEntry.Text = "zzay";

#endif
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        bool clicked = false;
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (!clicked)
            {
                clicked = true;
                var user = new User
                {
                    Email = usernameEntry.Text,
                    Password = passwordEntry.Text
                };

                RestService serv = new RestService();

                var res = await serv.Login(user.Email, user.Password);
                if (res)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
                }

                else
                {
                    messageLabel.Text = "Login failed";
                    passwordEntry.Text = string.Empty;
                }
                clicked = false;
            }

        }

    }
}
