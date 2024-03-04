using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage;
public interface IReader
{
    public DateTime Created();
    public TimeOnly Duration();
    public string Summary();
    public string FullText();
}
