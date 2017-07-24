using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using My_Shopping.Class;
using My_Shopping.Views;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Automation;
using Windows.UI.Core;


// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace My_Shopping.Views
{
    public sealed partial class Shop : Page
    {

        List<Magasin> ListMag = new List<Magasin>();
        Produit Pr;
        bool aig = false;
        List<Magasin> List = new List<Magasin>();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);



            Pr = ProductManager.GetProFromNom(ProductManager.ListCat, ProductManager.GetNomProFromListMag(ProductManager.ListCat, e.Parameter as List<Magasin>));
            ListMag = e.Parameter as List<Magasin>;
            List = ListMag;


            AppShell.TextBar.Text = ProductManager.GetNomProFromListMag(ProductManager.ListCat, ListMag);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped1;
        }
        private void backButton_Tapped1(object sender, BackRequestedEventArgs e)
        {

            if (ProductManager.PageSource == typeof(Product))
            {

                Frame.Navigate(ProductManager.PageSource, (ProductManager.GetCategorieFromMag(ProductManager.ListCat, List)).ListProduit);

            }
            if (ProductManager.PageSource == typeof(Products))
            {

                Frame.Navigate(ProductManager.PageSource, null);

            }
            if (ProductManager.PageSource == typeof(Favorite))
            {

                Frame.Navigate(ProductManager.PageSource, null);

            }
        }








        public Shop()
        {


            this.InitializeComponent();

        }
        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void gridView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ProductManager.ShopRightSelectedItem = (sender as StackPanel).DataContext as Magasin;
            FrameworkElement senderElement = sender as FrameworkElement;
            MenuFlyout Menu = new MenuFlyout();
            //---------------------------------------------------------------------------- 
            var ChangPriceShopItem = new MenuFlyoutItem();
            ChangPriceShopItem.Text = "Change Price";
            Menu.Items.Add(ChangPriceShopItem);
            ChangPriceShopItem.Click += ChangPriceShopItem_Click;
            //----------------------------------------------------------------------------          

            var RenamShopItem = new MenuFlyoutItem();
            RenamShopItem.Text = "Rename";
            Menu.Items.Add(RenamShopItem);
            RenamShopItem.Click += RenamShopItem_Click;
            //----------------------------------------------------------------------------
            var ChangImgShopItem = new MenuFlyoutItem();
            ChangImgShopItem.Text = "Change Image";
            Menu.Items.Add(ChangImgShopItem);
            ChangImgShopItem.Click += ChangImgShopItem_Click;
            //----------------------------------------------------------------------------
            var RemoShopItem = new MenuFlyoutItem();
            RemoShopItem.Text = "Remove";
            Menu.Items.Add(RemoShopItem);
            RemoShopItem.Click += RemoShopItem_Click;
            //---------------------------------------------------------------------------- 
            var AddToFavShopItem = new MenuFlyoutItem();
            AddToFavShopItem.Text = "Add to favorite";
            Menu.Items.Add(AddToFavShopItem);
            AddToFavShopItem.Click += AddToFavShopItem_Click;


            Menu.ShowAt(senderElement, new Point(200, 100));
        }
        private async void RemoShopItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
             "Are you sure to remove " + ProductManager.ShopRightSelectedItem.Nom,
             "Remove Product");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.SupMagasin(ProductManager.ListCat, ProductManager.GetProFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom).Nom, ProductManager.ShopRightSelectedItem.Nom, ProductManager.GetCategorieFromMag(ProductManager.ListCat, ListMag).Nom);
                ProductManager.PageSource1 = typeof(Category);
                ProductManager.PageSource = typeof(Product);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shop), Pr.ListMagasin);
            }
        }
        private void SuppGrids()
        {

            if (GridRenamShop != null) panel.Children.Remove(GridRenamShop);
            if (GridChangShopImg != null) panel.Children.Remove(GridChangShopImg);
            if (GridChangePrice != null) panel.Children.Remove(GridChangePrice);


        }
        private async void RenamShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridRenamShop);
            Dialog.Title = "Rename " + ProductManager.ShopRightSelectedItem.Nom;
            RenamShopRemar.Text = String.Empty;
            if (aig)
            {
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamShop.Margin = TopMargin;
            }
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.PageSource1 = typeof(Category);
                ProductManager.PageSource = typeof(Product);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shop), Pr.ListMagasin);

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
        private async void ChangImgShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ShopImage.Source = img;
            panel.Children.Add(GridChangShopImg);
            aig = true;
            Dialog.Title = "Change Image to " + ProductManager.ShopRightSelectedItem.Nom;
            ChangeShopImgRemar.Text = String.Empty;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.PageSource1 = typeof(Category);
                ProductManager.PageSource = typeof(Product);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shop), Pr.ListMagasin);

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
        private async void AddToFavShopItem_Click(object sender, RoutedEventArgs e)
        {
            var ListFavShop = ProductManager.GetMagFav(ProductManager.ListCat);
            if (ProductManager.Exist(ListFavShop, ProductManager.ShopRightSelectedItem.Nom))
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                 ProductManager.ShopRightSelectedItem.Nom + " Already added to favorite list",
                "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });




                dialog.DefaultCommandIndex = 0;
                var result = await dialog.ShowAsync();


            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
              "Are you sure to Add " + ProductManager.ShopRightSelectedItem.Nom + " to favorite list",
              "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();
                if (String.Equals(result.Label, "Yes"))
                {

                    ProductManager.TrueFavShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem);
                    ProductManager.PageSource1 = typeof(Category);
                    ProductManager.PageSource = typeof(Product);
                    ProductManager.Sauvgarder();
                    Frame.Navigate(typeof(Favorite), null);
                }

            }
        }
        private async void ChangPriceShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridChangePrice);
            Dialog.Title = "Change Price to " + ProductManager.ShopRightSelectedItem.Nom;
            ChangePriceRemar.Text = String.Empty;
            if (aig)
            {
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridChangePrice.Margin = TopMargin;
            }
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.PageSource1 = typeof(Category);
                ProductManager.PageSource = typeof(Product);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shop), Pr.ListMagasin);

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

        private void NewNomShop_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            RenamShopRemar.Text = String.Empty;
        }

        private void RenameShopButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(NewNomShop.Text, String.Empty)) RenamShopRemar.Text = "No Text";
            else
            {
                if (String.Equals(NewNomShop.Text, ProductManager.ShopRightSelectedItem.Nom))
                {
                    RenamShopRemar.Text = "Shop Renamed";
                }
                else
                {
                    if (ProductManager.Exist(ProductManager.GetAllMag(ProductManager.ListCat), NewNomShop.Text)) RenamShopRemar.Text = "Shop Name Already Existed ";
                    else
                    {
                        ProductManager.RenameShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom, NewNomShop.Text);
                        RenamShopRemar.Text = "Shop Renamed";
                    }
                }

            }
        }

        private async void ChooseShopImg_Click(object sender, RoutedEventArgs e)
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

                ProductManager.ShopFile = file;
                // Application now has read/write access to the picked file
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                   await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    bitmapImage.DecodePixelHeight = 190;
                    bitmapImage.DecodePixelWidth = 190;
                    ShopImage.Source = bitmapImage;
                }

            }

        }

        private async void ChangeShopImg_Click(object sender, RoutedEventArgs e)
        {
            if (ProductManager.ShopFile != null)
            {
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                string imgNam = "Shop" + (ProductManager.GetAllMag(ProductManager.ListCat).Count).ToString() + ".jpg";
                ProductManager.ChangeImgShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom, imgNam);
                StorageFile copiedFile = await ProductManager.ShopFile.CopyAsync(localFolder, ProductManager.ShopRightSelectedItem.ImageName, NameCollisionOption.ReplaceExisting);
            }
            else ProductManager.ShopRightSelectedItem.ImageName = String.Empty;
            ChangeShopImgRemar.Text = "Image Changed";
            ProductManager.ShopFile = null;



        }

        private void ChangePrice_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(Prix.Text, String.Empty)) ChangePriceRemar.Text = "No Price";
            else
            {
                ProductManager.ShopRightSelectedItem.Prix = ProductManager.StringToFloat(Prix.Text);
                ChangePriceRemar.Text = "Price Changed";
            }
        }

        private void Prix_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            ChangePriceRemar.Text = String.Empty;
            string TEXT = String.Empty;


            for (int i = 0; i < Prix.Text.Length; i++)
            {
                if (ProductManager.IsChiffr(Prix.Text[i])) TEXT = String.Concat(TEXT, Prix.Text[i]);
            }

            Prix.Text = TEXT;
        }
    }
}
