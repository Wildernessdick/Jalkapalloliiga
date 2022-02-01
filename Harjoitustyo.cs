using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Media;

namespace Harjoitustyo
{
    public partial class Harjoitustyo : Form
    {

        bool edit = false;
        int sellecetdroW = -1;

        private Oikopolku oikopolku = new Oikopolku();
        public List<Ottelu> ottelu = new List<Ottelu>();
        private List<Pelaaja> Pelaajanhallinta;
        private List<Joukkuetilastot> joukkuetilastot;
        private List<Ottelutilastot> ottelutilastot;
        private List<Pelaajatilastot> pelaajatilastot;
        DataTable koitos = new DataTable();


        public Harjoitustyo()
        {
            InitializeComponent();
            LataaDgw();
        }

        //Ottelu välilehti
        private void btnAloita_Click(object sender, EventArgs e)
        {//Aloittaa ottelun

            btnLisaa.Enabled = false;
            btnMuokkaa.Enabled = false;
            btnTallenna.Enabled = false;
            tmrAika.Enabled = true;
            btnTauko.Enabled = true;
            btnAloita.Enabled = false;
            btnKotitapahtuma.Enabled = false;
            cbKotijoukkue.Enabled = false;
            cbVierasjoukkue.Enabled = false;
            btnKelaus.Enabled = false;
            btnVierastapahtuma.Enabled = false;
            btnAloita.Text = "Aloita";
            if (oikopolku.minuutti < 90)
            {
                tmrLisaaika.Stop();
            }

        }

        private void btnTauko_Click(object sender, EventArgs e)
        {//Kun ottelussa tulee tapahtuma, tämän merkitsemisen aika on myös lisäajan kerrytys

            tmrAika.Stop();
            btnTauko.Enabled = false;
            btnKotitapahtuma.Enabled = true;
            btnVierastapahtuma.Enabled = true;
            btnAloita.Enabled = true;
            cbKotipelaaja.Enabled = true;
            cbKotitapahtuma.Enabled = true;
            cbViearaspelaaja.Enabled = true;
            cbVierastapahtuma.Enabled = true;
            btnAloita.Text = "Jatka";
            tbKotiaika.Text = oikopolku.minuutti.ToString() + ":" + oikopolku.sekuntti.ToString();
            tbViearasaika.Text = tbKotiaika.Text;
            tmrLisaaika.Start();
        }

        private void btnKelaus_Click(object sender, EventArgs e)
        {//Hyppää ottelun loppuun

            oikopolku.minuutti = 89;
            oikopolku.sekuntti = 50;
            oikopolku.lisasekuntti = 10;
            labOtteluaika.Text = "89";
            labEra.Text = "Erä 2";
        }
        private void btnkorjaa_Click(object sender, EventArgs e)
        {
           
         
            PoistaKaikki();
        }
        private void btnKotitapahtuma_Click(object sender, EventArgs e)
        {//Lisää kotijoukkueelle tapahtuma

          

            koitos.Rows.Add(tbKotiaika.Text, cbKotijoukkue.Text, cbKotipelaaja.Text, cbKotitapahtuma.Text);
            dgvOttelu.DataSource = koitos;

            if (cbKotitapahtuma.SelectedItem.ToString() == "Maali")
            {
                oikopolku.kotimaali++;
                labKotimaali.Text = oikopolku.kotimaali.ToString();


                for (int i = 0; i < pelaajatilastot.Count; i++)
                {                    
                    if (pelaajatilastot[i].snimi == cbKotipelaaja.Text)
                    {
   
                        pelaajatilastot[i].maalit = pelaajatilastot[i].maalit + 1;
 
                        pelaajatilastot[i].pisteet = pelaajatilastot[i].pisteet + 3;
                    }

                }

            }
            if (cbKotitapahtuma.SelectedItem.ToString() == "Syöttäjä")
            {
                for (int i = 0; i < pelaajatilastot.Count; i++)
                {
                    if (pelaajatilastot[i].snimi == cbKotipelaaja.Text)
                    {
                        pelaajatilastot[i].syotot = pelaajatilastot[i].syotot + 1;
                        pelaajatilastot[i].pisteet = pelaajatilastot[i].pisteet + 1;
                    }
                }
            }
            

        }

        private void btnVierastapahtuma_Click(object sender, EventArgs e)
        {//Lisää vierasjoukkueelle tapahtuma
            btnkorjaa.Enabled = true;
            dgvOttelu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            koitos.Rows.Add(tbViearasaika.Text, cbVierasjoukkue.Text, cbViearaspelaaja.Text, cbVierastapahtuma.Text);

            if (cbVierastapahtuma.SelectedItem.ToString() == "Maali")
            {
                oikopolku.vierasmaali++;
                labVierasmaali.Text = oikopolku.vierasmaali.ToString();
                for (int i = 0; i < pelaajatilastot.Count; i++)
                {
                    if (pelaajatilastot[i].snimi == cbViearaspelaaja.Text)
                    {

                        pelaajatilastot[i].maalit = pelaajatilastot[i].maalit + 1;

                        pelaajatilastot[i].pisteet = pelaajatilastot[i].pisteet + 3;
                    }

                }

            }
            if (cbVierastapahtuma.SelectedItem.ToString() == "Syöttäjä")
            {
                
                    for (int i = 0; i < pelaajatilastot.Count; i++)
                    {
                        if (pelaajatilastot[i].snimi == cbViearaspelaaja.Text)
                        {
                            pelaajatilastot[i].syotot = pelaajatilastot[i].syotot + 1;
                            pelaajatilastot[i].pisteet = pelaajatilastot[i].pisteet + 1;
                        }
                    }
                
            }

            dgvOttelu.DataSource = koitos;
            dgtPelaajatilastot.DataSource = pelaajatilastot;     
            }

        private void cbKotijoukkue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbKotijoukkue.SelectedItem.ToString() == "Arsenal")
            {
                cbVierasjoukkue.Text = "Savonia";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(oikopolku.lyhyt + "Arsenal.xml");
                this.cbKotipelaaja.DataSource = dataSet.Tables[0];
                this.cbKotipelaaja.DisplayMember = "Snimi";
                btnAloita.Enabled = true;
                btnKelaus.Enabled = true;
                btnLisaa.Enabled = false;
                btnMuokkaa.Enabled = false;
                btnTallenna.Enabled = false;
                btnPoista.Enabled = false;
            }
            if (cbKotijoukkue.SelectedItem.ToString() == "Savonia")
            {
                cbVierasjoukkue.Text = "Arsenal";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(oikopolku.lyhyt + "Savonia.xml");
                this.cbKotipelaaja.DataSource = dataSet.Tables[0];
                this.cbKotipelaaja.DisplayMember = "Snimi";
                btnAloita.Enabled = true;
                btnKelaus.Enabled = true;

            }
            switch (cbKotijoukkue.SelectedItem.ToString().Trim())
            {
                case "Arsenal":
                    pbKotijoukkue.Image = Image.FromFile(oikopolku.lyhyt + "ArsenalPieni.png");
                    break;
                case "Savonia":
                    pbKotijoukkue.Image = Image.FromFile(oikopolku.lyhyt + "SavoniaPieni.png");
                    break;

            }
            cbKotipelaaja.SelectedIndex = 1;
            
            cbViearaspelaaja.SelectedIndex = 1;
            cbVierastapahtuma.SelectedIndex = 1;
            cbKotitapahtuma.SelectedIndex = 1;
        }

        private void cbVierasjoukkue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVierasjoukkue.SelectedItem.ToString() == "Arsenal")
            {
                cbKotijoukkue.Text = "Savonia";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(oikopolku.lyhyt + "Arsenal.xml");
                this.cbViearaspelaaja.DataSource = dataSet.Tables[0];
                this.cbViearaspelaaja.DisplayMember = "Snimi";

            }
            if (cbVierasjoukkue.SelectedItem.ToString() == "Savonia")
            {
                cbKotijoukkue.Text = "Arsenal";
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(oikopolku.lyhyt + "Savonia.xml");
                this.cbViearaspelaaja.DataSource = dataSet.Tables[0];
                this.cbViearaspelaaja.DisplayMember = "Snimi";
            }
            switch (cbVierasjoukkue.SelectedItem.ToString().Trim())
            {
                case "Arsenal":
                    pbVierasjoukkue.Image = Image.FromFile(oikopolku.lyhyt + "ArsenalPieni.png");
                    break;
                case "Savonia":
                    pbVierasjoukkue.Image = Image.FromFile(oikopolku.lyhyt + "SavoniaPieni.png");
                    break;

            }
        }
        //Muokkaa välilähti
        public void SXML(List<Pelaaja> input)
        {
            string polku = " ";

            if (cbJoukkue.SelectedItem.ToString() == "Arsenal")
            {
                polku = oikopolku.lyhyt + "Arsenal.xml";
            }
            if (cbJoukkue.SelectedItem.ToString() == "Savonia")
            {
                polku = oikopolku.lyhyt + "Savonia.xml";
            }
            System.Xml.Serialization.XmlSerializer serializer =
            new System.Xml.Serialization.XmlSerializer(input.GetType());
            StreamWriter ar = new StreamWriter(polku);
            serializer.Serialize(ar, input);
            ar.Close();

        }
        public List<Pelaaja> DeSXML()
        {
            string polku = " ";
            if (cbJoukkue.SelectedItem.ToString() == "Arsenal")
            {
                polku = oikopolku.lyhyt + "Arsenal.xml";
            }
            if (cbJoukkue.SelectedItem.ToString() == "Savonia")
            {
                polku = oikopolku.lyhyt + "Savonia.xml";
            }

            if (File.Exists(polku))
            {
                StreamReader stream = new StreamReader(polku);
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(List<Pelaaja>));
                object obj = ser.Deserialize(stream);
                stream.Close();
                return (List<Pelaaja>)obj;
            }
            else
                return null;

        }

        private void btnLisaa_Click(object sender, EventArgs e)
        {//Lisää pelaaja

            tabValikko.SelectedIndex = 1;
            edit = false;
            Null();
        }

        private void btnMuokkaa_Click(object sender, EventArgs e)
        {//Muokkaa pelaaja

            Pelaaja pelaaja = Pelaajanhallinta[sellecetdroW];
            cbJoukkue.Text = pelaaja.joukkue;
            tbNimi.Text = pelaaja.nimi;
            tbSnimi.Text = pelaaja.snimi;
            tbNumero.Text = pelaaja.pelaajanumero;
            cbPelipaikka.Text = pelaaja.pelipaikka;
            edit = true;
        }

        private void btnTallenna_Click(object sender, EventArgs e)
        {//Tallenna pelaaja
            Pelaaja pelaaja;

            try
            {
                pelaaja.joukkue = cbJoukkue.Text;
                pelaaja.nimi = tbNimi.Text;
                pelaaja.snimi = tbSnimi.Text;
                pelaaja.pelaajanumero = tbNumero.Text;
                pelaaja.pelipaikka = cbPelipaikka.Text;

                if (edit == false)
                {
                    Pelaajanhallinta.Add(pelaaja);
                }
                else
                {
                    Pelaajanhallinta[sellecetdroW] = pelaaja;
                }
                SXML(Pelaajanhallinta);
                dgvPelaajat.DataSource = null;
                dgvPelaajat.DataSource = Pelaajanhallinta;
                edit = false;

                Null();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvPelaajat_SelectionChanged(object sender, EventArgs e)
        {
            sellecetdroW = dgvPelaajat.CurrentRow.Index;
        }
        void Null()
        {
            cbJoukkue.Text = "";
            tbSnimi.Text = "";
            tbSnimi.Text = "";
            tbNumero.Text = "";
            cbPelipaikka.Text = "";
        }

        private void btnPoista_Click(object sender, EventArgs e)
        {
            sellecetdroW = dgvPelaajat.CurrentRow.Index;

            if (sellecetdroW >= 0)
            {
                Pelaajanhallinta.RemoveAt(sellecetdroW);
                SXML(Pelaajanhallinta);
                dgvPelaajat.DataSource = null;
                dgvPelaajat.DataSource = Pelaajanhallinta;
                            }
        }
        private void cbJoukkue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbJoukkue.SelectedItem.ToString() == "Arsenal")
            {
                pbHahmo.Image = Image.FromFile(oikopolku.lyhyt + "aPelaaja.png");
                pbJoukkue.Image = Image.FromFile(oikopolku.lyhyt + "ArsenalPieni.png");

                if (File.Exists(oikopolku.lyhyt + "Arsenal.xml"))
                {
                    Pelaajanhallinta = DeSXML();
                    dgvPelaajat.DataSource = Pelaajanhallinta;
                }
                else
                {
                    Pelaajanhallinta = new List<Pelaaja>();
                }
            }
            if (cbJoukkue.SelectedItem.ToString() == "Savonia")
            {
                pbHahmo.Image = Image.FromFile(oikopolku.lyhyt + "sPelaaja.png");
                pbJoukkue.Image = Image.FromFile(oikopolku.lyhyt + "SavoniaPieni.png");
                if (File.Exists(oikopolku.lyhyt + "Savonia.xml"))
                {
                    Pelaajanhallinta = DeSXML();
                    dgvPelaajat.DataSource = Pelaajanhallinta;
                }
                else
                {
                    Pelaajanhallinta = new List<Pelaaja>();
                }
            }
        }
        //Tilastot välilehti
        private void btnTallennaTilastot_Click(object sender, EventArgs e)
        {//Tallentaa kaikki tilastot

            SXMLJoukkuetilastot(joukkuetilastot);
            dgwTilastot.DataSource = null;
            dgwTilastot.DataSource = joukkuetilastot;

            SXMLOttelutilastot(ottelutilastot);
            dgwPelatutottelut.DataSource = null;
            dgwPelatutottelut.DataSource = ottelutilastot;

            SerializeXMPelaajatilastot(pelaajatilastot);
            dgtPelaajatilastot.DataSource = null;
            dgtPelaajatilastot.DataSource = pelaajatilastot;
        }

        private void Harjoitustyo_SizeChanged(object sender, EventArgs e)
        {
            this.pnlOttelu.Left = (this.Width - pnlOttelu.Width) / 2;
            this.dgvOttelu.Left = this.pnlOttelu.Left; //käynnissä olevan ottelun tilastot

            this.dgtPelaajatilastot.Left = this.pnlOttelu.Left; //tilastohistoria
            this.dgwTilastot.Left = this.pnlOttelu.Left;
            this.dgwPelatutottelut.Left = this.pnlOttelu.Left;

            this.pnlPelaaja.Left = this.pnlOttelu.Left; //Joukkueen / pelaajan edit
            this.dgvPelaajat.Left = this.pnlOttelu.Left;

            this.btnkorjaa.Left = this.Width / 2;
            this.btnTallennaTilastot.Left = this.Width / 2;



        }
        //Peliaika
        private void tmrAika_Tick(object sender, EventArgs e)
        {
            if (oikopolku.sekuntti < 60)
            {
                oikopolku.sekuntti++;
            }
            if (oikopolku.sekuntti == 60)
            {
                oikopolku.sekuntti = 00;
                oikopolku.minuutti++;
            }
            if (oikopolku.minuutti == 45 && oikopolku.sekuntti == 00)
            {
                tmrAika.Stop();
                labEra.Text = "Erä 2";
                btnAloita.Enabled = true;
            }
            if (oikopolku.minuutti == 90 && oikopolku.sekuntti == 00)
            {
                tmrAika.Stop();

                btnAloita.Enabled = true;
            }
            if (oikopolku.minuutti == 90 && oikopolku.lisasekuntti > 0)
            {

                oikopolku.lisasekuntti--;

                labEra.Text = "Lisäaika";
                labOtteluaika.Text = oikopolku.lisasekuntti.ToString() + "sec";
            }
            if (oikopolku.lisasekuntti == 0)
            {
                tmrAika.Stop();
                tmrLisaaika.Stop();

                labOtteluaika.Text = oikopolku.lisasekuntti.ToString() + "sec";
                btnLisaa.Enabled = true;
                btnMuokkaa.Enabled = true;
                btnTallenna.Enabled = true;
                btnTauko.Enabled = false;
                btnAloita.Enabled = false;

                OtteluTilastot();
                JoukkueTilastot();
                btnTallenna.PerformClick();


                {
                    btnUusi.Enabled = true;
                    btnLisaa.Enabled = true;
                    btnMuokkaa.Enabled = true;
                    btnTallenna.Enabled = true;
                    btnPoista.Enabled = true;
                }
            }
            labOtteluaika.Text = oikopolku.minuutti.ToString() + ":" + oikopolku.sekuntti.ToString();


        }
        //Merkkienestely
        private void tbNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbSnimi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }
        private void LataaDgw()
        {
            cbJoukkue.SelectedIndex = 0;
            pnlOttelu.BackgroundImage = Image.FromFile(oikopolku.imagepolku + "tausta.png");
            pnlPelaaja.BackgroundImage = Image.FromFile(oikopolku.imagepolku + "tausta.png");

            koitos.Columns.Add("Aika", typeof(string));
            koitos.Columns.Add("Joukkue", typeof(string));
            koitos.Columns.Add("Pelaaja", typeof(string));
            koitos.Columns.Add("Tapahtuma", typeof(string));

            joukkuetilastot = DeSXMLJoukkuetilastot();
            dgwTilastot.DataSource = joukkuetilastot;
            ottelutilastot = DeSXMLOttelutilastot();
            dgwPelatutottelut.DataSource = ottelutilastot;
            pelaajatilastot = DeSXMLpelaajatilastot();
            dgtPelaajatilastot.DataSource = pelaajatilastot;


        }
        public List<Pelaajatilastot> DeSXMLpelaajatilastot()
        {
            string polku = oikopolku.lyhyt + "Pelaajatilasot.xml";


            if (File.Exists(polku))
            {
                StreamReader stream = new StreamReader(polku);
                System.Xml.Serialization.XmlSerializer ser =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Pelaajatilastot>));
                object obj = ser.Deserialize(stream);
                stream.Close();
                return (List<Pelaajatilastot>)obj;
            }
            else
                return null;

        }
        public List<Joukkuetilastot> DeSXMLJoukkuetilastot()
        {
            string polku = oikopolku.lyhyt + "Joukkuetilastot.xml";


            if (File.Exists(polku))
            {
                StreamReader stream = new StreamReader(polku);
                System.Xml.Serialization.XmlSerializer ser =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Joukkuetilastot>));
                object obj = ser.Deserialize(stream);
                stream.Close();
                return (List<Joukkuetilastot>)obj;
            }
            else
                return null;

        }
        public List<Ottelutilastot> DeSXMLOttelutilastot()
        {
            string polku = oikopolku.lyhyt + "Ottelutilastot.xml";


            if (File.Exists(polku))
            {
                StreamReader stream = new StreamReader(polku);
                System.Xml.Serialization.XmlSerializer ser =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Ottelutilastot>));
                object obj = ser.Deserialize(stream);
                stream.Close();
                return (List<Ottelutilastot>)obj;
            }
            else
                return null;

        }
        public void SXMLJoukkuetilastot(List<Joukkuetilastot> input)
        {
            string Joukkuetilastot = oikopolku.lyhyt + "Joukkuetilastot.xml";
            System.Xml.Serialization.XmlSerializer jot =
            new System.Xml.Serialization.XmlSerializer(input.GetType());
            StreamWriter jotilastot = new StreamWriter(Joukkuetilastot);
            jot.Serialize(jotilastot, input);
            jotilastot.Close();
        }
        public void SXMLOttelutilastot(List<Ottelutilastot> input)
        {
            string Ottelutilastot = oikopolku.lyhyt + "Ottelutilastot.xml";
            System.Xml.Serialization.XmlSerializer jt =
            new System.Xml.Serialization.XmlSerializer(input.GetType());
            StreamWriter jtilastot = new StreamWriter(Ottelutilastot);
            jt.Serialize(jtilastot, input);
            jtilastot.Close();

        }
        public void SerializeXMPelaajatilastot(List<Pelaajatilastot> input)
        {
            string Pelaajatilastot = oikopolku.lyhyt + "Pelaajatilasot.xml";
            System.Xml.Serialization.XmlSerializer pt =
            new System.Xml.Serialization.XmlSerializer(input.GetType());
            StreamWriter piilastot = new StreamWriter(Pelaajatilastot);
            pt.Serialize(piilastot, input);
            piilastot.Close();

        }

        private void btnUusi_Click(object sender, EventArgs e)
        {

            btnUusi.Enabled = false;
            btnLisaa.Enabled = false;
            btnMuokkaa.Enabled = false;
            btnTallenna.Enabled = false;
            btnVierastapahtuma.Enabled = false;
            btnKotitapahtuma.Enabled = false;

            cbKotijoukkue.Enabled = true;
            cbVierasjoukkue.Enabled = true;
            btnKelaus.Enabled = true;
            tmrAika.Enabled = true;
            btnTauko.Enabled = true;
            btnAloita.Enabled = true;

            oikopolku.kotimaali = 0;
            oikopolku.vierasmaali = 0;
            oikopolku.sekuntti = 0;
            oikopolku.minuutti = 0;

            labVierasmaali.Text = "0";
            labKotimaali.Text = "0";
            btnAloita.Text = "Aloita";
            labEra.Text = "Erä 1";
            tbKotiaika.Text = "";
            tbViearasaika.Text = "";

        }
        public void OtteluTilastot()
        {
            Ottelutilastot add = new Ottelutilastot();
            add.pvm = oikopolku.paivamaara;
            add.koti = cbKotijoukkue.Text;
            add.vieras = cbVierasjoukkue.Text;
            if (oikopolku.kotimaali > oikopolku.vierasmaali)
            {
                add.yksi = "x";
                add.risti = " ";
                add.kaksi = " ";
            }
            else if (oikopolku.kotimaali < oikopolku.vierasmaali)
            {
                add.yksi = " ";
                add.risti = "x";
                add.kaksi = " ";
            }
            else
            {
                add.yksi = " ";
                add.risti = " ";
                add.kaksi = "x";
            }

            ottelutilastot.Add(add);
            dgwPelatutottelut.DataSource = null;
            dgwPelatutottelut.DataSource = ottelutilastot;
            dgwPelatutottelut.ClearSelection();
        }
        public void JoukkueTilastot()
        {
            if (cbKotijoukkue.SelectedItem.ToString() == "Arsenal")
            {
                
                if (oikopolku.kotimaali > oikopolku.vierasmaali)
                {
                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 1;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 0;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 3;

                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[1].voitto + 0;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[1].tasapeli + 0;
                    joukkuetilastot[1].tappio = joukkuetilastot[1].tappio + 1;
                    joukkuetilastot[1].pisteet = joukkuetilastot[1].pisteet + 0;
                }
                else if (oikopolku.kotimaali < oikopolku.vierasmaali)
                {
                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 0;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 1;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 0;

                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[1].voitto + 1;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[1].tasapeli + 0;
                    joukkuetilastot[1].tappio = joukkuetilastot[1].tappio + 0;
                    joukkuetilastot[1].pisteet = joukkuetilastot[1].pisteet + 3;
                }
                else
                {
                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 1;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 1;

                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[0].tasapeli + 1;
                    joukkuetilastot[1].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[1].pisteet = joukkuetilastot[0].pisteet + 1;
                }
            }
            if (cbKotijoukkue.SelectedItem.ToString() == "Savonia")
            {
                if (oikopolku.kotimaali > oikopolku.vierasmaali)
                {
                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[1].voitto + 1;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[1].tasapeli + 0;
                    joukkuetilastot[1].tappio = joukkuetilastot[1].tappio + 0;
                    joukkuetilastot[1].pisteet = joukkuetilastot[1].pisteet + 3;

                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 0;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 1;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 0;
                }
                else if (oikopolku.kotimaali < oikopolku.vierasmaali)
                {
                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[1].voitto + 0;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[1].tasapeli + 0;
                    joukkuetilastot[1].tappio = joukkuetilastot[1].tappio + 1;
                    joukkuetilastot[1].pisteet = joukkuetilastot[1].pisteet + 0;

                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 1;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 0;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 3;
                }
                else
                {
                    joukkuetilastot[1].nimi = "Savonia";
                    joukkuetilastot[1].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[1].tasapeli = joukkuetilastot[0].tasapeli + 1;
                    joukkuetilastot[1].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[1].pisteet = joukkuetilastot[0].pisteet + 1;

                    joukkuetilastot[0].nimi = "Arsenal";
                    joukkuetilastot[0].voitto = joukkuetilastot[0].voitto + 0;
                    joukkuetilastot[0].tasapeli = joukkuetilastot[0].tasapeli + 1;
                    joukkuetilastot[0].tappio = joukkuetilastot[0].tappio + 0;
                    joukkuetilastot[0].pisteet = joukkuetilastot[0].pisteet + 1;
                }
            }

        }
        public void PoistaKaikki()
        {                    

            int selectedIndex = dgvOttelu.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            dgvOttelu.Rows.RemoveAt(selectedIndex);
            dgvOttelu.Refresh();
            
        }
            



    }
}
