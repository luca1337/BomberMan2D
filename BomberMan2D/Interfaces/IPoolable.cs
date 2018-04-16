using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public interface IPoolable
    {
        void OnGet();
        void OnRecycle();
    }
}
