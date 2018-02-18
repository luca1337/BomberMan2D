using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Interfaces
{
    public interface IPoolable
    {
        void OnGet();
        void OnRecycle();
    }
}
