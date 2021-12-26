using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGlobalizationService
    {
        string this[int key] { get; }
        string this[string key] { get; }
        IEnumerable<CultureInfo> SupportedCultures { get; }
        CultureInfo ActiveUiCulture { get; set; }
    }
}
