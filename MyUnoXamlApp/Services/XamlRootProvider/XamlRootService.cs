using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnoXamlApp.Services.XamlRootProvider;

public class XamlRootService
{
    private static XamlRoot? _xamlRoot;

    public static void Initialize(XamlRoot xamlRoot)
    {
        _xamlRoot = xamlRoot;
    }
    public static XamlRoot GetXamlRoot()
    {
        if (_xamlRoot == null)
        {
            throw new InvalidOperationException("XamlRootService needs to be intialized before calling GetXamlRoot!");
        }
        return _xamlRoot;
    }

}
