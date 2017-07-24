using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shopping.Class
{
    class Categorie
    {
        public Categorie()
        {

        }
        public Categorie(string NomCategorie, List<Produit> ListProduit, bool CatFav, string ImageName)
        {
            _nom = NomCategorie;
            _listProduit = ListProduit;
            _favorite = CatFav;
            _imageName = ImageName;
            _image = ProductManager.PathImage + _imageName;
        }
        private string _imageName;

        private string _image;
        public string ImageName { get { return _imageName; } set { _imageName = value; _image = ProductManager.PathImage + _imageName; } }
        public string Image { get { return _image; } }
        private bool _favorite;
        public bool Favorite { get { return _favorite; } set { _favorite = value; } }
        private string _nom;
        private List<Produit> _listProduit;
        public string Nom { get { return _nom; } set { _nom = value; } }
        public List<Produit> ListProduit { get { return _listProduit; } set { _listProduit = value; } }
    }
}
