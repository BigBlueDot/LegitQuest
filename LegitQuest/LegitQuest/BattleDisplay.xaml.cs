﻿using MessageDataStructures.ViewModels;
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
    /// Interaction logic for BattleDisplay.xaml
    /// </summary>
    public partial class BattleDisplay : UserControl
    {
        private Dictionary<Guid, int> indexDictionary;
        private Dictionary<int, Dictionary<int, AbilityDisplay>> abilityDictionary;
        private Dictionary<Guid, CharacterDisplay> characterDisplays;
        private Dictionary<Guid, EnemyDisplay> enemyDisplays;

        public event LegitQuest.CharacterDisplay.CharacterClickedEventHandler characterClicked;
        public event LegitQuest.EnemyDisplay.CharacterClickedEventHandler enemyClicked;
        public event LegitQuest.AbilityDisplay.AbilitySelectedEventHandler abilityClicked;

        public BattleDisplay(List<Character> pcs, List<Character> npcs)
        {
            InitializeComponent();
            indexDictionary = new Dictionary<Guid, int>();
            abilityDictionary = new Dictionary<int, Dictionary<int, AbilityDisplay>>();
            characterDisplays = new Dictionary<Guid, CharacterDisplay>();
            enemyDisplays = new Dictionary<Guid, EnemyDisplay>();
            for (int i = 0; i < pcs.Count && i < 3; i++)
            {
                addCharacter(i, pcs[i]);
                indexDictionary.Add(pcs[i].id, i);
            }
            for (int i = 0; i < npcs.Count && i < 3; i++)
            {
                addEnemy(i, npcs[i]);
                indexDictionary.Add(npcs[i].id, i);
            }
        }

        private void addCharacter(int index, Character character)
        {
            CharacterDisplay characterDisplay = new CharacterDisplay(character);
            characterDisplay.CharacterClicked += characterDisplay_CharacterClicked;
            Grid.SetColumn(characterDisplay, index);
            PlayerCharacters.Children.Add(characterDisplay);
            characterDisplays.Add(character.id, characterDisplay);
        }

        public void modifyHP(Guid character, int damage)
        {
            if (this.characterDisplays.ContainsKey(character))
            {
                this.characterDisplays[character].changeHP(damage);
            }
            else if (this.enemyDisplays.ContainsKey(character))
            {
                this.enemyDisplays[character].changeHP(damage);
            }
        }

        public void modifyMaxHP(Guid character, int mod)
        {
            if (this.characterDisplays.ContainsKey(character))
            {
                this.characterDisplays[character].changeMaxHP(mod);
            }
            else if (this.enemyDisplays.ContainsKey(character))
            {
                this.enemyDisplays[character].changeMaxHP(mod);
            }
        }

        void characterDisplay_CharacterClicked(EventInfo.CharacterSelectedEventArgs args)
        {
            if (this.characterClicked != null)
            {
                this.characterClicked(args);
            }
        }

        private void addEnemy(int index, Character character)
        {
            EnemyDisplay enemyDisplay = new EnemyDisplay(character);
            enemyDisplay.EnemyClicked += enemyDisplay_EnemyClicked;
            Grid.SetColumn(enemyDisplay, index);
            EnemyGrid.Children.Add(enemyDisplay);
            enemyDisplays.Add(character.id, enemyDisplay);
        }

        void enemyDisplay_EnemyClicked(EventInfo.CharacterSelectedEventArgs args)
        {
            if (this.enemyClicked != null)
            {
                this.enemyClicked(args);
            }
        }

        public void addCommand(Guid characterId, int index, String name, bool enabled)
        {
            int characterIndex = indexDictionary[characterId];
            removeAbility(characterIndex, index);
            addAbility(characterId, characterIndex, index, name, enabled);
        }

        private void removeAbility(int characterIndex, int index)
        {
            if (abilityDictionary.ContainsKey(characterIndex) && abilityDictionary[characterIndex].ContainsKey(index))
            {
                AbilityDisplay toRemove = abilityDictionary[characterIndex][index];
                this.Abilities.Children.Remove(toRemove);
            }
        }

        private void addAbility(Guid id, int characterIndex, int index, String name, bool enabled)
        {
            AbilityDisplay abilityDisplay = new AbilityDisplay(id, name, index, enabled);
            if (!abilityDictionary.ContainsKey(characterIndex))
            {
                abilityDictionary.Add(characterIndex, new Dictionary<int, AbilityDisplay>());
            }
            if (!abilityDictionary[characterIndex].ContainsKey(index))
            {
                abilityDictionary[characterIndex].Add(index, abilityDisplay);
            }
            else
            {
                abilityDictionary[characterIndex][index] = abilityDisplay;
            }
            Grid.SetColumn(abilityDisplay, (characterIndex * 3) + index);
            abilityDisplay.AbilitySelected += abilityDisplay_AbilitySelected;
            Abilities.Children.Add(abilityDisplay);
        }

        void abilityDisplay_AbilitySelected(EventInfo.AbilitySelectedEventArgs e)
        {
            if (abilityClicked != null)
            {
                abilityClicked(e);
            }
        }

        public void disableAbilities(Guid characterId)
        {
            int index = indexDictionary[characterId];
            removeAbility(index, 0);
            removeAbility(index, 1);
            removeAbility(index, 2);
        }
    }
}
