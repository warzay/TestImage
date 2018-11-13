using ExifLib;
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
	public partial class AddImage : ContentPage
	{
        byte[] image;
        public AddImage ()
		{
			InitializeComponent ();
		}
        double gpsLat, gpsLng;
        async void OnPhotoButtonClicked(object sender, EventArgs e)
        {
            //initialize our media plugin
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }


            // wait until the file is written
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true              
            });

            if (file == null)
                return;
            else
            {
                imgAvatar.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    //  avatar = ReadFully(stream);
                    return stream;
                });
                using (Stream streamPic = file.GetStream())
                {
                    var picInfo = ExifReader.ReadJpeg(streamPic);


                    latitude = picInfo.GpsLatitude[0] + picInfo.GpsLatitude[1] / 60 + picInfo.GpsLatitude[2] / 3600;
                    longitude = picInfo.GpsLongitude[0] + picInfo.GpsLongitude[1] / 60 + picInfo.GpsLongitude[2] / 3600;


                    messageLabel.Text = string.Format("GpsLatitude {0}, GpsLongitude {1}", latitude, longitude);

                }
            }
        }

        double latitude = 0;
        double longitude=0;
        async void OnImageButtonClicked(object sender, EventArgs e)
        {
            //initialize our media plugin
            await CrossMedia.Current.Initialize();

        

            // wait until the file is written
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;
            else
            {
                imgAvatar.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    //  avatar = ReadFully(stream);
                   return stream;
                });
                using (Stream streamPic = file.GetStream())
                {
                    var picInfo = ExifReader.ReadJpeg(streamPic);
                   

                     latitude = picInfo.GpsLatitude[0] + picInfo.GpsLatitude[1] / 60 + picInfo.GpsLatitude[2] / 3600;
                     longitude = picInfo.GpsLongitude[0] + picInfo.GpsLongitude[1] / 60 + picInfo.GpsLongitude[2] / 3600;


                    messageLabel.Text = string.Format("GpsLatitude {0}, GpsLongitude {1}", latitude, longitude);

                }
            }
           

         
        }

        public byte [] ImgToByteArray ()
        {
            StreamImageSource streamImageSource = (StreamImageSource)imgAvatar.Source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            var res = ReadFully(stream);
            return res;
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
        async void OnAddImageClicked(object sender, EventArgs e)
        {
            //  await Navigation.PushAsync(new AddImage());
            RestService serv = new RestService();

            var res = await serv.UploadImage(descriptionEntry.Text, hastagEntry.Text, (float)latitude, (float)longitude, ImgToByteArray());
            if (res)
            {
                DisplayAlert("Upload", "Image Uploaded", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}