using System;
using System.Collections.Generic;
using MessagePack;

namespace Yugawara
{
    [MessagePackObject]
    public class Commitment
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Key(1)]
        public int Id { get; set; }
            
        [Key(2)]
        public string Address { get; set; }
        
        [Key(3)]
        public string TrainerAddress { get; set; }
        
        [Key(4)]
        public int Deposit { get; set; }
        
        [Key(5)]
        public int ExpireDate { get; set; }
    }
}

