using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMod.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class GameArgument : Attribute
    {
        public string title;
        public GameArgument(string _title, Action listOfArgs)
        {
            title = _title;
        }
    }
}
