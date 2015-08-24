using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.ViewModels
{
    public class Character
    {
        public int hp { get; set; }
        public int maxHp { get; set; }
        public int position { get; set; }
        //TODO:  String should be replaced with a visual indicator class (e.g. link to image and name of weakness)
        public List<String> weaknesses { get; set; }
        public List<Ability> abilities { get; set; }
    }
}
