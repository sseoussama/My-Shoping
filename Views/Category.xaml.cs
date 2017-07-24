using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using My_Shopping.Class;
using System.Net.Http;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using System.Net;
using Windows.UI.Core;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace My_Shopping.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>

    public sealed partial class Category : Page
    {
        bool aig = false;
        List<Categorie> ListCat = new List<Categorie>();

        public Category()
        {

            this.InitializeComponent();

            ListCat = ProductManager.ListCat;
            SuppGrids();
            AppShell.TextBar.Text = "All Categories";
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            ProductManager.PageSource1 = typeof(Category);
            Frame.Navigate(typeof(Product), ((Categorie)e.ClickedItem).ListProduit);



        }

        private async void FindBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string Text = sender.Text;
                if (sender.Text.Length > 0)
                {
                    sender.ItemsSource = await Task<string[]>.Run(() => { return this.GetSuggestion(Text); });
                }
                else sender.ItemsSource = new string[] { "No seggestions.." };
            }
        }

        private string[] GetSuggestion(string text)

        {


            var ListSugget = ProductManager.GetNomCats(ProductManager.ListCat);

            var List = ListSugget.ToArray();

            var result = List.Where(x => x.StartsWith(text)).ToArray();
            return result;
        }

        private void FindBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var Selected = args.ChosenSuggestion as string;
            if (ProductManager.GetNomCats(ProductManager.ListCat).Contains(Selected))
            {
                sender.Text = String.Empty;
                ProductManager.PageSource1 = typeof(Category);
                Frame.Navigate(typeof(Product), ProductManager.GetCatFromNom(ProductManager.ListCat, Selected).ListProduit);
            }
            if (ProductManager.GetNomPros(ProductManager.ListCat).Contains(Selected))
            {
                sender.Text = String.Empty;
                ProductManager.PageSource = typeof(Products);

                Frame.Navigate(typeof(Shop), ProductManager.GetProFromNom(ProductManager.ListCat, Selected).ListMagasin);


            }
            if (ProductManager.GetNomShops(ProductManager.GetAllMag(ProductManager.ListCat)).Contains(Selected))
            {
                sender.Text = String.Empty;
                ProductManager.PageSource1 = typeof(Shops);
                Frame.Navigate(typeof(Product), ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.GetShopFromNom(ProductManager.ListCat, Selected)));
            }
        }

        private void gridView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ProductManager.CatRightSelectedItem = (sender as StackPanel).DataContext as Categorie;
            FrameworkElement senderElement = sender as FrameworkElement;
            MenuFlyout Menu = new MenuFlyout();
            //----------------------------------------------------------------------------           
            var AddProItem = new MenuFlyoutItem();
            AddProItem.Text = "Add Product";
            Menu.Items.Add(AddProItem);
            AddProItem.Click += AddProItem_Click;
            //-----------------------------------------------------------------------------
            var RemoCatItem = new MenuFlyoutItem();
            RemoCatItem.Text = "Remove";
            Menu.Items.Add(RemoCatItem);
            RemoCatItem.Click += RemoCatItem_Click;
            //----------------------------------------------------------------------------
            var RenamCatItem = new MenuFlyoutItem();
            RenamCatItem.Text = "Rename";
            Menu.Items.Add(RenamCatItem);
            RenamCatItem.Click += RenamCatItem_Click;
            //----------------------------------------------------------------------------
            var ChangCatImg = new MenuFlyoutItem();
            ChangCatImg.Text = "Change Image";
            Menu.Items.Add(ChangCatImg);
            ChangCatImg.Click += ChanCatImg_Click;
            //----------------------------------------------------------------------------
            var AddToFavCatItem = new MenuFlyoutItem();
            AddToFavCatItem.Text = "Add to favorite";
            Menu.Items.Add(AddToFavCatItem);
            AddToFavCatItem.Click += AddToFavCatItem_Click;


            Menu.ShowAt(senderElement, new Point(200, 100));
        }
        private async void AddProItem_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Title = "Add Product to " + ProductManager.CatRightSelectedItem.Nom;
            SuppGrids();
            ProductManager.ProFile = null;
            AddProRemar.Text = String.Empty;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ProImage.Source = img;
            aig = true;
            panel.Children.Add(GridAddPro);
            Thickness TopMargin = new Thickness(0, 70, 0, 0);
            GridAddPro.Margin = TopMargin;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Category);
                Frame.Navigate(typeof(Product), ProductManager.CatRightSelectedItem.ListProduit);

            };


            Dialog.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void RemoCatItem_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Windows.UI.Popups.MessageDialog(
           "Are you sure to remove " + ProductManager.CatRightSelectedItem.Nom,
           "Remove Category");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.SupCategorie(ProductManager.ListCat, ProductManager.CatRightSelectedItem.Nom);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Category), null);
            }

        }
        private async void RenamCatItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridRenamCat);
            if (aig)
            {
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamCat.Margin = TopMargin;
            }
            Dialog.Title = "Rename " + ProductManager.CatRightSelectedItem.Nom;
            RenamCatRemar.Text = String.Empty;
            NewNomCat.Text = String.Empty;
            Dialog.Width = 200;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Category), null);

            };


            Dialog.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }

        }
        private async void AddToFavCatItem_Click(object sender, RoutedEventArgs e)
        {
            var ListFavCat = ProductManager.GetCatFav(ProductManager.ListCat);
            if (ProductManager.Exist(ListFavCat, ProductManager.CatRightSelectedItem.Nom))
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                 ProductManager.CatRightSelectedItem.Nom + " Already added to favorite list",
                "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });




                dialog.DefaultCommandIndex = 0;
                var result = await dialog.ShowAsync();


            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
              "Are you sure to Add " + ProductManager.CatRightSelectedItem.Nom + " to favorite list",
              "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();
                if (String.Equals(result.Label, "Yes"))
                {
                    ProductManager.CatRightSelectedItem.Favorite = true;
                    ProductManager.Sauvgarder();
                    Frame.Navigate(typeof(Favorite), null);
                }

            }
        }
        private async void ChanCatImg_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridChangCatImg);
            Thickness TopMargin = new Thickness(0, 70, 0, 0);
            GridChangCatImg.Margin = TopMargin;
            ProductManager.CatFile = null;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            CatImage.Source = img;
            aig = true;
            Dialog.Title = "Change Image to " + ProductManager.CatRightSelectedItem.Nom;
            ChangeCatImgRemar.Text = String.Empty;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {

                ProductManager.Sauvgarder();

                Frame.Navigate(typeof(Category), null);

            };


            Dialog.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private void RenameCatButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(NewNomCat.Text, String.Empty)) RenamCatRemar.Text = "No Text";
            else
            {
                if (String.Equals(NewNomCat.Text, ProductManager.CatRightSelectedItem.Nom))
                {
                    RenamCatRemar.Text = "Category Renamed";
                }
                else
                {
                    if (ProductManager.Exist(ProductManager.ListCat, NewNomCat.Text)) RenamCatRemar.Text = "Category Name Already Existed ";
                    else
                    {
                        ProductManager.CatRightSelectedItem.Nom = NewNomCat.Text;
                        RenamCatRemar.Text = "Category Renamed";
                    }
                }

            }
        }

        private void NewNomCat_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            RenamCatRemar.Text = String.Empty;
        }
        private void SuppGrids()
        {

            if (GridRenamCat != null) panel.Children.Remove(GridRenamCat);
            if (GridAddPro != null) panel.Children.Remove(GridAddPro);
            if (GridChangCatImg != null) panel.Children.Remove(GridChangCatImg);

        }

        private void NomPro_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddProRemar.Text = String.Empty;
        }

        private async void AddProButton_Click(object sender, RoutedEventArgs e)
        {
            var bol = false;
            var ImgName = String.Empty;
            if (TogglePro.IsOn) bol = true;
            if (String.Equals(NomPro.Text, String.Empty)) AddProRemar.Text = "No text ";
            else
            {
                if (String.Equals(NomPro.Text, "No Product")) AddProRemar.Text = "Name Invalide ";
                else
                {
                    if (ProductManager.Exist(ProductManager.CatRightSelectedItem, NomPro.Text)) AddProRemar.Text = "Product Already exists";
                    else
                    {
                        if (ProductManager.ProFile != null)
                        {
                            ImgName = "Pro" + (ProductManager.GetAllProduct(ProductManager.ListCat).Count).ToString() + ".jpg";
                            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                            StorageFile copiedFile = await ProductManager.ProFile.CopyAsync(localFolder, ImgName, NameCollisionOption.ReplaceExisting);
                        }


                        ProductManager.AjNouvProduit(ProductManager.ListCat, NomPro.Text, ProductManager.CatRightSelectedItem.Nom, bol, ImgName);
                        AddProRemar.Text = "Product Added";
                        ProductManager.GetModifed = ProductManager.GetCatFromNom(ProductManager.ListCat, NomPro.Text);

                        ProductManager.ProFile = null;

                    }
                }

            }
        }

        private async void ChangeCatImg_Click(object sender, RoutedEventArgs e)
        {
            if (ProductManager.CatFile != null)
            {
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");

                ProductManager.CatRightSelectedItem.ImageName = "Cat" + (ProductManager.ListCat.Count).ToString() + ".jpg";

                StorageFile copiedFile = await ProductManager.CatFile.CopyAsync(localFolder, ProductManager.CatRightSelectedItem.ImageName, NameCollisionOption.ReplaceExisting);

            }
            else ProductManager.CatRightSelectedItem.ImageName = String.Empty;
            ChangeCatImgRemar.Text = "Image Changed";
            ProductManager.CatFile = null;
        }

        private async void ChooseCatImg_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {

                ProductManager.CatFile = file;
                // Application now has read/write access to the picked file
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                   await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    bitmapImage.DecodePixelHeight = 190;
                    bitmapImage.DecodePixelWidth = 190;
                    CatImage.Source = bitmapImage;
                }

            }
        }

        private async void ChooseProImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {

                ProductManager.ProFile = file;
                // Application now has read/write access to the picked file

                // Application now has read/write access to the picked file
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                   await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    bitmapImage.DecodePixelHeight = 190;
                    bitmapImage.DecodePixelWidth = 190;
                    ProImage.Source = bitmapImage;
                }





            }
        }
    }
}
