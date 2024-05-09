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
using Newtonsoft.Json.Linq;
using LetsPlay_app.Classes; // for writing json to file

namespace LetsPlay_app
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page
    {
        private readonly SelectLogin selectLogin;
        string imgPath = "C:\\Users\\stef_\\Desktop\\Odisee graduaat programmeren\\portfolio\\LetsPlay app\\LetsPlay app\\LetsPlay app\\Img\\profileImg.png";
        string imageUrl = "";
        string profileImgUrl = "";

        public UserSettings()
        {
            InitializeComponent();


            selectLogin = new SelectLogin();

            // get user info from database
            RetrieveUserInfo();
        }

        private void RetrieveUserInfo()
        {
            // get email of logged in user
            string userEmail = UserSession.LoggedInUserEmail;

            Console.WriteLine("check user email: " + userEmail);

            if (userEmail != null)
            {
                UserInfo userInfo = selectLogin.GetUserInfo(userEmail);

                if (userInfo != null)   // --------------  how to save and encrypt password if changed by user, and how to leave it the same (so that the user can login with unencrypted)

                {
                    // add user info to form fields
                    txbNameUserSettings.Text = userInfo.UserName;
                    txbEmailUserSettings.Text = userInfo.Email;
                   // psbPasswordUserSettings.Password = userInfo.Password;
                    txbWebsiteUserSettings.Text = userInfo.WebsiteUrl;

                    // parse imgUrl to image source
                    imageUrl = userInfo.ImgUrl;

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        
                        imgProfileImgUserSettings.Source = new BitmapImage(new Uri(imageUrl));
                        imgPath = imageUrl;


                    }
                    
                    else
                    {
                        // set default profile img path variable 
                        imgPath = "C:\\Users\\stef_\\Desktop\\Odisee graduaat programmeren\\portfolio\\LetsPlay app\\LetsPlay app\\LetsPlay app\\Img\\profileImg.png";

                    }
                }

                else
                {
                    MessageBox.Show("error retrieveing user email");
                }
            }
        }



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
            if(!string.IsNullOrEmpty(imgPath) && (imgPath != imageUrl))
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

            // update database

            // get email of logged in user
            string userEmail = UserSession.LoggedInUserEmail;

            // get info from fields
            string name = txbNameUserSettings.Text;
            string email = txbEmailUserSettings.Text;
           // string password = psbPasswordUserSettings.Password;
            string websiteUrl = txbWebsiteUserSettings.Text;
            string imgUrl = imgProfileImgUserSettings.Source.ToString();

            if (!string.IsNullOrEmpty(profileImgUrl))
            {
                imgUrl = profileImgUrl;
            }
            else
            {
                imgUrl = imgPath;
            }
            UpdateUser update = new UpdateUser();

            bool updateSuccess = update.UpdateUserInfo(userEmail, name, imgUrl, websiteUrl); // password,

            if (updateSuccess)
            {
                MessageBox.Show("update succesfull");
                RetrieveUserInfo();
            }
            else
            {
                MessageBox.Show("update failed");

            }


        }


        //function to upload to imgbb

        private async Task<bool> UploadToImgBB(string imgPath)
        {
            using (HttpClient  httpclient = new HttpClient())
            {
                string apiUrl = "https://api.imgbb.com/1/upload";
                string apiKey = "7c31a3477fbc763597d09ae589d389d7"; // encrypt or hide for github public <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(apiKey), "key");

                byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                formData.Add(new ByteArrayContent(imageBytes), "image", "image.png");

                                                                                // add form handling if offline
                HttpResponseMessage response = await httpclient.PostAsync(apiUrl, formData);

                if(response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                   
                    txbResult.Text = result;

                    // write to file
                    JObject json = JObject.Parse(result);

                    // extract url from json object result
                    profileImgUrl = json["data"]["url"].ToString();

                    return true;
                }

                return false;
            }

        }

    }
}
