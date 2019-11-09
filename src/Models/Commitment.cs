using System;
using System.Collections.Generic;
using MessagePack;

namespace Yugawara
{
    public class Commitment
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
            
        public string Address { get; set; }
        
        public string TrainerAddress { get; set; }
        
        public int Deposit { get; set; }
        
        public int ExpireDate { get; set; }
    }
}

