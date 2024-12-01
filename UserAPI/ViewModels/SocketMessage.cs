using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAPI.ViewModels
{
    public class SocketMessage<T>
    {
        public string Page { get; set; }
        public T Message { get; set; }
    }
}
