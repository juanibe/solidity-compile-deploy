using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using CompileAndDeploy.Contracts.UsersContract.ContractDefinition;

namespace CompileAndDeploy.Contracts.UsersContract
{
    public partial class UsersContractService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, UsersContractDeployment usersContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<UsersContractDeployment>().SendRequestAndWaitForReceiptAsync(usersContractDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, UsersContractDeployment usersContractDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<UsersContractDeployment>().SendRequestAsync(usersContractDeployment);
        }

        public static async Task<UsersContractService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, UsersContractDeployment usersContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, usersContractDeployment, cancellationTokenSource);
            return new UsersContractService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public UsersContractService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<GetUserOutputDTO> GetUserQueryAsync(GetUserFunction getUserFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetUserFunction, GetUserOutputDTO>(getUserFunction, blockParameter);
        }

        public Task<GetUserOutputDTO> GetUserQueryAsync(string addr, BlockParameter blockParameter = null)
        {
            var getUserFunction = new GetUserFunction();
                getUserFunction.Addr = addr;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetUserFunction, GetUserOutputDTO>(getUserFunction, blockParameter);
        }

        public Task<string> JoinRequestAsync(JoinFunction joinFunction)
        {
             return ContractHandler.SendRequestAsync(joinFunction);
        }

        public Task<TransactionReceipt> JoinRequestAndWaitForReceiptAsync(JoinFunction joinFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(joinFunction, cancellationToken);
        }

        public Task<string> JoinRequestAsync(string name, string surname)
        {
            var joinFunction = new JoinFunction();
                joinFunction.Name = name;
                joinFunction.Surname = surname;
            
             return ContractHandler.SendRequestAsync(joinFunction);
        }

        public Task<TransactionReceipt> JoinRequestAndWaitForReceiptAsync(string name, string surname, CancellationTokenSource cancellationToken = null)
        {
            var joinFunction = new JoinFunction();
                joinFunction.Name = name;
                joinFunction.Surname = surname;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(joinFunction, cancellationToken);
        }
    }
}
