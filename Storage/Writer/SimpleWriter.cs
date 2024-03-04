using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Writer;
internal class SimpleWriter : IWriter
{
    string _path;
    string _identity;

    public SimpleWriter(string path, string identity)
    {
        _path = path;
        _identity = identity;
    }

    DateTime _created;
    TimeOnly _duration;
    string _summary = string.Empty;
    string _fulltext = string.Empty;
    public void Created(DateTime started) => _created = started;
    public void Duration(TimeOnly duration) => _duration = duration;
    public void FullText(in string text) => _fulltext = text;
    public void Summary(in string summary) => _summary = summary;
    public void Write()
    {
        using var writer = new StreamWriter(Path.Combine(_path,_identity));
        writer.Write("###CONFIG###");
        writer.Write("\nname=");
        writer.Write(_identity);
        writer.Write("\ncreated=");
        writer.Write(_created.ToString());
        writer.Write("\nduration=");
        writer.Write(_duration.ToString());
        writer.Write("\n###SUMMARY###\n");
        writer.Write(_summary);
        writer.Write("\n###FULLTEXT###\n");
        writer.Write(_fulltext);

    }
}
