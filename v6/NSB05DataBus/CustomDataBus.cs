using NServiceBus.DataBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05DataBus
{
    class CustomDataBus : IDataBus
    {
        Task<Stream> IDataBus.Get(string key)
        {
            return Task.FromResult<Stream>(File.OpenRead("blob.dat"));
        }

        Task<string> IDataBus.Put(Stream stream, TimeSpan timeToBeReceived)
        {
            using(var destination = File.OpenWrite("blob.dat"))
            {
                stream.CopyTo(destination);
            }
            return Task.FromResult("the-key-of-the-stored-file-such-as-the-full-path");
        }

        Task IDataBus.Start()
        {
            return Task.CompletedTask;
        }
    }
}
