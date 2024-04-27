using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Core.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
            
        }

        public Response(T Data, string message = null)
        {
            Succeded = true;
            Message = message;
            this.Data = Data;
        }

        public Response(string message)
        {
            Succeded = false;
            Message = message;

        }

        public bool Succeded { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
