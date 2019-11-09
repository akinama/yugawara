using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;
using MagicOnion.Server;
using Nethereum.Web3;

namespace Yugawara
{
    public class CommitmentService : ServiceBase<ICommitmentService>, ICommitmentService
    {
        public async Task<UnaryResult<List<Commitment>>> GetCommitments()
        {
            var web3 = new Web3("http://127.0.0.1:8545/");
            
            var commitment = new Commitment();

            var result = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            
            commitment.Address = result.ToString();
            
            return UnaryResult(new List<Commitment>() { commitment });
        }
    }
}
