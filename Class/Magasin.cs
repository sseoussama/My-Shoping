using Windows.System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace My_Shopping.Class
{
    class Magasin
    {
        private Point _point;
        public Point Point { get { return _point; } set { _point = value; } }
        private string _nom;

        private float _prix;
        public Magasin()
        {

        }
        public Magasin(string NomMagasin, float Prix, bool MagFav, string ImageName)
        {
            _nom = NomMagasin;
            _prix = Prix;

            _PrixToString = Prix.ToString() + " $";
            _favorite = MagFav;
            _imageName = ImageName;
        }

        private string _imageName;
        public string ImageName { get { return _imageName; } set { _imageName = value; } }
        public string Image { get { return ProductManager.PathImage + _imageName; } }
        private int _nbProduit;
        public int NbProduit { get { return _nbProduit; } set { _nbProduit = value; _nbProduitToString = _nbProduit.ToString() + " Products"; } }
        private string _nbProduitToString;
        public string NbProduitTostring { get { return _nbProduitToString; } }
        private bool _favorite;
        private string _PrixToString;
        public string PrixToString { get { return _PrixToString; } }
        public bool Favorite { get { return _favorite; } set { _favorite = value; } }
        public string Nom { get { return _nom; } set { _nom = value; } }
        public float Prix { get { return _prix; } set { _prix = value; _PrixToString = _prix.ToString() + " $"; } }
    }
}
