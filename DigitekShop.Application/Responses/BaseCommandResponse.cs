using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitekShop.Application.Responses
{
    public class BaseCommandResponse
    {
        public int Id { get; set; }
        public string Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
