using Microsoft.Win32;
using Storage.Reader;
using Storage.Writer;

namespace Storage;

public class Store : IStore
{
    private static readonly string _key= "HKEY_CURRENT_USER\\Software\\TGC";

    private static readonly string _valueName = "DaWae";
    public string GetLocation() => (string)Registry.GetValue(_key, _valueName, "");
    public void SetLocation(string location) => Registry.SetValue(_key, _valueName, location);
    public IWriter CreateWriter(string identity) => new SimpleWriter(GetLocation(),identity);
    public IReader CreateReader(string identity) => new SimpleReader(GetLocation(), identity);
}
