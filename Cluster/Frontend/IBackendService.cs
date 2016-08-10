using System;
using System.Threading.Tasks;

namespace Frontend
{
    public interface IBackendService
    {
        Task<int> Increment(int x);
    }
}
