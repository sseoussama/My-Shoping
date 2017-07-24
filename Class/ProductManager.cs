using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;
using System.Threading.Tasks;
using System.IO;

namespace My_Shopping.Class
{
    class ProductManager
    {
        public static Produit ProRightSelectedItem;
        public static Categorie CatRightSelectedItem;
        public static Magasin ShopRightSelectedItem;
        public static Type PageSource1;
        public static Type PageSource;
        public static StorageFile ProFile;
        public static List<Categorie> ListCat = new List<Categorie>();
        public static object GetModifed;
        public static Produit PruitModi;
        public static Char[] TabChiffr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
        public static StorageFile CatFile;

        public static StorageFile ShopFile;
        public static string PathImage = "ms-appx:///Image/";
        public static string Fin = "*****************";
        public static bool IsChiffr(char Car)
        {
            bool aig = false;
            for (int i = 0; i < TabChiffr.Length; i++)
            {
                if (TabChiffr[i] == Car) aig = true;
            }

            return aig;
        }
        public static bool Exist(List<Produit> listpr, string NomProduit)
        {
            bool aig = false;
            foreach (var Elm in listpr)
            {
                if (string.Equals(Elm.Nom, NomProduit))
                { aig = true; break; }

            }
            return aig;
        }

        public static bool Exist(Categorie Cat, string NomProduit)
        {
            bool aig = false;
            var ListPr = Cat.ListProduit;
            foreach (var item in ListPr)
            {
                if (string.Equals(item.Nom, NomProduit))
                {
                    aig = true; break;
                }
            }
            return aig;
        }
        public static bool Exist(List<Magasin> listmg, string NomMagasin)
        {
            bool aig = false;
            foreach (var Elm in listmg)
            {
                if (string.Equals(Elm.Nom, NomMagasin))
                { aig = true; break; }

            }
            return aig;
        }

        public static bool Exist(List<Categorie> listcat, string NomCategorie)
        {
            bool aig = false;
            foreach (var Elm in listcat)
            {
                if (string.Equals(Elm.Nom, NomCategorie))
                { aig = true; break; }

            }
            return aig;
        }
        private static bool Exist(Categorie Cat, List<Magasin> ListMg)
        {
            bool aig = false;


            var ListPr = Cat.ListProduit;
            foreach (var item in ListPr)
            {
                var ListMag = item.ListMagasin;
                if (ListMag == ListMg)
                {
                    aig = true; break;
                }


            }



            return aig;
        }
        public static bool Exist(Produit Pr, string NomMag)
        {
            bool aig = false;
            var ListMag = Pr.ListMagasin;
            foreach (var item in ListMag)
            {
                if (string.Equals(item.Nom, NomMag))
                {
                    aig = true;
                }
            }
            return aig;
        }

        public static void AjNouvMagasin(List<Categorie> listCat, string NomProduit, string NomMagasin, float Prix, string NomCategorie, bool MagFav, string ImageName)
        {
            int i;
            var Listpr = new List<Produit>();
            var ListMagasin = new List<Magasin>();
            for (i = 0; i < listCat.Count; i++)
            {
                if (string.Equals(listCat[i].Nom, NomCategorie)) { Listpr = listCat[i].ListProduit; break; }

            }

            for (i = 0; i < Listpr.Count; i++)
            {
                if (string.Equals(Listpr[i].Nom, NomProduit)) { ListMagasin = Listpr[i].ListMagasin; break; }

            }

            if (!Exist(ListMagasin, NomMagasin))
            {
                var magasin = new Magasin(NomMagasin, Prix, MagFav, ImageName);
                ListMagasin.Add(magasin);
            }
            else { }
        }
        public static void AjNouvProduit(List<Categorie> listCat, string NomProduit, string NomCategorie, bool PrFav, string ImageName)
        {
            int i;
            for (i = 0; i < listCat.Count; i++)
            {
                if (string.Equals(listCat[i].Nom, NomCategorie)) break;

            }
            var listpr = listCat[i].ListProduit;
            if (!Exist(listpr, NomProduit))
            {

                var ListMagasin = new List<Magasin>();
                var produit = new Produit(NomProduit, ListMagasin, PrFav, ImageName);
                listpr.Add(produit);
            }
            else { }
        }
        public static void RenameShop(List<Categorie> listCat, string NomShop, string NewNomShop)
        {
            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    var ListMag = item.ListMagasin;
                    foreach (var item1 in ListMag)
                    {
                        if (String.Equals(item1.Nom, NomShop)) item1.Nom = NewNomShop;
                    }
                }
            }
        }
        public static void RemoveShop(List<Categorie> listCat, string NomShop)
        {
            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    var ListMag = item.ListMagasin;
                    for (int j = 0; j < ListMag.Count; j++)
                    {
                        if (string.Equals(ListMag[j].Nom, NomShop))
                        {
                            ListMag.RemoveAt(j);

                        }

                    }
                }
            }
        }



        public static void ChangeImgShop(List<Categorie> listCat, string NomShop, string ImgName)
        {
            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    var ListMag = item.ListMagasin;
                    foreach (var item1 in ListMag)
                    {
                        if (String.Equals(item1.Nom, NomShop)) item1.ImageName = ImgName;
                    }
                }
            }
        }
        public static void ChangePriceToProduct(List<Categorie> listCat, string NomShop, string NomPro, float Prix)
        {

            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    if (String.Equals(NomPro, item.Nom))
                    {
                        var ListMag = item.ListMagasin;
                        foreach (var item1 in ListMag)
                        {
                            if (String.Equals(item1.Nom, NomShop)) item1.Prix = Prix;

                        }
                    }
                }
            }
        }
        public static void AjCategorie(List<Categorie> listCat, string NomCategorie, bool CatFav, string ImageName)
        {
            if (!Exist(listCat, NomCategorie))
            {

                var ListProduit = new List<Produit>();
                var categorie = new Categorie(NomCategorie, ListProduit, CatFav, ImageName);

                listCat.Add(categorie);
            }
            else { }

        }
        public static void SupProduit(List<Categorie> listCat, string NomProduit, string NomCategorie)
        {
            int i;
            for (i = 0; i < listCat.Count; i++)
            {
                if (string.Equals(listCat[i].Nom, NomCategorie)) break;

            }
            var Listpr = listCat[i].ListProduit;



            for (i = 0; i < Listpr.Count; i++)
            {
                if (string.Equals(Listpr[i].Nom, NomProduit))
                {
                    var ListMagain = Listpr[i].ListMagasin;
                    ListMagain.Clear();
                    Listpr.RemoveAt(i);
                    break;
                }
            }
        }



        public static void SupMagasin(List<Categorie> listCat, string NomProduit, string NomMagasin, string NomCategorie)
        {
            int i;
            for (i = 0; i < listCat.Count; i++)
            {
                if (string.Equals(listCat[i].Nom, NomCategorie)) break;

            }
            var Listpr = listCat[i].ListProduit;
            for (i = 0; i < Listpr.Count; i++)
            {
                if (string.Equals(Listpr[i].Nom, NomProduit)) break;

            }
            var ListMagasin = Listpr[i].ListMagasin;

            for (int j = 0; j < ListMagasin.Count; j++)
            {
                if (string.Equals(ListMagasin[j].Nom, NomMagasin))
                {
                    ListMagasin.RemoveAt(j);
                    break;
                }
            }


        }

        public static void SupCategorie(List<Categorie> listCat, string NomCategorie)
        {
            int i;
            for (i = 0; i < listCat.Count; i++)
            {
                if (string.Equals(listCat[i].Nom, NomCategorie)) break;

            }
            var ListPr = listCat[i].ListProduit;
            ListPr.Clear();
            listCat.RemoveAt(i);

        }
        public static int GetIndCat(List<Categorie> List, Categorie cat)
        {
            int i = 0;
            for (i = 0; i < List.Count; i++)
            {
                if (List[i] == cat) break;
            }
            return i + 1;
        }
        public static int GetIndPro(List<Produit> List, Produit Pro)
        {
            int i = 0;
            for (i = 0; i < List.Count; i++)
            {
                if (List[i] == Pro) break;
            }
            return i + 1;
        }
        public static int GetIndShop(List<Magasin> List, Magasin Shop)
        {
            int i = 0;
            for (i = 0; i < List.Count; i++)
            {
                if (List[i] == Shop) break;
            }
            return i + 1;
        }
        private static void Permute(object a, object b)
        {
            object c;
            c = a;
            a = b;
            b = c;
        }
        public static void Tri(List<Magasin> ListMag)
        {
            int i;
            for (i = 0; i < ListMag.Count; i++)
            {


                Permute(ListMag[i], ListMag[IndPetit(ListMag, i, ListMag.Count)]);

            }
        }
        private static int IndPetit(List<Magasin> List, int a, int b)
        {

            int inter = a;
            for (int i = a + 1; i <= b; i++)
            {
                if (List[i].Prix < List[inter].Prix) inter = i;
            }
            return inter;

        }
        public static List<Produit> GetListPrFromListMag(List<Categorie> ListCat, List<Magasin> ListMag)
        {
            bool Arre = false;
            List<Produit> ListPr = new List<Produit>();
            foreach (var item in ListCat)
            {
                var ListProduit = item.ListProduit;
                foreach (var val in ListProduit)
                {
                    if (val.ListMagasin == ListMag)
                    {
                        ListPr = ListProduit; Arre = true; break;
                    }

                }
                if (Arre) break;
            }
            return ListPr;
        }
        public static List<string> GetNomProsFromListPr(List<Produit> ListPr)
        {
            List<string> List = new List<string>();
            foreach (var Pr in ListPr)
            {
                List.Add(Pr.Nom);
            }
            return List;
        }
        public static List<string> GetNomProsinNotMag(List<Categorie> ListCat, Magasin mag)
        {
            List<string> List = new List<string>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var var in ListPr)
                {
                    if (!Exist(var, mag.Nom)) List.Add(var.Nom);
                }
            }
            return List;
        }
        public static Produit GetProFromNom(List<Categorie> ListCat, string NomPro)
        {
            bool Arre = false;
            var Produit = new Produit();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var val in ListPr)
                {
                    if (String.Equals(NomPro, val.Nom))
                    {
                        Produit = val; Arre = true; break;
                    }
                }
                if (Arre) break;
            }
            return Produit;
        }
        public static Magasin GetShopFromNom(List<Categorie> ListCat, string NomShop)
        {
            var Shop = new Magasin();
            foreach (var val in ListCat)
            {
                var ListPr = val.ListProduit;
                foreach (var value in ListPr)
                {
                    var ListMag = value.ListMagasin;
                    foreach (var item in ListMag)
                    {
                        if (String.Equals(NomShop, item.Nom))
                        {
                            Shop = item; break;
                        }
                    }
                }

            }

            return Shop;
        }
        public static string GetNomCatFromPr(List<Categorie> ListCat, string NomPro)
        {
            string value = String.Empty;

            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var val in ListPr)
                {
                    if (String.Equals(NomPro, val.Nom)) { value = item.Nom; break; }

                }
            }
            return value;
        }
        public static void MiseAjourProduit(List<Categorie> ListCat, List<Produit> ListPr)
        {
            string NomCat = String.Empty; ;
            foreach (var item in ListPr)
            {
                NomCat = GetNomCatFromPr(ListCat, item.Nom);
                item.NomCat = NomCat;
            }
        }
        public static List<Categorie> GetCatFav(List<Categorie> ListCat)
        {
            List<Categorie> List = new List<Categorie>();
            foreach (var item in ListCat)
            {
                if (item.Favorite) List.Add(item);
            }
            return List;

        }
        public static List<Produit> GetPrFav(List<Categorie> ListCat)
        {
            List<Produit> ListFav = new List<Produit>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Val in ListPr)
                {
                    if (Val.Favorite) ListFav.Add(Val);
                }

            }
            return ListFav;

        }
        public static List<Magasin> GetMagFav(List<Categorie> ListCat)
        {
            List<Magasin> ListFav = new List<Magasin>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Val in ListPr)
                {
                    var ListMag = Val.ListMagasin;
                    foreach (var val in ListMag)
                    {
                        if (val.Favorite) ListFav.Add(val);
                    }
                }

            }
            return ListFav;

        }
        public static List<Produit> GetAllProduct(List<Categorie> ListCat)

        {
            List<Produit> ListPr = new List<Produit>();
            foreach (var item in ListCat)
            {
                var Listpr = item.ListProduit;
                foreach (var val in Listpr)
                {
                    ListPr.Add(val);
                }

            }
            return ListPr;
        }
        public static List<Magasin> GetAllMag(List<Categorie> ListCat)
        {
            List<Magasin> List = new List<Magasin>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var val in ListPr)
                {
                    var ListMag = val.ListMagasin;
                    foreach (var Mag in ListMag)
                    {
                        if (!Exist(List, Mag.Nom)) List.Add(Mag);
                    }

                }
            }
            return List;
        }
        public static void FalseFavShop(List<Categorie> ListCat, Magasin Shop)
        {

            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    var ListMag = item.ListMagasin;
                    foreach (var item1 in ListMag)
                    {
                        if (String.Equals(item1.Nom, Shop.Nom)) item1.Favorite = false;
                    }
                }
            }


        }
        public static void TrueFavShop(List<Categorie> ListCat, Magasin Shop)
        {

            foreach (var Elm in ListCat)
            {
                var ListPr = Elm.ListProduit;
                foreach (var item in ListPr)
                {
                    var ListMag = item.ListMagasin;
                    foreach (var item1 in ListMag)
                    {
                        if (String.Equals(item1.Nom, Shop.Nom)) item1.Favorite = true;
                    }
                }
            }


        }
        public static List<Magasin> GetAllFavShop(List<Categorie> ListCat)
        {
            var List = new List<Magasin>();
            var list = ProductManager.GetAllMag(ListCat);
            foreach (var item in list)
            {
                if (item.Favorite == true) List.Add(item);
            }
            return List;
        }
        public static Categorie GetCategorieFromMag(List<Categorie> ListCat, List<Magasin> ListMg)
        {
            Categorie Categorie = null;
            foreach (var item in ListCat)
            {
                if (Exist(item, ListMg))
                {
                    Categorie = item; break;
                }
            }
            return Categorie;
        }
        public static List<Produit> GetListPrFromMag(List<Categorie> ListCat, Magasin mag)
        {
            List<Produit> List = new List<Produit>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Pr in ListPr)
                {
                    if (Exist(Pr, mag.Nom)) List.Add(Pr);
                }



            }
            return List;
        }
        public static List<Produit> GetListPrFromNomPro(List<Categorie> ListCat, string NomPro)
        {
            List<Produit> List = new List<Produit>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Pr in ListPr)
                {

                    if (String.Equals(NomPro, Pr.Nom))
                    {
                        List = ListPr; break;
                    }
                }



            }
            return List;
        }
        public static List<Magasin> GetListMagFromNomMag(List<Categorie> ListCat, string NomMagasin)
        {
            List<Magasin> List = new List<Magasin>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Pr in ListPr)
                {
                    var ListMag = Pr.ListMagasin;
                    foreach (var Mag in ListMag)
                    {
                        if (String.Equals(Mag.Nom, NomMagasin))
                        {
                            List = ListMag; break;
                        }
                    }
                }



            }
            return List;
        }
        public static void MiseAjourMagasin(List<Categorie> ListCat, List<Magasin> ListMag)
        {
            foreach (var item in ListMag)
            {

                item.NbProduit = GetListPrFromMag(ListCat, item).Count;
            }
        }
        public static void MiseAjourProduit(List<Produit> ListPr)
        {
            foreach (var item in ListPr)
            {
                item.PrixToString = String.Empty;
            }
        }
        public static void MiseAjourProduit(List<Categorie> ListCat, List<Produit> ListPr, string ShopName)
        {

            foreach (var item in ListPr)
            {
                foreach (var Cat in ListCat)
                {
                    var ListProd = Cat.ListProduit;
                    foreach (var Pro in ListProd)
                    {
                        if (String.Equals(item.Nom, Pro.Nom))
                        {
                            var ListMag = item.ListMagasin;
                            foreach (var Shop in ListMag)
                            {
                                if (String.Equals(Shop.Nom, ShopName)) item.Prix = Shop.Prix;
                            }
                        }
                    }
                }
            }
        }
        public static Categorie GetCatFromNom(List<Categorie> ListCat, string NomCat)
        {
            Categorie Cat = new Categorie();
            foreach (var item in ListCat)
            {
                if (String.Equals(NomCat, item.Nom))
                {
                    Cat = item;
                    break;
                }
            }
            return Cat;
        }

        public static float StringToFloat(string value)
        {


            if (value.Contains("."))
                value = value.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            return float.Parse(value);


        }

        public static void Sauvgarder()
        {


            string CatValue = String.Empty;
            string MagValue = String.Empty;
            string ProduitValue = String.Empty;


            if (ProductManager.ListCat != null)
            {
                foreach (var Cat in ListCat)
                {

                    CatValue += Cat.Nom + "\r\n" + Cat.Favorite.ToString() + "\r\n" + Cat.ImageName + "\r\n";
                    var ListProduit = Cat.ListProduit;
                    foreach (var Produit in ListProduit)
                    {
                        ProduitValue += Produit.Nom + "\r\n" + Produit.Favorite.ToString() + "\r\n" + Produit.ImageName + "\r\n";
                        var ListMagasin = Produit.ListMagasin;
                        foreach (var Magasin in ListMagasin)
                        {
                            MagValue += Magasin.Nom + "\r\n" + Magasin.Prix.ToString() + "\r\n" + Magasin.Favorite.ToString() + "\r\n" + Magasin.ImageName + "\r\n";

                        }
                        MagValue += Fin + "\r\n";



                    }
                    ProduitValue += Fin + "\r\n";



                }
                CatValue += Fin + "\r\n";

                File.WriteAllText("Categorie.txt", CatValue, Encoding.ASCII);
                File.WriteAllText("Produit.txt", ProduitValue, Encoding.ASCII);
                File.WriteAllText("Magasin.txt", MagValue, Encoding.ASCII);




            }
            else { }

        }
        private static bool GetBollFav(string value)
        {
            bool aig = false;
            if (String.Equals("True", value)) aig = true;

            return aig;
        }
        public static List<string> GetNomCats(List<Categorie> ListCat)
        {
            List<string> List = new List<string>();
            foreach (var item in ListCat)
            {
                List.Add(item.Nom);
            }
            return List;
        }
        public static string GetNomCatFromListPr(List<Categorie> ListCat, List<Produit> ListPr)
        {
            string NomCat = String.Empty;
            foreach (var item in ListCat)
            {
                var List = item.ListProduit;
                if (List == ListPr)
                {
                    NomCat = item.Nom; break;
                }
            }
            return NomCat;
        }
        public static string GetNomProFromListMag(List<Categorie> ListCat, List<Magasin> ListMag)
        {
            bool arre = false;
            string NomPro = String.Empty;
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var Val in ListPr)
                {
                    var List = Val.ListMagasin;
                    if (List == ListMag)
                    {
                        NomPro = Val.Nom; arre = true; break;
                    }
                }
                if (arre) break;

            }
            return NomPro;
        }
        public static List<string> GetNomShops(List<Magasin> ListMag)
        {
            List<string> List = new List<string>();
            foreach (var item in ListMag)
            {
                List.Add(item.Nom);
            }
            return List;
        }
        public static List<string> GetNomPros(List<Categorie> ListCat)
        {
            List<string> List = new List<string>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var var in ListPr)
                {
                    List.Add(var.Nom);
                }
            }
            return List;
        }
        public static List<string> GetNomProsFromCat(List<Categorie> ListCat, Categorie Cat)
        {
            List<string> List = new List<string>();
            var ListPr = Cat.ListProduit;
            foreach (var item in ListPr)
            {
                List.Add(item.Nom);
            }
            return List;
        }
        public static Produit GetProFromMag(List<Categorie> ListCat, string NomMag)
        {
            var Pro = new Produit();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var val in ListPr)
                {
                    var ListMag = val.ListMagasin;
                    foreach (var value in ListMag)
                    {
                        if (String.Equals(NomMag, value.Nom))
                        {
                            Pro = val; break;
                        }
                    }
                }
            }
            return Pro;
        }
        public static List<string> GetNomShopsFromPro(List<Categorie> ListCat, Produit Produit)
        {
            List<string> List = new List<string>();
            var ListMag = Produit.ListMagasin;
            foreach (var item in ListMag)
            {
                List.Add(item.Nom);
            }
            return List;
        }
        public static List<string> GetNomShops(List<Categorie> ListCat)
        {
            List<string> List = new List<string>();
            foreach (var item in ListCat)
            {
                var ListPr = item.ListProduit;
                foreach (var var in ListPr)
                {
                    var ListShop = var.ListMagasin;
                    foreach (var value in ListShop)
                    {
                        List.Add(value.Nom);
                    }
                }
            }
            return List;
        }
        public static void Restaurer()
        {
           
            string[] CatValue = File.ReadAllLines("Categorie.txt", Encoding.ASCII);
            string[] ProduitValue = File.ReadAllLines("Produit.txt", Encoding.ASCII);
            string[] MagValue = File.ReadAllLines("Magasin.txt", Encoding.ASCII);

            int i;
            int j = 0;
            int k = 0;
            bool CatFav;
            bool PrFav;
            bool MagFav;

            for (i = 0; i < CatValue.Length; i += 3)
            {
                if (String.Equals(CatValue[i], ProductManager.Fin)) break;
                CatFav = GetBollFav(CatValue[i + 1]);
                AjCategorie(ListCat, CatValue[i], CatFav, CatValue[i + 2]);
                while (j < ProduitValue.Length)
                {
                    if (String.Equals(ProduitValue[j], ProductManager.Fin)) { j++; break; }
                    PrFav = GetBollFav(ProduitValue[j + 1]);
                    AjNouvProduit(ListCat, ProduitValue[j], CatValue[i], PrFav, ProduitValue[j + 2]);
                    while (k < MagValue.Length)
                    {
                        if (String.Equals(MagValue[k], ProductManager.Fin)) { k++; break; }
                        MagFav = GetBollFav(MagValue[k + 2]);
                        AjNouvMagasin(ListCat, ProduitValue[j], MagValue[k], StringToFloat(MagValue[k + 1]), CatValue[i], MagFav, MagValue[k + 3]);
                        k += 4;
                    }
                    j += 3;
                }

            }

        }
    }
}
