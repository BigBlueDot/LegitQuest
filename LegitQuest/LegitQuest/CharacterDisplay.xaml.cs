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
    /// Interaction logic for CharacterDisplay.xaml
    /// </summary>
    public partial class CharacterDisplay : UserControl
    {
        private int hp;
        private int maxHP;
        private string name;
        private Guid characterId;

        public delegate void CharacterClickedEventHandler(CharacterSelectedEventArgs args);
        public event CharacterClickedEventHandler CharacterClicked;

        public CharacterDisplay(MessageDataStructures.ViewModels.Character character)
        {
            InitializeComponent();
            UpdateCharacter(character);
        }

        public void UpdateCharacter(MessageDataStructures.ViewModels.Character character)
        {
            this.hp = character.hp;
            this.maxHP = character.maxHp;
            this.name = "Name";
            this.characterId = character.id;

            this.txtName.Text = this.name;
            this.txtHP.Text = this.hp.ToString() + " / " + this.maxHP.ToString();
        }

        public void changeHP(int damage)
        {
            hp = hp - damage;
            this.txtHP.Text = this.hp.ToString() + " / " + this.maxHP.ToString();
        }

        private void Select_Click_1(object sender, RoutedEventArgs e)
        {
            if (CharacterClicked != null)
            {
                CharacterSelectedEventArgs args = new CharacterSelectedEventArgs();
                args.id = this.characterId;
                CharacterClicked(args);
            }
        }
    }
}
