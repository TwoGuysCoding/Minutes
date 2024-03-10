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
    DateTime _created;
    TimeOnly _duration;
    string _summary;
    string _fulltext;
    public SimpleReader(string path, string identity)
    {
        _path = path;
        _identity = identity;
        _created = DateTime.MinValue;
        _duration = new TimeOnly();
        _summary = string.Empty;
        _fulltext = string.Empty;
    }

    public void Read()
    {
        string contents;
        using (StreamReader reader = new StreamReader(Path.Combine(_path,_identity)))
        {
            contents = reader.ReadToEnd();
        }
        contents.ReplaceLineEndings();
        var parts = contents.Split(new string[] {"###CONFIG###","###SUMMARY###","###FULLTEXT###" }, StringSplitOptions.TrimEntries);
        Dictionary<string,string> dict = new Dictionary<string,string>();
        foreach (var assignment in parts[1].Split(Environment.NewLine)) {
            var temp = assignment.Split('=', StringSplitOptions.TrimEntries);
            dict[temp[0]] = temp[1];
        }
        _created = DateTime.Parse(dict["created"]);
        _duration = TimeOnly.Parse(dict["duration"]);
        _summary = parts[2];
        _fulltext = parts[3];
    }
    public DateTime Created() => _created;
    public TimeOnly Duration() => _duration;
    public string Summary() => _summary;
    public string FullText() => _fulltext;
}
