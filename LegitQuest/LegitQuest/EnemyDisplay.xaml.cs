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
    /// Interaction logic for EnemyDisplay.xaml
    /// </summary>
    public partial class EnemyDisplay : UserControl
    {
        private int hp;
        private int maxHP;
        private string name;
        private Guid characterId;

        public delegate void CharacterClickedEventHandler(CharacterSelectedEventArgs args);
        public event CharacterClickedEventHandler EnemyClicked;

        public EnemyDisplay(MessageDataStructures.ViewModels.Character character)
        {
            InitializeComponent();
            UpdateEnemy(character);
        }

        public void UpdateEnemy(MessageDataStructures.ViewModels.Character enemy)
        {
            this.hp = enemy.hp;
            this.maxHP = enemy.maxHp;
            this.name = "Name";
            this.characterId = enemy.id;

            this.txtName.Text = this.name;
            this.txtHP.Text = this.hp.ToString() + " / " + this.maxHP.ToString();
        }

        private void Select_Click_1(object sender, RoutedEventArgs e)
        {
            if (EnemyClicked != null)
            {
                CharacterSelectedEventArgs args = new CharacterSelectedEventArgs();
                args.id = this.characterId;
                EnemyClicked(args);
            }
        }
    }
}
