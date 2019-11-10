using System.Numerics;
using MessagePack;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Yugawara
{
    [MessagePackObject]
    [Event("CommitmentEventDTO")]
    public class Commitment
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Key(1)]
        public int Id { get; set; }
            
        [Key(2)]
        [Parameter("address", "Address", 1, true)]
        public string Address { get; set; }
        
        [Key(3)]
        [Parameter("address", "TrainerAddress", 2, true)]
        public string TrainerAddress { get; set; }
        
        [Key(4)]
        [Parameter("uint256", "Deposit", 3, false)]
        public BigInteger Deposit { get; set; }
        
        [Key(5)]
        public int ExpireDate { get; set; }
    }
}

