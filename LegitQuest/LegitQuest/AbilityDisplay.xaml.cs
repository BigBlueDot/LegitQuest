using LegitQuest.EventInfo;
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
    /// Interaction logic for AbilityDisplay.xaml
    /// </summary>
    public partial class AbilityDisplay : UserControl
    {
        private Guid characterId;
        private int commandNumber;
        public event AbilitySelectedEventHandler AbilitySelected;
        public delegate void AbilitySelectedEventHandler(AbilitySelectedEventArgs e);


        public AbilityDisplay(Guid characterId, String name, int commandNumber, bool enabled)
        {
            InitializeComponent();
            Ability.Content = name;
            this.characterId = characterId;
            this.commandNumber = commandNumber;
            Ability.IsEnabled = enabled;
        }

        private void Ability_Click_1(object sender, RoutedEventArgs e)
        {
            AbilitySelectedEventArgs args = new AbilitySelectedEventArgs();
            args.commandIssued = new MessageDataStructures.CommandIssued();
            args.commandIssued.source = characterId;
            args.commandIssued.commandNumber = this.commandNumber;
            if (AbilitySelected != null)
            {
                AbilitySelected(args);
            }
        }
    }
}
