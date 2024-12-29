using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spinning_Wheel.Classes
{
    internal class Item
    {
        private string title;
        public string Title
        {
            get => title;
            set => title = value ?? "";
        }
    }
}
