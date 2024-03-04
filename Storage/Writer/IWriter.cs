using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage;
public interface IWriter
{
    public void Created(DateTime started);
    public void Duration(TimeOnly duration);


    // We are using "in string" to avoid excessive copying.
    // There may be better ways to do it,
    // depending on how those are handled in the main implementation.
    public void Summary(in string summary);
    public void FullText(in string text);

    public void Write();
}
