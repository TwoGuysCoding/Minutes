using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Reader;
public class SimpleReader : IReader
{
    string _path;
    string _identity;

    public SimpleReader(string path, string identity)
    {
        _path = path;
        _identity = identity;
    }
    public DateTime Created() => DateTime.MinValue;
    public TimeOnly Duration() => TimeOnly.MaxValue;
    public string Summary() => "I will be bothered to write an implementation on a later date.\n";
    public string FullText() => "I will not be bothered to write an implementation on a later date.\n";
}
