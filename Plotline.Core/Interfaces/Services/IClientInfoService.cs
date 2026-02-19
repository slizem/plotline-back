using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotline.Core.Interfaces.Services
{
    public interface IClientInfoService
    {
        string GetClientIp();
        string GetUserAgent();
    }
}
