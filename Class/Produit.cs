using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shopping.Class
{
    class Produit
    {
        public Produit()
        {

        }
        public Produit(string NomProduit, List<Magasin> ListMagasin, bool PrFav, string ImageName)
        {
            _nom = NomProduit;
            _listeMagasin = ListMagasin;
            _favorite = PrFav;

            _imageName = ImageName;
        }
        private string _imageName;
        public string ImageName { get { return _imageName; } set { _imageName = value; } }
        public string Image { get { return ProductManager.PathImage + _imageName; } }
        private string _nomCat;
        public string NomCat { get { return _nomCat; } set { _nomCat = value; } }
        private string _nom;
        private bool _favorite;
        public bool Favorite { get { return _favorite; } set { _favorite = value; } }
        private List<Magasin> _listeMagasin;
        public string Nom { get { return _nom; } set { _nom = value; } }
        private float _prix;
        private string _PrixToString;
        public string PrixToString { get { return _PrixToString; } set { _PrixToString = value; } }
        public float Prix { get { return _prix; } set { _prix = value; _PrixToString = _prix.ToString() + " $"; } }

        public List<Magasin> ListMagasin { get { return _listeMagasin; } set { _listeMagasin = value; } }
    }
}
