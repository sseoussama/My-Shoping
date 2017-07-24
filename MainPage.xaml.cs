using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System.Net;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using My_Shopping.Controls;
using Windows.Storage;
using My_Shopping.Class;
using Windows.UI;
using Windows.Devices.Geolocation;
using My_Shopping.Views;
using Windows.UI.ViewManagement;

namespace My_Shopping
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell : Page
    {

        public static int i = 0;
        private bool Modifed = false;
        public static TextBlock TextBar = new TextBlock();
    

        // Declare the top level nav items

        private List<NavMenuItem> navlist = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    Symbol = Symbol.Home,
                    Label = "categories ",
                    DestinationPage = typeof(Views.Category)

                },
                new NavMenuItem()
                {
                    Symbol = Symbol.List,
                    Label = "Products",
                    DestinationPage = typeof(Views.Products)
                },
                 new NavMenuItem()
                {
                    Symbol = Symbol.Shop,
                    Label = "shops",
                    DestinationPage = typeof(Views.Shops)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Favorite,
                    Label = "Favorite",
                    DestinationPage = typeof(Views.Favorite)
                },


            });

        public static AppShell Current = null;

        /// <summary>
        /// Initializes a new instance of the AppShell, sets the static 'Current' reference,
        /// adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        /// provide the nav menu list with the data to display.
        /// </summary>
        public AppShell()
        {



            this.InitializeComponent();



            this.SuppGrids();

            PanelBar.Children.Add(TextBar);
            TextBar.FontSize = 20;
            TextBar.TextAlignment = TextAlignment.Center;
            this.Loaded += (sender, args) =>
            {
                Current = this;


                this.TogglePaneButton.Focus(FocusState.Programmatic);
            };



            // If on a phone device that has hardware buttons then we hide the app's back button.

            ProductManager.Restaurer();
            NavMenuList.ItemsSource = navlist;
            NavMenuList.SelectedItem = NavMenuList.Items[0];

        }

        public Frame AppFrame { get { return this.frame; } }

        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;



            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }





        private void FindButton_Click(object sender, RoutedEventArgs e)
        {

        }




        #region BackRequested Handlers





        #endregion
        #region Settings



        #endregion

        #region Navigation

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private async void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            if (NavMenuList.SelectedItem == NavMenuList.Items[0]) NavMenuList.SelectedItem = NavMenuList.Items[0];
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item == NavMenuList.Items[0])
            {


                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            if (item != null)
            {
                if (item.DestinationPage != null)
                {
                    if (item.DestinationPage == typeof(Uri))
                    {
                        // Grab the URL from the argument
                        Uri url = null;
                        if (Uri.TryCreate(item.Arguments as string, UriKind.Absolute, out url))
                        {
                            await Launcher.LaunchUriAsync(url);
                        }
                    }
                    else if (item.DestinationPage != this.AppFrame.CurrentSourcePageType)
                    {
                        this.AppFrame.Navigate(item.DestinationPage, item.Arguments);

                    }
                }
            }
            currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in navlist where p.DestinationPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && this.AppFrame.BackStackDepth > 0)
                {

                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in this.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in navlist where p.DestinationPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }

        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;
            }



        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
            this.CheckTogglePaneButtonSizeChanged();
        }

        #endregion

        public Rect TogglePaneButtonRect
        {
            get;
            private set;
        }


        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// Callback when the SplitView's Pane is toggled open or close.  When the Pane is not visible
        /// then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {

            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            if (handler != null)
            {
                // handler(this, this.TogglePaneButtonRect);
                handler.DynamicInvoke(this, this.TogglePaneButtonRect);
            }

        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }



        }


        private async void ButtonAddMessageDialog_Click(object sender, RoutedEventArgs e)
        {

            Dialog.Title = "Add";
            this.SuppGrids();

            Ra1.IsChecked = false;
            Ra2.IsChecked = false;
            Ra3.IsChecked = false;

            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                btn.Content = "Result: OK";
                btn.Content = "Result: NONE";
                if (ProductManager.GetModifed is Categorie)
                { ProductManager.Sauvgarder(); AppFrame.Navigate(typeof(Category), null); }
                if (ProductManager.GetModifed is Produit)
                {
                    ProductManager.Sauvgarder();
                    ProductManager.PageSource1 = typeof(Category);
                    AppFrame.Navigate(typeof(Product), (ProductManager.GetCatFromNom(ProductManager.ListCat, ProductManager.GetNomCatFromPr(ProductManager.ListCat, (ProductManager.GetModifed as Produit).Nom)).ListProduit));

                }
                if (ProductManager.GetModifed is Magasin) { ProductManager.Sauvgarder(); ProductManager.PageSource = typeof(Product); ProductManager.PageSource1 = typeof(Category); AppFrame.Navigate(typeof(Shop), ProductManager.PruitModi.ListMagasin); }


            };


            Dialog.SecondaryButtonClick += delegate
            {
                btn.Content = "Result: Cancel";

            };

            // Show Dialog
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }

        }
        private async void ButtonDelateMessageDialog_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Title = "Delate";
            this.SuppGrids();

            Ra1.IsChecked = false;
            Ra2.IsChecked = false;
            Ra3.IsChecked = false;
            var btn = sender as Button;
            Dialog.PrimaryButtonClick += delegate
            {
                btn.Content = "Result: OK";
                btn.Content = "Result: NONE";
                if (ProductManager.GetModifed is List<Categorie>)
                { ProductManager.Sauvgarder(); AppFrame.Navigate(typeof(Category), null); }
                if (ProductManager.GetModifed is List<Produit>) { ProductManager.Sauvgarder(); ProductManager.PageSource1 = typeof(Category); AppFrame.Navigate(typeof(Product), ProductManager.GetModifed as List<Produit>); }
                if (ProductManager.GetModifed is List<Magasin>) { ProductManager.Sauvgarder(); ProductManager.PageSource = typeof(Product); ProductManager.PageSource1 = typeof(Category); AppFrame.Navigate(typeof(Shop), ProductManager.GetModifed as List<Magasin>); }


            };


            Dialog.SecondaryButtonClick += delegate
            {
                btn.Content = "Result: Cancel";

            };

            // Show Dialog
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {

            }
        }
        private void SuppGrids()
        {

            if (GridRemovePro != null) panel.Children.Remove(GridRemovePro);
            if (GridAddCat != null) panel.Children.Remove(GridAddCat);
            if (GridAddPro != null) panel.Children.Remove(GridAddPro);
            if (GridAddShop != null) panel.Children.Remove(GridAddShop);
            if (GridRemoveCat != null) panel.Children.Remove(GridRemoveCat);
            if (GridRemoveShop != null) panel.Children.Remove(GridRemoveShop);


        }
        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            if (String.Equals(Dialog.Title, "Add"))
            {
                Thickness Margin = Ra2.Margin;
                ProductManager.CatFile = null;
                this.SuppGrids();
                BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
                CatImage.Source = img;
                panel.Children.Add(this.GridAddCat);
                Thickness TopMargin = new Thickness(0, 60, 0, 0);
                GridAddCat.Margin = TopMargin;
                AddCatRemar.Text = String.Empty; NomCat.Text = String.Empty;
            }
            if (String.Equals(Dialog.Title, "Delate"))
            {
                this.SuppGrids();

                panel.Children.Add(GridRemoveCat);
                Dialog.Height = 850;
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRemoveCat.Margin = TopMargin;
                DelCatRemar.Text = String.Empty;
            }

        }
        private void Product_Checked(object sender, RoutedEventArgs e)
        {
            if (String.Equals(Dialog.Title, "Add"))
            {
                ProductManager.ProFile = null;
                this.SuppGrids();
                BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
                ProImage.Source = img;

                panel.Children.Add(this.GridAddPro);
                Thickness TopMargin = new Thickness(0, 60, 0, 0);
                GridAddPro.Margin = TopMargin;
                NomPro.Text = String.Empty;
                AddProRemar.Text = String.Empty;
            }
            if (String.Equals(Dialog.Title, "Delate"))
            {
                this.SuppGrids();
                panel.Children.Add(GridRemovePro);
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRemovePro.Margin = TopMargin;
                DelProRemar.Text = String.Empty;
            }

        }

        private void Shop_Checked(object sender, RoutedEventArgs e)
        {
            if (String.Equals(Dialog.Title, "Add"))
            {

                ProductManager.ShopFile = null;
                BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.png"));
                ShopImage.Source = img;
                this.SuppGrids();
                panel.Children.Add(this.GridAddShop);
                Thickness TopMargin = new Thickness(0, 60, 0, 0);
                GridAddShop.Margin = TopMargin;
                NomShop.Text = String.Empty;
                AddShopRemar.Text = String.Empty;
            }
            if (String.Equals(Dialog.Title, "Delate"))
            {
                this.SuppGrids();
                panel.Children.Add(GridRemoveShop);
                Thickness TopMargin = new Thickness(0, 80, 0, 0);
                GridRemoveShop.Margin = TopMargin;
                DelShopRemar.Text = String.Empty;
            }

        }






        private async void AddCatButton_Click(object sender, RoutedEventArgs e)
        {
            var bol = false;
            var NomImg = String.Empty;
            if (ToggleCat.IsOn) bol = true;
            if (String.Equals(NomCat.Text, String.Empty)) AddCatRemar.Text = "No text ";
            else
            {
                if (String.Equals(NomCat.Text, "No CATEGORY")) AddCatRemar.Text = "Name invalid ";
                else
                {
                    if (ProductManager.Exist(ProductManager.ListCat, NomCat.Text)) AddCatRemar.Text = "Category Already exists";
                    else
                    {
                        if (ProductManager.CatFile != null)
                        {

                            NomImg = "Cat" + (ProductManager.ListCat.Count).ToString() + ".jpg";
                            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                            StorageFile copiedFile = await ProductManager.CatFile.CopyAsync(localFolder, NomImg, NameCollisionOption.ReplaceExisting);
                        }


                        ProductManager.AjCategorie(ProductManager.ListCat, NomCat.Text, bol, NomImg);
                        AddCatRemar.Text = "Category Added";
                        ProductManager.CatFile = null;

                        ProductManager.GetModifed = ProductManager.GetCatFromNom(ProductManager.ListCat, NomCat.Text);

                    }
                }

            }
        }
        private async void AddProButton_Click(object sender, RoutedEventArgs e)
        {
            var bol = false;
            var NomImg = String.Empty;
            if (TogglePro.IsOn) bol = true;
            if (String.Equals(NomPro.Text, String.Empty)) AddProRemar.Text = "No text ";
            else
            {
                if (String.Equals(ComboCatAddPro.SelectedItem as string, "No Category")) AddProRemar.Text = "No Category";
                else
                {
                    if (String.Equals(NomPro.Text, "No Product")) AddProRemar.Text = "Name invalid ";
                    else
                    {
                        if (ProductManager.Exist(ProductManager.GetCatFromNom(ProductManager.ListCat, ComboCatAddPro.SelectedItem as string), NomPro.Text)) AddProRemar.Text = "Product Already exists";
                        else
                        {
                            if (ProductManager.ProFile != null)
                            {
                                NomImg = "Pro" + (ProductManager.GetAllProduct(ProductManager.ListCat).Count).ToString() + ".jpg";
                                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");
                                StorageFile copiedFile = await ProductManager.ProFile.CopyAsync(localFolder, NomImg, NameCollisionOption.ReplaceExisting);
                            }

                            ProductManager.AjNouvProduit(ProductManager.ListCat, NomPro.Text, ComboCatAddPro.SelectedItem as string, bol, NomImg);
                            AddProRemar.Text = "Product Added";
                            ProductManager.ProFile = null;

                            ProductManager.GetModifed = ProductManager.GetProFromNom(ProductManager.ListCat, NomPro.Text);

                        }
                    }

                }

            }
        }
        private async void AddShopButton_Click(object sender, RoutedEventArgs e)
        {
            var bol = false;
            var NomImg = String.Empty;
            if (ToggleShop.IsOn) bol = true;
            if (String.Equals(NomShop.Text, String.Empty)) AddShopRemar.Text = "No text ";
            else
            {
                if (String.Equals(Prix.Text, String.Empty)) AddShopRemar.Text = "No Prix";
                else
                {
                    if (String.Equals(ComboProAddShop.SelectedItem as string, "No Product")) AddShopRemar.Text = "No Product in " + ComboCatAddShop.SelectedItem as string;
                    else
                    {
                        if (String.Equals(NomShop.Text, "No Shop")) AddShopRemar.Text = "Name invalid ";
                        if (ProductManager.Exist((ProductManager.GetProFromNom(ProductManager.ListCat, ComboProAddShop.SelectedItem as string)), NomShop.Text)) AddShopRemar.Text = "Shop Already exist";
                        else
                        {

                            if (ProductManager.ShopFile != null)
                            {
                                NomImg = "Shop" + (ProductManager.GetAllMag(ProductManager.ListCat).Count).ToString() + ".jpg";
                                StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");

                                StorageFile copiedFile = await ProductManager.ShopFile.CopyAsync(localFolder, NomImg, NameCollisionOption.ReplaceExisting);
                            }

                            ProductManager.AjNouvMagasin(ProductManager.ListCat, ComboProAddShop.SelectedItem as string, NomShop.Text, ProductManager.StringToFloat(Prix.Text), ComboCatAddShop.SelectedItem as string, bol, NomImg);
                            AddShopRemar.Text = "Shop Added";
                            ProductManager.ShopFile = null;
                            ProductManager.PruitModi = ProductManager.GetProFromNom(ProductManager.ListCat, ComboProAddShop.SelectedItem as string);
                            ProductManager.GetModifed = ProductManager.GetShopFromNom(ProductManager.ListCat, NomShop.Text);
                        }
                    }

                }

            }

        }
        private async void DelateCatButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(ComboCatDelCat.SelectedItem as string, "No Category")) DelCatRemar.Text = "No Category";
            else
            {

                if (String.Equals(ProductManager.GetCatFromNom(ProductManager.ListCat, ComboCatDelCat.SelectedItem as string).ImageName, String.Empty))
                {


                }
                else
                {
                    StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Directory.GetCurrentDirectory() + "\\image");




                }


            }
            DelCatRemar.Text = "Category Delated";
            ProductManager.GetModifed = ProductManager.ListCat;
            ProductManager.SupCategorie(ProductManager.ListCat, ComboCatDelCat.SelectedItem as string);

        }
        private void DelateProButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(ComboCatDelPro.SelectedItem as string, "No Category")) DelProRemar.Text = "No Category";
            else
            {
                if (String.Equals(ComboProDelPro.SelectedItem as string, "No Product")) DelProRemar.Text = "No Product in " + ComboCatDelPro.SelectedItem as string;
                else
                {
                    ProductManager.GetModifed = ProductManager.GetListPrFromNomPro(ProductManager.ListCat, ComboProDelPro.SelectedItem as string);
                    ProductManager.SupProduit(ProductManager.ListCat, ComboProDelPro.SelectedItem as string, ComboCatDelPro.SelectedItem as string);
                    DelProRemar.Text = "Product Delated";


                }
            }
        }

        private void DelateShopButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.Equals(ComboCatDelShop.SelectedItem as string, "No Category")) DelShopRemar.Text = "No Category";
            else
            {
                if (String.Equals(ComboProDelShop.SelectedItem as string, "No Product")) DelShopRemar.Text = "No Product in " + ComboCatDelShop.SelectedItem as string;
                else
                {
                    if (String.Equals(ComboShopDelShop.SelectedItem as string, "No Shop")) DelShopRemar.Text = "No Shop in " + ComboProDelShop.SelectedItem as string;
                    else
                    {
                        ProductManager.SupMagasin(ProductManager.ListCat, ComboProDelShop.SelectedItem as string, ComboShopDelShop.SelectedItem as string, ComboCatDelShop.SelectedItem as string);
                        DelShopRemar.Text = "Shop Delated";
                        ProductManager.GetModifed = ProductManager.GetProFromNom(ProductManager.ListCat, ComboProDelShop.SelectedItem as string).ListMagasin;

                    }
                }
            }
        }
        private void NomCat_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            this.AddCatRemar.Text = String.Empty;
        }



        private void NomPro_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            this.AddProRemar.Text = String.Empty;
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
        private void NomShop_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            this.AddShopRemar.Text = String.Empty;
        }

        private void ComboCat_Loaded(object sender, RoutedEventArgs e)
        {
            var Combo = sender as ComboBox;
            if (Combo.Items.Count == 0) Combo.ItemsSource = ProductManager.GetNomCats(ProductManager.ListCat);


            if (Combo.Items.Count == 0) Combo.ItemsSource = new List<string>() { "No Category" };
            Combo.SelectedIndex = 0;
        }
        private void ComboPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Combo = sender as ComboBox;
            if (ComboProAddShop == Combo) AddShopRemar.Text = String.Empty;
            if (Combo == ComboProDelShop)
            {
                DelShopRemar.Text = String.Empty;
                if (!Modifed) this.ChangeComboShopDelShop();
                Modifed = false;
            }
            if (ComboProDelPro == Combo) DelProRemar.Text = String.Empty;


        }
        private void ComboShop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DelShopRemar.Text = String.Empty;
        }
        private void ComboCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Combo = sender as ComboBox;
            if (Combo == ComboCatAddPro) AddProRemar.Text = String.Empty;
            if (ComboCatDelCat == Combo) DelCatRemar.Text = String.Empty;
            if (Combo == ComboCatAddShop)
            {
                AddShopRemar.Text = String.Empty;
                ComboProAddShop.ItemsSource = ProductManager.GetNomProsFromCat(ProductManager.ListCat, ProductManager.GetCatFromNom(ProductManager.ListCat, ComboCatAddShop.SelectedItem as string));
                if (ComboProAddShop.Items.Count == 0)
                {
                    ComboProAddShop.ItemsSource = new List<String>() { "No Product" };
                }
                ComboProAddShop.SelectedIndex = 0;
            }
            if (Combo == ComboCatDelPro)
            {
                DelProRemar.Text = String.Empty;
                ComboProDelPro.ItemsSource = ProductManager.GetNomProsFromCat(ProductManager.ListCat, ProductManager.GetCatFromNom(ProductManager.ListCat, ComboCatDelPro.SelectedItem as string));
                if (ComboProDelPro.Items.Count == 0)
                {
                    ComboProDelPro.ItemsSource = new List<String>() { "No Product" };
                }
                ComboProDelPro.SelectedIndex = 0;
            }
            if (Combo == ComboCatDelShop)
            {
                DelShopRemar.Text = String.Empty;
                this.ChangeComboProDelShop();

                this.ChangeComboShopDelShop();

            }

        }
        private void ChangeComboProDelShop()
        {
            if (ComboCatDelShop.SelectedItem as string != "No Category")
            {
                Modifed = true;
                ComboProDelShop.ItemsSource = ProductManager.GetNomProsFromCat(ProductManager.ListCat, ProductManager.GetCatFromNom(ProductManager.ListCat, ComboCatDelShop.SelectedItem as string));
                if (ComboProDelShop.Items.Count == 0)
                {
                    ComboProDelShop.ItemsSource = new List<String>() { "No Product" };
                }

            }
            else
            {
                ComboProDelShop.ItemsSource = new List<String>() { "No Product" };
            }
            ComboProDelShop.SelectedIndex = 0;

        }
        private void ChangeComboShopDelShop()
        {
            if (ComboProDelShop.SelectedItem as string != "No Product")
            {
                ComboShopDelShop.ItemsSource = ProductManager.GetNomShopsFromPro(ProductManager.ListCat, ProductManager.GetProFromNom(ProductManager.ListCat, ComboProDelShop.SelectedItem as string));
                if (ComboShopDelShop.Items.Count == 0)
                {
                    ComboShopDelShop.ItemsSource = new List<String>() { "No Shop" };
                }
            }
            else ComboShopDelShop.ItemsSource = new List<String>() { "No Shop" };

            ComboShopDelShop.SelectedIndex = 0;

        }

        private async void ChooseCatImage_Click(object sender, RoutedEventArgs e)
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

        private async void ChooseShopImage_Click(object sender, RoutedEventArgs e)
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
