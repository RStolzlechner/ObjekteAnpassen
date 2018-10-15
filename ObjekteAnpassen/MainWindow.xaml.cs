using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aucotec.EngineeringBase.Client.Runtime;
using EBObjektLibary4.EBLClasses;
using EBApplication = Aucotec.EngineeringBase.Client.Runtime.Application;

namespace ObjekteAnpassen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// Event auslösen wenn Property verändert wurde, zur aktualisierung der Ansicht
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        /// <summary>
        /// Selektierte Objekt Item in EB
        /// </summary>
        private ObjectItem selectedItem;

        /// <summary>
        /// EBApplication
        /// </summary>
        private EBApplication application;

        /// <summary>
        /// Liste zur Ansicht in der ListView
        /// </summary>
        public List<EBObjView> ObjektAnsicht
        {
            get;
            set;
        }

        /// <summary>
        /// StartListe alle Kinder des selektierten Elements in EB
        /// </summary>
        public List<EBObjView> KinderDerEBAuswahl
        {
            get
            {
                List<EBObjView> kinderDerEBAuswahl = new List<EBObjView>();

                foreach (EBLObjItBase item in EBLObjItBase.GetChildren(selectedItem, application))
                    kinderDerEBAuswahl.Add(new EBObjView(true, item));


                return kinderDerEBAuswahl;
            }
        }

        /// <summary>
        /// Liste Aller Source Assoc Elemente der Auswahl
        /// </summary>
        public List<EBObjView> AssocElementeDerAuswahl
        {
            get
            {
                List<EBObjView> assocElementeDerAuswahl = new List<EBObjView>();

                foreach(EBLObjItBase item in AllCheckedObjects)
                {
                    foreach (EBLObjItBase assItem in item.EBLSourceAssocObjects)
                    {
                        //TODO: Sind objekte mit mehreren Auswahlobjekten verknüpft werden diese doppelt angezeigt
                        assocElementeDerAuswahl.Add(new EBObjView(true, assItem));
                    }
                }

                return assocElementeDerAuswahl;
            }
        }

        public List<EBObjView> AssocKabelKiDeep
        {
            get
            {
                List<EBObjView> assocElementeDerAuswahl = new List<EBObjView>();

                foreach (EBLObjItBase item in AllCheckedObjects)
                {
                    List<EBLObjItBase> childs = EBLObjItBase.GetChildrenDeep(item.ObjectItemEB, item.ApplicationEB);

                    foreach (EBLObjItBase child in childs)
                    {
                        List<EBLKabelBase> kabel = child.EBLSourceAssocObjects.OfType<EBLKabelBase>().ToList();
                        //TODO: Sind objekte mit mehreren Auswahlobjekten verknüpft werden diese doppelt angezeigt
                        foreach (EBLKabelBase kab in kabel)
                        {
                            assocElementeDerAuswahl.Add(new EBObjView(true, kab));
                        }
                    }
                }


                return assocElementeDerAuswahl;
            }
        }

        /// <summary>
        /// Liefert Liste aller Selektierten Elemente im Auswahlfenster
        /// </summary>
        public List<EBLObjItBase> AllCheckedObjects
        {
            get
            {
                List<EBLObjItBase> allcheckedObject = new List<EBLObjItBase>();

                foreach(EBObjView viewItem in ObjektAnsicht)
                {
                    if (viewItem.Checked)
                        allcheckedObject.Add(viewItem.ObjIt);
                }

                return allcheckedObject;
            }
        }

        #region Konstruktor
        public MainWindow(ObjectItem _selected, EBApplication _application)
        {
            selectedItem = _selected;
            application = _application;
            ObjektAnsicht = KinderDerEBAuswahl;

            InitializeComponent();
        }
        #endregion

        #region ButtonHandler
        /// <summary>
        /// Beendet das Programm
        /// </summary>
        private void btBeenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Lädt alle Assoziierten Objekte der Auswahl in die Auswahl
        /// </summary>
        private void BtAssocAuswahl_Click(object sender, RoutedEventArgs e)
        {
            ObjektAnsicht = AssocElementeDerAuswahl;
            NotifyPropertyChanged("ObjektAnsicht");
        }
        #endregion

        private void BtAttrHinzuf_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(TbAttrID.Text, out int attrID) && attrID > 0)
            {
                foreach(EBLObjItBase item in AllCheckedObjects)
                {
                    item.AddAttribute(attrID, TbAttrWert.Text);
                }
            }
            else
                MessageBox.Show("Attribute ID muss eine ganze positive Zahl sein");
        }

        private void BtAssocKablDeep_Click(object sender, RoutedEventArgs e)
        {
            ObjektAnsicht = AssocKabelKiDeep;
            NotifyPropertyChanged("ObjektAnsicht");
        }
    }

    /// <summary>
    /// Hilfsklasse zur Ansicht in der Listview
    /// </summary>
    public class EBObjView
    {
        #region Eigenschaften
        /// <summary>
        /// Gibt an ob Element selektiert wurde
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Abgebildete ObjectItem in EBObLibV3 Form
        /// </summary>
        public EBLObjItBase ObjIt { get; private set; }
        #endregion

        #region Konstruktor
        public EBObjView(bool _Checked, EBLObjItBase objIt)
        {
            Checked = _Checked;
            ObjIt = objIt;
        }
        #endregion
    }
}
