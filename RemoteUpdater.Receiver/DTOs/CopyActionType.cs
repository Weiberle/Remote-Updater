using System;
using System.Collections.Generic;

namespace RemoteUpdater.Receiver.DTOs
{
    internal class CopyActionType
    {
        internal IEnumerable<Type> PreCopyTypes { get; set; }
        internal IEnumerable<Type> PostCopyTypes { get; set; }

        internal string FilePath { get; set; }
    }
}
