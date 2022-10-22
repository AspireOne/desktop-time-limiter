using System.ComponentModel;

// This is a dummy class that enables the use of the init keyword on frameworks earlier than .net 5.0.
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}