using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Yugawara 
{
    [Event("CommitmentEventDTO")]
    public class CommitmentEvent : IEventDTO
    {
        [Parameter("address", "Address", 1, true)]
        public string Address { get; set; }
        
        [Parameter("address", "TrainerAddress", 2, true)]
        public string TrainerAddress { get; set; }
        
        [Parameter("uint256", "Deposit", 3, false)]
        public BigInteger Deposit { get; set; }
    }
}
