using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage;
public interface IStore
{
    // Yes, this could be a property.
    // No, it will not be a property.
    // Why? Because you don't want to set it willy-nilly,
    // so you will have to suffer whenever you want to change it.
    public string GetLocation();
    public void SetLocation(string location);
    public IWriter CreateWriter(string identity);
    public IReader CreateReader(string identity);
}
