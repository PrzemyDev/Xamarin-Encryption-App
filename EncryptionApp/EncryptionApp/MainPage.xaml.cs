using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EncryptionApp
{
    public partial class MainPage : ContentPage
    {
        private string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EncApp");
        AES aes = new AES();
        public MainPage()
        {
            InitializeComponent();
        }

        private string KeyInput;
        private string IV = "żź05sy!y3Sa9HH*@";
        private bool IsKeyCorrect = false;

        private async void btnAcceptKey_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(entKey.Text) || entKey.Text.Length < 16 || (entKey.Text.Length > 16 && entKey.Text.Length < 32) || entKey.Text.Length > 16 && entKey.Text.Length > 32)
            {
                await DisplayAlert("Klucz", "Klucz musi zawierać 16 lub 32 znaków", "OK");
                IsKeyCorrect = false;
            }
            else
            {
                KeyInput = entKey.Text;
                IsKeyCorrect = true;
                lblKeyInfo.Text = "Klucz poprawny!";
                await Task.Delay(2000);
                lblKeyInfo.Text = "EncryptionApp";
                entKey.IsReadOnly = true;
            }
        }
        private void btnClearKey_Clicked(object sender, EventArgs e) 
        {
            entKey.Text = String.Empty;
            entKey.IsReadOnly = false;
        }

        private void btnEncrypt_Clicked(object sender, EventArgs e)
        {

            if (IsKeyCorrect && !String.IsNullOrEmpty(edtUserInput.Text) )
            {
                byte[] bKey = Encoding.ASCII.GetBytes(KeyInput);
                byte[] bIV = Encoding.ASCII.GetBytes(IV);
                byte[] bEncrypt = aes.Encryption(edtUserInput.Text, bKey, bIV);
                edtUserInput.Text = Convert.ToBase64String(bEncrypt);
            }
            else
            {
                DisplayAlert("Info", "Najpierw należy zatwierdzić klucz oraz podać tekst do zaszyfrowania.", "OK");
            }

        }
        private void btnDecrypt_Clicked(object sender, EventArgs e)
        {
            if (IsKeyCorrect && !String.IsNullOrEmpty(edtUserInput.Text))
            {
                byte[] bKey = Encoding.ASCII.GetBytes(KeyInput);
                byte[] bIV = Encoding.ASCII.GetBytes(IV);
                byte[] bInput = Convert.FromBase64String(edtUserInput.Text);
                string sDecrypt = aes.Decription(bInput, bKey, bIV);
                edtUserInput.Text = sDecrypt;
            }
            else
            {
                DisplayAlert("Info", "Najpierw należy zatwierdzić klucz oraz podać tekst do odszyfrowania.", "OK");
            }
        }
        private async void btnCopy_Clicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(edtUserInput.Text);
            lblKeyInfo.Text = "Skopiowano wiadomość do schowka!";
            await Task.Delay(2000);
            lblKeyInfo.Text = "EncryptionApp";
        }
        private async void btnPaste_Clicked(object sender, EventArgs e)
        {
            edtUserInput.Text = await Clipboard.GetTextAsync();
        }
        private void btnClear_Clicked(object sender, EventArgs e)
        {
            edtUserInput.Text = String.Empty;
        }
        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Pomoc", "Aby zaszyfrować lub odszyfrować wiadomość należy wpisać klucz i wcisnąć przycisk \u0022ZATWIERDŹ\u0022. Tekst do szyfrowania i odszyfrowania należy podać poniżej. \nOznaczenia przycisków belki od lewej strony: \nZaszyfruj i Odszyfruj wiadomość, Skopiuj wiadomość do schowka, Wklej wiadomość ze schowka, Wyczyść wiadomość, Wyświetl pomoc, Wyłącz aplikację.\n\nAplikacja napisana przez:\nhttps://github.com/PrzemyDev - 2022", "OK");
        }
        private void btnExit_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        
    }
}
