using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using My_Shopping.Class;
using System.Threading.Tasks;
using My_Shopping.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace My_Shopping.Views
{
    public sealed partial class Shops : Page
    {
        bool aig = false;
        List<Magasin> ListMag = new List<Magasin>();
        public Shops()
        {

            this.InitializeComponent();

            ListMag = ProductManager.GetAllMag(ProductManager.ListCat);
            ProductManager.MiseAjourMagasin(ProductManager.ListCat, ListMag);
            AppShell.TextBar.Text = "All Shops";
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProductManager.PageSource1 = typeof(Shops);
            Frame.Navigate(typeof(Product), new List<object> { ProductManager.GetListPrFromMag(ProductManager.ListCat, (Magasin)e.ClickedItem), (Magasin)e.ClickedItem });

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


            var ListSugget = ProductManager.GetNomShops(ProductManager.GetAllMag(ProductManager.ListCat));
            var List = ListSugget.ToArray();

            var result = List.Where(x => x.StartsWith(text)).ToArray();
            return result;
        }

        private void FindBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var Selected = args.ChosenSuggestion as string;
            
            if (ProductManager.GetNomShops(ProductManager.GetAllMag(ProductManager.ListCat)).Contains(Selected))
            {
                sender.Text = String.Empty;
                ProductManager.PageSource1 = typeof(Shops);
                
                Frame.Navigate(typeof(Product), new List<object> { ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.GetShopFromNom(ProductManager.ListCat, Selected)), ProductManager.GetShopFromNom(ProductManager.ListCat, Selected) } );
                
            }
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
                ProductManager.RemoveShop(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom);
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shops), null);
            }
        }
        private void SuppGrids()
        {

            if (GridRenamShop != null) panel.Children.Remove(GridRenamShop);
            if (GridChangShopImg != null) panel.Children.Remove(GridChangShopImg);
            if (GridChangePrice != null) panel.Children.Remove(GridChangePrice);
            if (GridAddProToShop != null) panel.Children.Remove(GridAddProToShop);

        }
        private async void RenamShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridRenamShop);
            if (aig)
            {

                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRenamShop.Margin = TopMargin;
            }
            Dialog.Title = "Rename " + ProductManager.ShopRightSelectedItem.Nom;
            RenamShopRemar.Text = String.Empty;
            NewNomShop.Text = String.Empty;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shops), null);

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
                ProductManager.Sauvgarder();
                Frame.Navigate(typeof(Shops), null);

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

                    ProductManager.Sauvgarder();
                    Frame.Navigate(typeof(Favorite), null);
                }

            }
        }
        private async void AddProToShopItem_Click(object sender, RoutedEventArgs e)
        {
            SuppGrids();
            panel.Children.Add(GridAddProToShop);
            Dialog.Title = "Add Product to  " + ProductManager.ShopRightSelectedItem.Nom;
            AddProToShopRemar.Text = String.Empty;
            if (aig)
            {

                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridAddProToShop.Margin = TopMargin;
            }
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Favorite);
                Frame.Navigate(typeof(Product), ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem));

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
                ProductManager.Sauvgarder();
                ProductManager.PageSource1 = typeof(Favorite);
                Frame.Navigate(typeof(Product), ProductManager.GetListPrFromMag(ProductManager.ListCat, ProductManager.ShopRightSelectedItem));

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
                ProductManager.ChangePriceToProduct(ProductManager.ListCat, ProductManager.ShopRightSelectedItem.Nom, ComboPro.SelectedItem as string, ProductManager.StringToFloat(Prix.Text));
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
