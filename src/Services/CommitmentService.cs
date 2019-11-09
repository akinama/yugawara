using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;
using MagicOnion.Server;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Yugawara
{
    public class CommitmentService : ServiceBase<ICommitmentService>, ICommitmentService
    {
        private static readonly string ContractAddress = "0xC2A205dC8aB1fED48A05fd354aC28E0A73512834";

        private static readonly string SenderAddress = "0x15596c7faf785846455b4e8c351204c761d9f8d2";
        
        private static readonly string TrainerAddress = "0xd6e19be6cc881241cc13460f0fb33dfe941d0164";
        
        public async Task<UnaryResult<List<Commitment>>> GetCommitments()
        {
            var web3 = new Web3("http://127.0.0.1:8545/");
            
            var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(SenderAddress, "password", new HexBigInteger(120));
            
            var contract = web3.Eth.GetContract(Abi, ContractAddress);
            
            var addProjectFunction = contract.GetFunction("addProject");
            
            var gas = await addProjectFunction.EstimateGasAsync(SenderAddress, new HexBigInteger(90000), null);
            
            var receipt = await addProjectFunction.SendTransactionAndWaitForReceiptAsync(SenderAddress, gas, null);

            var transactionResult = await addProjectFunction.SendTransactionAsync(SenderAddress, TrainerAddress);

            var commitmentEvent = contract.GetEvent("Commitment");
            
            var filterAll = await commitmentEvent.CreateFilterAsync();

            var logs = await commitmentEvent.GetFilterChanges<CommitmentEvent>(filterAll);
            
            var commitment = new Commitment();
            
            return UnaryResult(new List<Commitment> { commitment });
        }
        
        private static readonly string Abi = $@"
[
  {{
    ""inputs"": [],
    ""payable"": false,
    ""stateMutability"": ""nonpayable"",
    ""type"": ""constructor""
  }},
  {{
    ""anonymous"": false,
    ""inputs"": [
      {{
        ""indexed"": true,
        ""name"": ""Address"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": true,
        ""name"": ""TrainerAddress"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": false,
        ""name"": ""Deposit"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""Commitment"",
    ""type"": ""event""
  }},
  {{
    ""anonymous"": false,
    ""inputs"": [
      {{
        ""indexed"": true,
        ""name"": ""projectOwner"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": false,
        ""name"": ""weiAmount"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""Deposited"",
    ""type"": ""event""
  }},
  {{
    ""anonymous"": false,
    ""inputs"": [
      {{
        ""indexed"": true,
        ""name"": ""projectOwner"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": false,
        ""name"": ""index"",
        ""type"": ""uint256""
      }},
      {{
        ""indexed"": false,
        ""name"": ""record"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""Recorded"",
    ""type"": ""event""
  }},
  {{
    ""anonymous"": false,
    ""inputs"": [
      {{
        ""indexed"": true,
        ""name"": ""projectOwner"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": false,
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""Completed"",
    ""type"": ""event""
  }},
  {{
    ""anonymous"": false,
    ""inputs"": [
      {{
        ""indexed"": true,
        ""name"": ""projectOwner"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": true,
        ""name"": ""trainer"",
        ""type"": ""address""
      }},
      {{
        ""indexed"": false,
        ""name"": ""weiAmount"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""Withdrawn"",
    ""type"": ""event""
  }},
  {{
    ""constant"": false,
    ""inputs"": [
      {{
        ""name"": ""trainer"",
        ""type"": ""address""
      }}
    ],
    ""name"": ""addProject"",
    ""outputs"": [],
    ""payable"": false,
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  }},
  {{
    ""constant"": false,
    ""inputs"": [
      {{
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""depositByIndex"",
    ""outputs"": [],
    ""payable"": true,
    ""stateMutability"": ""payable"",
    ""type"": ""function""
  }},
  {{
    ""constant"": false,
    ""inputs"": [
      {{
        ""name"": ""record"",
        ""type"": ""uint256""
      }},
      {{
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""recordByIndex"",
    ""outputs"": [],
    ""payable"": false,
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  }},
  {{
    ""constant"": false,
    ""inputs"": [
      {{
        ""name"": ""projectOwner"",
        ""type"": ""address""
      }},
      {{
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""completeByIndex"",
    ""outputs"": [],
    ""payable"": false,
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  }},
  {{
    ""constant"": false,
    ""inputs"": [
      {{
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""withdrawByIndex"",
    ""outputs"": [],
    ""payable"": true,
    ""stateMutability"": ""payable"",
    ""type"": ""function""
  }},
  {{
    ""constant"": true,
    ""inputs"": [
      {{
        ""name"": ""index"",
        ""type"": ""uint256""
      }}
    ],
    ""name"": ""depositOfProjectByIndex"",
    ""outputs"": [
      {{
        ""name"": """",
        ""type"": ""uint256""
      }}
    ],
    ""payable"": false,
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }},
  {{
    ""constant"": true,
    ""inputs"": [],
    ""name"": ""getProjects"",
    ""outputs"": [
      {{
        ""name"": """",
        ""type"": ""uint256[]""
      }},
      {{
        ""name"": """",
        ""type"": ""address[]""
      }}
    ],
    ""payable"": false,
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }}
]
";
    }
}
