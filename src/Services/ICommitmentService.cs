using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;

namespace Yugawara
{
    public interface ICommitmentService : IService<ICommitmentService>
    {
        Task<UnaryResult<List<Commitment>>> GetCommitments();
    }
}
