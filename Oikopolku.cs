using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyo
{
    class Oikopolku
    {
        //alkuperäinen käsitys oli, että pitää tehdä useammasta lajisa,
        //mutta onneksi Jukka sanoi koodiputkassa ettei tarvitse tehdä kuin yhdestä lajista.
        //joten koodin kauniimmaksi saantia ajatellen ajateltiin, että 
        //laitellaan toistuvat elementit, pitkät polut yms, omaan classiin.
        //Mutta, kyllähän tämä pelkällä jalkapallollakin toimii?

        public int kotimaali;
        public int vierasmaali;
        public int kotijoukkuelaskuri;
        public int vierasjoukkuelaskuri;
        public int i = 0;
        public int j;
        public string paivamaara = DateTime.Now.ToString("MMM dd yyyy"); //käytetää tätäpäivää
        public string kellonaika = DateTime.Now.ToString("hh:mm"); //käytetää tätäkellonaikaa


        //Jalkapallo
        public string lyhyt = @"..\..\Teams\Jalkapallo\";
        //public string lyhyt2 = @"..\..\..\Teams\Jalkapallo\";

        //Varsinainenpeliaika
        public double minuutti;
        public double sekuntti;
        //Lisäaika
        public double lisasekuntti = 1;
        //Tapahtumat
        public string imagepolku = @"..\..\Teams\";

      
    }

}
