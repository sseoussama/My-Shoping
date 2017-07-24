using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.Storage;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using My_Shopping.Class;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace My_Shopping.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Favorite : Page
    {
        List<Produit> ListPr = new List<Produit>();
        bool aig = false;
        bool aig1 = false;
        bool aig2 = false;
        int i;
        List<Categorie> ListFavCat = new List<Categorie>();
        List<Produit> ListFavPr = new List<Produit>();
        List<Magasin> ListFavMag = new List<Magasin>();
        public Favorite()
        {
            this.InitializeComponent();
            AppShell.TextBar.Text = "Al favorites";
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            ListPr = ProductManager.GetAllProduct(ProductManager.ListCat);
            ListFavCat = ProductManager.GetCatFav(ProductManager.ListCat);
            ListFavPr = ProductManager.GetPrFav(ProductManager.ListCat);
            ProductManager.MiseAjourProduit(ProductManager.ListCat, ListFavPr);
            ListFavMag = ProductManager.GetAllFavShop(ProductManager.ListCat);
            ProductManager.MiseAjourMagasin(ProductManager.ListCat, ListFavMag);
        }
        private void GridView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProductManager.PageSource1 = typeof(Favorite);
            i = 0;
            Frame.Navigate(typeof(Product), new List<object> { i, ((Categorie)e.ClickedItem).ListProduit, ((Categorie)e.ClickedItem).Nom });
        }
        private void GridView2_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProductManager.PageSource = typeof(Favorite);
            Frame.Navigate(typeof(Shop), ((Produit)e.ClickedItem).ListMagasin);
        }

        private void GridView3_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProductManager.PageSource1 = typeof(Favorite);
            i = 1;
            Frame.Navigate(typeof(Product), new List<object> { i, ProductManager.GetListPrFromMag(ProductManager.ListCat, (Magasin)e.ClickedItem), ((Magasin)e.ClickedItem).Nom });
        }
        private void gridView_RightTapped1(object sender, RightTappedRoutedEventArgs e)
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


            var RemoveFromFavCatItem = new MenuFlyoutItem();
            RemoveFromFavCatItem.Text = "Remove From Favory list";
            Menu.Items.Add(RemoveFromFavCatItem);
            RemoveFromFavCatItem.Click += RemoveFromFavCatItem_Click;

            Menu.ShowAt(senderElement, new Point(200, 100));

        }
        private void gridView_RightTapped2(object sender, RightTappedRoutedEventArgs e)
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
            var RemoveFromFavProItem = new MenuFlyoutItem();
            RemoveFromFavProItem.Text = "Remove from list favory";
            Menu.Items.Add(RemoveFromFavProItem);
            RemoveFromFavProItem.Click += RemoveFromFavProItem_Click;


            Menu.ShowAt(senderElement, new Point(200, 100));
        }
        private void gridView_RightTapped3(object sender, RightTappedRoutedEventArgs e)
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
            var AddProToShopItem = new MenuFlyoutItem();
            AddProToShopItem.Text = "Add Product ";
            Menu.Items.Add(AddProToShopItem);
            AddProToShopItem.Click += AddProToShopItem_Click;
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
            var RemoveFromFavShopItem = new MenuFlyoutItem();
            RemoveFromFavShopItem.Text = "Remove From Favory list";
            Menu.Items.Add(RemoveFromFavShopItem);
            RemoveFromFavShopItem.Click += RemoveFromFavShopItem_Click;


            Menu.ShowAt(senderElement, new Point(200, 100));
        }
        private async void RemoveFromFavCatItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
          "Are you sure to remove " + ProductManager.CatRightSelectedItem.Nom + " From List Favory",
          "Remove Category from list favory");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.CatRightSelectedItem.Favorite = false;
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
            }
        }
        private async void RemoveFromFavShopItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
          "Are you sure to remove " + ProductManager.ShopRightSelectedItem.Nom + " From List Favory",
          "Remove Shop from list favory");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.FalseFavShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
            }
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
                ProductManager.PageSource1 = typeof(Favorite);
                i = 0;
                Frame.Navigate(typeof(Product), new List<object> { i, ProductManager.CatRightSelectedItem.ListProduit, ProductManager.CatRightSelectedItem.Nom });


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
                Frame.Navigate(typeof(Favorite), null);
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
                Frame.Navigate(typeof(Favorite), null);

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

                Frame.Navigate(typeof(Favorite), null);

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
                            StorageFile copiedFile = await ProductManager.ProFile.CopyAsync(localFolder, ImgName);
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
        private async void AddShopItem_Click(object sender, RoutedEventArgs e)
        {
            Dialog1.Title = "Add Shop to " + ProductManager.ProRightSelectedItem.Nom;
            SuppGrids1();
            ProductManager.ShopFile = null;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ShopImage.Source = img;
            aig1 = true;
            AddShopRemar.Text = String.Empty;
            panel1.Children.Add(GridAddShop);
            Dialog1.PrimaryButtonClick += delegate
            {
                ProductManager.PageSource = typeof(Favorite);
                Frame.Navigate(typeof(Shop), ProductManager.ProRightSelectedItem.ListMagasin);
            };


            Dialog1.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog1.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void ChangImgProItem_Click(object sender, RoutedEventArgs e)
        {
            Dialog1.Title = "Change image to " + ProductManager.ProRightSelectedItem.Nom;
            SuppGrids1();
            ProductManager.ProFile = null;
            ChangeProImgRemar.Text = String.Empty;
            aig1 = true;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ProImage1.Source = img;
            panel1.Children.Add(GridChangProImg);
            Dialog1.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
            };


            Dialog1.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog1.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void RemoProItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog1 = new Windows.UI.Popups.MessageDialog(
              "Are you sure to remove " + ProductManager.ProRightSelectedItem.Nom,
              "Remove Product");

            dialog1.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog1.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog1.DefaultCommandIndex = 0;
            dialog1.CancelCommandIndex = 1;

            var result = await dialog1.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.SupProduit(ProductManager.ListCat, ProductManager.ProRightSelectedItem.Nom, ProductManager.GetNomCatFromPr(ProductManager.ListCat, ProductManager.ProRightSelectedItem.Nom));
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
            }

        }
        private async void RenamProItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids1();
            panel1.Children.Add(GridRenamPro);
            Dialog1.Title = "Rename " + ProductManager.ProRightSelectedItem.Nom;
            RenamProRemar.Text = String.Empty;
            var btn = sender as Button;
            if (aig1)
            {
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamPro.Margin = TopMargin;
            }
            Dialog1.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);

            };


            Dialog1.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog1.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void RemoveFromFavProItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog1 = new Windows.UI.Popups.MessageDialog(
              "Are you sure to remove " + ProductManager.ProRightSelectedItem.Nom + " from list favory",
              "Remove from list favory");

            dialog1.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog1.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog1.DefaultCommandIndex = 0;
            dialog1.CancelCommandIndex = 1;

            var result = await dialog1.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.ProRightSelectedItem.Favorite = false;
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
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
        private void SuppGrids1()
        {

            if (GridRenamPro != null) panel1.Children.Remove(GridRenamPro);
            if (GridAddShop != null) panel1.Children.Remove(GridAddShop);
            if (GridChangProImg != null) panel1.Children.Remove(GridChangProImg);

        }

        private void NomShop_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddShopRemar.Text = String.Empty;
        }

        private void Prix_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddShopRemar.Text = String.Empty;
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
                    bitmapImage.DecodePixelHeight = 190;
                    bitmapImage.DecodePixelWidth = 190;
                    ProImage1.Source = bitmapImage;
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
        private async void RemoShopItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog2 = new Windows.UI.Popups.MessageDialog(
             "Are you sure to remove " + ProductManager.ShopRightSelectedItem.Nom,
             "Remove Product");

            dialog2.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog2.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });



            dialog2.DefaultCommandIndex = 0;
            dialog2.CancelCommandIndex = 1;

            var result = await dialog2.ShowAsync();
            if (String.Equals(result.Label, "Yes"))
            {
                ProductManager.RemoveShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);
            }
        }
        private void SuppGrids2()
        {

            if (GridRenamShop != null) panel2.Children.Remove(GridRenamShop);
            if (GridChangShopImg != null) panel2.Children.Remove(GridChangShopImg);
            if (GridChangePrice != null) panel2.Children.Remove(GridChangePrice);
            if (GridAddProToShop != null) panel2.Children.Remove(GridAddProToShop);

        }
        private async void RenamShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids2();
            panel2.Children.Add(GridRenamShop);
            if (aig2)
            {

                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamShop.Margin = TopMargin;
            }
            Dialog2.Title = "Rename " + ProductManager.ShopRightSelectedItem.Nom;
            RenamShopRemar.Text = String.Empty;
            NewNomShop.Text = String.Empty;
            var btn = sender as Button;
            Dialog2.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shops), null);

            };


            Dialog2.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog2.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void ChangImgShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids2();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
            ShopImage1.Source = img;
            panel2.Children.Add(GridChangShopImg);
            aig2 = true;
            Dialog2.Title = "Change Image to " + ProductManager.ShopRightSelectedItem.Nom;
            ChangeShopImgRemar.Text = String.Empty;
            var btn = sender as Button;
            Dialog2.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Favorite), null);

            };


            Dialog2.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog2.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }

        private async void AddProToShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids2();
            panel2.Children.Add(GridAddProToShop);
            Dialog2.Title = "Add Product to  " + ProductManager.ShopRightSelectedItem.Nom;
            AddProToShopRemar.Text = String.Empty;
            if (aig2)
            {

                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridAddProToShop.Margin = TopMargin;
            }
            var btn = sender as Button;
            Dialog2.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Favorite);
                i = 1;
                Frame.Navigate(typeof(Product), new List<object> { i, ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem), ProductManager.ShopRightSelectedItem.Nom });


            };


            Dialog2.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog2.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private async void ChangPriceShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids2();
            panel2.Children.Add(GridChangePrice);
            Dialog2.Title = "Change Price to " + ProductManager.ShopRightSelectedItem.Nom;
            ChangePriceRemar.Text = String.Empty;
            if (aig2)
            {

                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridChangePrice.Margin = TopMargin;
            }
            var btn = sender as Button;
            Dialog2.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Favorite);
                i = 1;
                Frame.Navigate(typeof(Product), new List<object> { i, ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem), ProductManager.ShopRightSelectedItem.Nom });


            };


            Dialog2.SecondaryButtonClick += delegate
            {

            };

            // Show Dialog
            var result = await Dialog2.ShowAsync();
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

        private async void ChooseShopImg1_Click(object sender, RoutedEventArgs e)
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
                    ShopImage1.Source = bitmapImage;
                }

            }

        }

        private async void ChangeShopImg_Click(object sender, RoutedEventArgs e)
        {
            if (ProductManager.ShopFile != null)
            {
                string imgNam = "Shop" + (ProductManager.GetAllMag(ProductManager.ListCat).Count).ToString() + ".jpg";
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                ProductManager.ChangeImgShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom, imgNam);
                StorageFile copiedFile = await ProductManager.ShopFile.CopyAsync(localFolder, imgNam, NameCollisionOption.ReplaceExisting);

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
                ProductManager.ChangePriceToProduct(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom, ComboPro.SelectedItem as string, ProductManager.StringToFloat(Prix1.Text));
                ChangePriceRemar.Text = "Price Changed";
            }
        }

        private void Prix1_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            ChangePriceRemar.Text = String.Empty;

            string TEXT = String.Empty;


            for (int i = 0; i < Prix1.Text.Length; i++)
            {
                if (ProductManager.IsChiffr(Prix1.Text[i])) TEXT = String.Concat(TEXT, Prix1.Text[i]);
            }

            Prix1.Text = TEXT;
        }

        private void ComboPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePriceRemar.Text = String.Empty;
        }

        private void ComboPro_Loaded(object sender, RoutedEventArgs e)
        {
            var Combo = sender as ComboBox;
            if (Combo.Items.Count == 0) Combo.ItemsSource = ProductManager.GetNomProsFromListPr(ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem));



            if (Combo.Items.Count == 0) Combo.ItemsSource = new List<string>() { "No Product" };
            Combo.SelectedIndex = 0;
        }

        private void ComboAddPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddProToShopRemar.Text = String.Empty;
        }

        private void ComboAddPro_Loaded(object sender, RoutedEventArgs e)
        {
            var Combo = sender as ComboBox;
            if (Combo.Items.Count == 0) Combo.ItemsSource = ProductManager.GetNomProsinNotMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem);



            if (Combo.Items.Count == 0) Combo.ItemsSource = new List<string>() { "No Product" };
            Combo.SelectedIndex = 0;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

            ProductManager.AjNouvMagasin(ProductManager.ListCat, ComboAddPro.SelectedItem as string, ProductManager.ShopRightSelectedItem.Nom, ProductManager.StringToFloat(PrixPro.Text), ProductManager.GetNomCatFromPr(ProductManager.ListCat, ComboAddPro.SelectedItem as string), ProductManager.ShopRightSelectedItem.Favorite, ProductManager.ShopRightSelectedItem.ImageName);
            AddProToShopRemar.Text = "Product Added";
        }

        private void PrixPro_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            AddProToShopRemar.Text = String.Empty;
            string TEXT = String.Empty;


            for (int i = 0; i < PrixPro.Text.Length; i++)
            {
                if (ProductManager.IsChiffr(PrixPro.Text[i])) TEXT = String.Concat(TEXT, PrixPro.Text[i]);
            }

            PrixPro.Text = TEXT;
        }

    }
}
