using System;
using System.Collections.Generic;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;

namespace Rahnemun.Layout
{
    public class UIWorkContext : IWorkContextStateProvider
    {
        public Func<IWorkContext, object> Get(string name)
        {
            if (name.EqualsIgnoreCase("EmbededDialogs"))
            {
                // It's really important to create list here not in the predicate below, otherwise everytime a new list is created.
                var embededDialogs = new HashSet<string>();
                return ctx => embededDialogs;
            }
            return null;
        }
    }
}
