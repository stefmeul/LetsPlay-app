using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using RestSharp;
using static System.Net.WebRequestMethods;
using System.IO;

namespace LetsPlay_app
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        // default profile img path variable 
        string imgPath = "/Img/profileImg.png"; // make function to load user img from api

        
        private void btnProfileImgUserSettings_Click(object sender, RoutedEventArgs e)
        {
            //user selects profile img
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select Profile Image",
                Filter = "Image Files (*.png;*.jpg;*.jpeg) | *.png;*.jpg;*.jpeg | All files (*.*) | *.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                imgPath = openFileDialog.FileName;

                imgProfileImgUserSettings.Source = new BitmapImage(new Uri(imgPath)); // add validation if user selects other filetype
            }
        }

        private async void btnSaveUserSettings_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(imgPath))
            {
                bool uploadSucces = await UploadToImgBB(imgPath);
                
                if (uploadSucces)
                {
                    MessageBox.Show("img uploaded");
                }
                else
                {
                    MessageBox.Show("not uploaded");
                }
            }

        
        }


        //function to upload to imgbb
        
        private async Task<bool> UploadToImgBB(string imgPath)
        {
            using (HttpClient  httpclient = new HttpClient())
            {
                string apiUrl = "https://api.imgbb.com/1/upload";
                string apiKey = "****"; // encrypt or hide for github public

                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(apiKey), "key");

                byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                formData.Add(new ByteArrayContent(imageBytes), "image", "image.png");

                HttpResponseMessage response = await httpclient.PostAsync(apiUrl, formData);

                if(response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    txbResult.Text = result;

                    return true;
                }

                return false;
            }

        }

        // function to get url from imgbb

    }
}
