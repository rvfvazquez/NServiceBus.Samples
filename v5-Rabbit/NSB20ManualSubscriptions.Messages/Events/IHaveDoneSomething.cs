using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSB20ManualSubscriptions.Messages.Events
{
    public interface IHaveDoneSomething
    {
        String What { get; set; }
    }
}
