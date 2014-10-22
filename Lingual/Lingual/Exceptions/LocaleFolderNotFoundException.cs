using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual.Exceptions
{
    class LocaleFolderNotFoundException : Exception
    {
        public LocaleFolderNotFoundException() 
            : base ("The locale folder was not found.  Please ensure there is a locale folder in the current project.") { }
    }
}
