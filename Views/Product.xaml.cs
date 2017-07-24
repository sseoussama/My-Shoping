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
using My_Shopping.Views;
using My_Shopping.Class;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace My_Shopping.Views
{
    public sealed partial class Product : Page
    {
        bool aig = false;
        List<Produit> ListPr;
        Magasin shop;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);



            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            if (ProductManager.PageSource1 == typeof(Shops))
            {
                var List = e.Parameter as List<object>;
                ListPr = List[0] as List<Produit>;
                shop = List[1] as Magasin;
                AppShell.TextBar.Text = shop.Nom;
                ProductManager.MiseAjourProduit(ProductManager.ListCat, ListPr, shop.Nom);

            }
            else
            {
                if (ProductManager.PageSource1 == typeof(Favorite))
                {
                    var List = e.Parameter as List<object>;
                    var i = (int)List[0];
                    if (i == 0)
                    {
                        ListPr = List[1] as List<Produit>;
                        AppShell.TextBar.Text = List[2] as string;
                        ProductManager.MiseAjourProduit(ListPr);
                    }
                    else
                    {
                        if (i == 1)
                        {

                            ListPr = List[1] as List<Produit>;

                            AppShell.TextBar.Text = List[2] as string;
                            ProductManager.MiseAjourProduit(ProductManager.ListCat, ListPr, List[2] as string);
                        }
                    }
                }
                else
                {
                    ListPr = e.Parameter as List<Produit>;
                    AppShell.TextBar.Text = ProductManager.GetNomCatFromListPr(ProductManager.ListCat, ListPr);
                    ProductManager.MiseAjourProduit(ListPr);

                }
            }


            currentView.BackRequested += backButton_Tapped1;

        }
        private void backButton_Tapped1(object sender, BackRequestedEventArgs e)
        {
            if (ProductManager.PageSource1 == typeof(Category)) Frame.Navigate(typeof(Category));
            if (ProductManager.PageSource1 == typeof(Shops)) Frame.Navigate(typeof(Shops));
            if (ProductManager.PageSource1 == typeof(Favorite)) Frame.Navigate(typeof(Favorite));
        }




        public Product()
        {
            this.InitializeComponent();


            SuppGrids();
        }
        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ProductManager.PageSource1 != typeof(Shops))
            {
                ProductManager.PageSource = typeof(Product);
                Frame.Navigate(typeof(Shop), ((Produit)e.ClickedItem).ListMagasin);
            }


        }

        private void gridView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ProductManager.ProRightSelectedItem = (sender as StackPanel).DataContext as Produit;
            FrameworkElement senderElement = sender as FrameworkElement;
            MenuFlyout Menu = new MenuFlyout();
            //----------------------------------------------------------------------------           
            var AddShopItem = new MenuFlyoutItem();
            AddShopItem.Text = "Add Shop";
            Menu.Items.Add(AddShopItem);
            AddShopItem.Click += AddShopItem_Click;
            //-----------------------------------------------------------------------------
            var RemoProItem = new MenuFlyoutItem();
            RemoProItem.Text = "Remove";
            Menu.Items.Add(RemoProItem);
            RemoProItem.Click += RemoProItem_Click;
            //----------------------------------------------------------------------------
            var RenamProItem = new MenuFlyoutItem();
            RenamProItem.Text = "Rename";
            Menu.Items.Add(RenamProItem);
            RenamProItem.Click += RenamProItem_Click;
            //----------------------------------------------------------------------------
            var ChangImgProItem = new MenuFlyoutItem();
            ChangImgProItem.Text = "Change Image";
            Menu.Items.Add(ChangImgProItem);
            ChangImgProItem.Click += ChangImgProItem_Click;
            //----------------------------------------------------------------------------
            var AddToFavProItem = new MenuFlyoutItem();
            AddToFavProItem.Text = "Add to favorite";
            Menu.Items.Add(AddToFavProItem);
            AddToFavProItem.Click += AddToFavProItem_Click;


            Menu.ShowAt(senderElement, new Point(200, 100));
        }
        private async void AddShopItem_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Title = "Add Shop to " + ProductManager.ProRightSelectedItem.Nom;
            SuppGrids();
            ProductManager.ShopFile = null;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ShopImage.Source = img;
            aig = true;
            Thickness TopMargin = new Thickness(0, 80, 0, 0);
            GridAddShop.Margin = TopMargin;
            AddShopRemar.Text = String.Empty;
            panel.Children.Add(GridAddShop);
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource = typeof(Product);
                Frame.Navigate(typeof(Shop), ProductManager.ProRightSelectedItem.ListMagasin);
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
        private async void ChangImgProItem_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Title = "Change image to " + ProductManager.ProRightSelectedItem.Nom;
            SuppGrids();
            aig = true;
            ProductManager.ProFile = null;
            ChangeProImgRemar.Text = String.Empty;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ProImage.Source = img;
            panel.Children.Add(GridChangProImg);
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Category);
                Frame.Navigate(typeof(Product), ListPr);
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
        private async void RemoProItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
              "Are you sure to remove " + ProductManager.ProRightSelectedItem.Nom,
              "Remove Product");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {

                ProductManager.SupProduit(ProductManager.ListCat, ProductManager.ProRightSelectedItem.Nom, ProductManager.GetNomCatFromListPr(ProductManager.ListCat, ListPr));
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Category);
                Frame.Navigate(typeof(Product), ListPr);
            }

        }
        private async void RenamProItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridRenamPro);
            if (aig)
            {
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamPro.Margin = TopMargin;
            }
            Dialog.Title = "Rename " + ProductManager.ProRightSelectedItem.Nom;
            RenamProRemar.Text = String.Empty;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.PageSource1 = typeof(Category);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Product), ListPr);

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
        private async void AddToFavProItem_Click(object sender, RoutedEventArgs e)
        {
            var ListFavPro = ProductManager.GetPrFav(ProductManager.ListCat);
            if (ProductManager.Exist(ListFavPro, ProductManager.ProRightSelectedItem.Nom))
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                 ProductManager.ProRightSelectedItem.Nom + " Already added to favorite list",
                "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });




                dialog.DefaultCommandIndex = 0;
                var result = await dialog.ShowAsync();


            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
              "Are you sure to Add " + ProductManager.ProRightSelectedItem.Nom + " to favorite list",
              "Add to favorite");

                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();
                if (String.Equals(result.Label, "Yes"))
                {
                    ProductManager.ProRightSelectedItem.Favorite = true;
                    ProductManager.Sauvgarder();
                    Frame.Navigate(typeof(Favorite), null);
                }

            }
        }

        private void NewNomPro_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            RenamProRemar.Text = String.Empty;
        }

        private void RenameProButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(NewNomPro.Text, String.Empty)) RenamProRemar.Text = "No Text";
            else
            {
                if (String.Equals(NewNomPro.Text, ProductManager.ProRightSelectedItem.Nom))
                {
                    RenamProRemar.Text = "Product Renamed";
                }
                else
                {
                    if (ProductManager.Exist(ListPr, NewNomPro.Text)) RenamProRemar.Text = "Product Name Already Existed ";
                    else
                    {
                        ProductManager.ProRightSelectedItem.Nom = NewNomPro.Text;
                        RenamProRemar.Text = "Product Renamed";
                    }
                }

            }
        }
        private void SuppGrids()
        {

            if (GridRenamPro != null) panel.Children.Remove(GridRenamPro);
            if (GridAddShop != null) panel.Children.Remove(GridAddShop);
            if (GridChangProImg != null) panel.Children.Remove(GridChangProImg);

        }

        private void NomShop_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddShopRemar.Text = String.Empty;
        }

        private void Prix_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddShopRemar.Text = String.Empty;
            string TEXT = String.Empty;


            for (int i = 0; i < Prix.Text.Length; i++)
            {
                if (ProductManager.IsChiffr(Prix.Text[i])) TEXT = String.Concat(TEXT, Prix.Text[i]);
            }

            Prix.Text = TEXT;
        }

        private async void AddShopButton_Click(object sender, RoutedEventArgs e)
        {
            var bol = false;
            var ImgName = String.Empty;
            if (ToggleShop.IsOn) bol = true;
            if (String.Equals(NomShop.Text, String.Empty)) AddShopRemar.Text = "No text ";
            else
            {
                if (String.Equals(Prix.Text, String.Empty)) AddShopRemar.Text = "No Price";
                else
                {
                    if (ProductManager.Exist(ProductManager.ProRightSelectedItem, NomShop.Text)) AddShopRemar.Text = "Shop Already exist";
                    else
                    {
                        if (ProductManager.ShopFile != null)
                        {
                            ImgName = "Shop" + (ProductManager.GetAllMag(ProductManager.ListCat).Count + 1).ToString() + ".jpg";
                            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");

                            StorageFile copiedFile = await ProductManager.ShopFile.CopyAsync(localFolder, ImgName);
                        }

                        ProductManager.AjNouvMagasin(ProductManager.ListCat, ProductManager.ProRightSelectedItem.Nom, NomShop.Text, ProductManager.StringToFloat(Prix.Text), ProductManager.GetNomCatFromListPr(ProductManager.ListCat, ListPr), bol, ImgName);
                        AddShopRemar.Text = "Shop Added";
                        ProductManager.ShopFile = null;

                    }


                }

            }
        }

        private async void ChooseProImg_Click(object sender, RoutedEventArgs e)
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
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                   await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    bitmapImage.DecodePixelHeight = 1900;
                    bitmapImage.DecodePixelWidth = 190;
                    ProImage.Source = bitmapImage;
                }

            }
        }

        private async void ChangeProImg_Click(object sender, RoutedEventArgs e)
        {
            if (ProductManager.ProFile != null)
            {
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                ProductManager.ProRightSelectedItem.ImageName = "Pro" + (ProductManager.GetAllProduct(ProductManager.ListCat).Count).ToString() + ".jpg";
                StorageFile copiedFile = await ProductManager.ProFile.CopyAsync(localFolder, ProductManager.ProRightSelectedItem.ImageName, NameCollisionOption.ReplaceExisting);
            }
            else ProductManager.ProRightSelectedItem.ImageName = String.Empty;
            ProductManager.ProFile = null;
            ChangeProImgRemar.Text = "Image Changed";
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
    }
}
