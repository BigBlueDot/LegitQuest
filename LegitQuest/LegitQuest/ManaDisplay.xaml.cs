using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LegitQuest
{
    /// <summary>
    /// Interaction logic for ManaDisplay.xaml
    /// </summary>
    public partial class ManaDisplay : UserControl
    {
        private int maxMana { get; set; }
        private int currentMana { get; set; }
        private string affinity { get; set; }

        public ManaDisplay(int maxMana, string affinity)
        {
            InitializeComponent();

            this.affinity = affinity;
            this.currentMana = maxMana;
            this.maxMana = maxMana;
            draw();
        }

        private void draw()
        {
            if (affinity != null && affinity != string.Empty)
            {
                this.txtMana.Text = affinity + " Mana:  " + currentMana + "/" + maxMana;
            }
            else
            {
                this.txtMana.Text = "Mana:  " + currentMana + "/" + maxMana;
            }
        }

        public void modifyMana(int mod)
        {
            this.currentMana += mod;
            draw();
        }
    }
}
