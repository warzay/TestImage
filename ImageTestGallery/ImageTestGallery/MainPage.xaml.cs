using DLToolkit.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageTestGallery
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
       
		public MainPage ()
		{
			InitializeComponent ();
            FlowListView.Init();


            GetImages();
        }

        async Task GetImages()
        {
            RestService serv = new RestService();
            var res = await serv.GetImages();
            if (res!=null)
            {
                ImageList.FlowItemsSource = res.images;
            }
        }

        async void OnShowGifClicked(object sender, EventArgs e)
        {
            RestService serv = new RestService();

            var res = await serv.GetGif(string.Empty);
            if (!string.IsNullOrEmpty(res))
            {
                  imgPopup.Source = ImageSource.FromUri(new Uri (res));
            
                popupImageView.IsVisible = true;
               
            }

        }
        async void OnAddImageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddImage());

        }

        async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            popupImageView.IsVisible = false;
        }
    }
}