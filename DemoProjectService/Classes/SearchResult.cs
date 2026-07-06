#region Assembly System.DirectoryServices.dll, v4.0.30319
// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.DirectoryServices.dll
#endregion

using System;
using System.DirectoryServices;

namespace DemoProjectService.Classes
{
    public class SearchResult
    {
        // Summary:
        //     Gets the path for this System.DirectoryServices.SearchResult.
        //
        // Returns:
        //     The path of this System.DirectoryServices.SearchResult.
        public string Path { get; }
        //
        // Summary:
        //     Gets a System.DirectoryServices.ResultPropertyCollection collection of properties
        //     for this object.
        //
        // Returns:
        //     A System.DirectoryServices.ResultPropertyCollection of properties set on
        //     this object.
        public ResultPropertyCollection Properties { get; }

        // Summary:
        //     Retrieves the System.DirectoryServices.DirectoryEntry that corresponds to
        //     the System.DirectoryServices.SearchResult from the Active Directory Domain
        //     Services hierarchy.
        //
        // Returns:
        //     The System.DirectoryServices.DirectoryEntry that corresponds to the System.DirectoryServices.SearchResult.
     //   public DirectoryEntry GetDirectoryEntry();
    }
}
