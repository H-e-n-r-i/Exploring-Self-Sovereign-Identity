﻿using ExploringSelfSovereignIdentityAPI.Models.Request;
using ExploringSelfSovereignIdentityAPI.Services.NetheriumBlockChain;
using MediatR;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.DeploymentHandlers;
using Nethereum.Web3;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Attribute = ExploringSelfSovereignIdentityAPI.Services.NetheriumBlockChain.Attribute;

namespace ExploringSelfSovereignIdentityAPI.Services
{
    public class MarketPlaceService : IMarketPlaceService
    {

        /*static string url = "http://127.0.0.1:8545";
        static string privateKey = "734674bd34f2476f15c6d5f6c8c1c7c92e465921e546771d088b958607531d10";
        private readonly string senderAddress = "0x8A1f48B91fbDC94b82E1997c2630466c5FaCf38b";
        private static string contractAddress = "0x7D6FAec0a4833Ad806EBCbe7d1BFe66caCF0a961";

        static Web3 web3 = new Web3(new Nethereum.Web3.Accounts.Account(privateKey), url);

        private ContractHandler contractHandler = web3.Eth.GetContractHandler(contractAddress);

        public MarketPlaceService()
        {
            web3.TransactionManager.UseLegacyAsDefault = true;
        }*/

        private static string url = "http://testchain.nethereum.com:8545";
        private static string privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
        static Web3 web3 = new Web3(new Nethereum.Web3.Accounts.Account(privateKey, 444444444500), url);

        private static ContractHandler contractHandler;

        private async Task<ContractHandler> deploy()
        {
            var marketPlaceDeployment = new MarketPlaceDeployment();

            var transactionReceiptDeployment = await web3.Eth.GetContractDeploymentHandler<MarketPlaceDeployment>().SendRequestAndWaitForReceiptAsync(marketPlaceDeployment);
            var contractAddress = transactionReceiptDeployment.ContractAddress;
            
            return web3.Eth.GetContractHandler(contractAddress);
        }

        public async Task<string> addDataPack(AddDataPackRequest request)
        {

            if (contractHandler == null) contractHandler = await deploy();

            var addDataPackFunction = new AddDataPackFunction();
            addDataPackFunction.Request = request;
            var addDataPackFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(addDataPackFunction);

            return "success";
        }

        public async Task<string> buyData(BuyDataRequest request)
        {
            if (contractHandler == null) contractHandler = await deploy();

            var buyDataFunction = new BuyDataFunction();
            buyDataFunction.Request = request;
            var buyDataFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(buyDataFunction);

            return "success";
        }

        public async Task<string> createOrganization(CreateOrgRequest request)
        {
            if (contractHandler == null) contractHandler = await deploy();

            var createOrganizationFunction = new CreateOrganizationFunction();
            createOrganizationFunction.Request = request;
            var createOrganizationFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createOrganizationFunction);

            return "success";
        }

        public async Task<GetOrganizationOutputDTO> getOrganization(CreateOrgRequest request)
        {
            if (contractHandler == null) contractHandler = await deploy();

            var getOrganizationFunction = new GetOrganizationFunction();
            getOrganizationFunction.Request = request;
            var getOrganizationOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetOrganizationFunction, GetOrganizationOutputDTO>(getOrganizationFunction);

            return getOrganizationOutputDTO;
        }
    }

    public partial class MarketPlaceDeployment : MarketPlaceDeploymentBase
    {
        public MarketPlaceDeployment() : base(BYTECODE) { }
        public MarketPlaceDeployment(string byteCode) : base(byteCode) { }
    }

    public class MarketPlaceDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x608060405234801561001057600080fd5b506121d0806100206000396000f3fe608060405234801561001057600080fd5b506004361061004c5760003560e01c806317d104f81461005157806373001f801461006d578063804a18b81461009d5780638d0f9c68146100b9575b600080fd5b61006b600480360381019061006691906115c0565b6100d5565b005b61008760048036038101906100829190611691565b6102be565b6040516100949190611b07565b60405180910390f35b6100b760048036038101906100b29190611d7a565b610c0c565b005b6100d360048036038101906100ce9190611691565b611106565b005b60008082600001516040516100ea9190611dff565b9081526020016040518091039020600301600081548092919061010c90611e45565b91905055905081602001516000836000015160405161012b9190611dff565b90815260200160405180910390206004016000838152602001908152602001600020600001908161015c9190612099565b508160400151600083600001516040516101769190611dff565b9081526020016040518091039020600401600083815260200190815260200160002060010181905550816060015151600083600001516040516101b99190611dff565b908152602001604051809103902060040160008381526020019081526020016000206002018190555060008083600001516040516101f79190611dff565b908152602001604051809103902060040160008381526020019081526020016000206004018190555060005b8260600151518110156102b957826060015181815181106102475761024661216b565b5b6020026020010151600084600001516040516102639190611dff565b90815260200160405180910390206004016000848152602001908152602001600020600301600083815260200190815260200160002090816102a59190612099565b5080806102b190611e45565b915050610223565b505050565b6102c66111f7565b6102ce6111f7565b6103898360200151600085600001516040516102ea9190611dff565b9081526020016040518091039020600101805461030690611ebc565b80601f016020809104026020016040519081016040528092919081815260200182805461033290611ebc565b801561037f5780601f106103545761010080835404028352916020019161037f565b820191906000526020600020905b81548152906001019060200180831161036257829003601f168201915b50505050506111cc565b6103d4576040518060400160405280600681526020017f6661696c65640000000000000000000000000000000000000000000000000000815250816040018190525080915050610c07565b826000015181600001819052506040518060400160405280600781526020017f73756363657373000000000000000000000000000000000000000000000000008152508160400181905250600083600001516040516104339190611dff565b90815260200160405180910390206002015481602001818152505060008084600001516040516104639190611dff565b90815260200160405180910390206003015490508067ffffffffffffffff8111156104915761049061129e565b5b6040519080825280602002602001820160405280156104ca57816020015b6104b761121f565b8152602001906001900390816104af5790505b50826060018190525060005b81811015610c0057600085600001516040516104f29190611dff565b90815260200160405180910390206004016000828152602001908152602001600020600001805461052290611ebc565b80601f016020809104026020016040519081016040528092919081815260200182805461054e90611ebc565b801561059b5780601f106105705761010080835404028352916020019161059b565b820191906000526020600020905b81548152906001019060200180831161057e57829003601f168201915b5050505050836060015182815181106105b7576105b661216b565b5b602002602001015160000181905250600085600001516040516105da9190611dff565b9081526020016040518091039020600401600082815260200190815260200160002060010154836060015182815181106106175761061661216b565b5b602002602001015160200181815250506000856000015160405161063b9190611dff565b908152602001604051809103902060040160008281526020019081526020016000206004015467ffffffffffffffff81111561067a5761067961129e565b5b6040519080825280602002602001820160405280156106b357816020015b6106a0611240565b8152602001906001900390816106985790505b50836060015182815181106106cb576106ca61216b565b5b60200260200101516040018190525060005b600086600001516040516106f19190611dff565b9081526020016040518091039020600401600083815260200190815260200160002060040154811015610bec57600086600001516040516107329190611dff565b908152602001604051809103902060040160008381526020019081526020016000206005016000828152602001908152602001600020600001805461077690611ebc565b80601f01602080910402602001604051908101604052809291908181526020018280546107a290611ebc565b80156107ef5780601f106107c4576101008083540402835291602001916107ef565b820191906000526020600020905b8154815290600101906020018083116107d257829003601f168201915b50505050508460600151838151811061080b5761080a61216b565b5b60200260200101516040015182815181106108295761082861216b565b5b602002602001015160000181905250600080876000015160405161084d9190611dff565b9081526020016040518091039020600401600084815260200190815260200160002060050160008381526020019081526020016000206001015490508067ffffffffffffffff8111156108a3576108a261129e565b5b6040519080825280602002602001820160405280156108dc57816020015b6108c961125a565b8152602001906001900390816108c15790505b50856060015184815181106108f4576108f361216b565b5b60200260200101516040015183815181106109125761091161216b565b5b60200260200101516020018190525060005b81811015610bd757600088600001516040516109409190611dff565b9081526020016040518091039020600401600085815260200190815260200160002060050160008481526020019081526020016000206002016000828152602001908152602001600020600001805461099890611ebc565b80601f01602080910402602001604051908101604052809291908181526020018280546109c490611ebc565b8015610a115780601f106109e657610100808354040283529160200191610a11565b820191906000526020600020905b8154815290600101906020018083116109f457829003601f168201915b505050505086606001518581518110610a2d57610a2c61216b565b5b6020026020010151604001518481518110610a4b57610a4a61216b565b5b6020026020010151602001518281518110610a6957610a6861216b565b5b60200260200101516000018190525060008860000151604051610a8c9190611dff565b90815260200160405180910390206004016000858152602001908152602001600020600501600084815260200190815260200160002060020160008281526020019081526020016000206001018054610ae490611ebc565b80601f0160208091040260200160405190810160405280929190818152602001828054610b1090611ebc565b8015610b5d5780601f10610b3257610100808354040283529160200191610b5d565b820191906000526020600020905b815481529060010190602001808311610b4057829003601f168201915b505050505086606001518581518110610b7957610b7861216b565b5b6020026020010151604001518481518110610b9757610b9661216b565b5b6020026020010151602001518281518110610bb557610bb461216b565b5b6020026020010151602001819052508080610bcf90611e45565b915050610924565b50508080610be490611e45565b9150506106dd565b508080610bf890611e45565b9150506104d6565b5081925050505b919050565b60005b60008260200151604051610c239190611dff565b908152602001604051809103902060030154811015610d965760005b60008360200151604051610c539190611dff565b9081526020016040518091039020600401600083815260200190815260200160002060040154811015610d8257610d6360008460200151604051610c979190611dff565b9081526020016040518091039020600401600084815260200190815260200160002060050160008381526020019081526020016000206000018054610cdb90611ebc565b80601f0160208091040260200160405190810160405280929190818152602001828054610d0790611ebc565b8015610d545780601f10610d2957610100808354040283529160200191610d54565b820191906000526020600020905b815481529060010190602001808311610d3757829003601f168201915b505050505084600001516111cc565b15610d6f575050611103565b8080610d7a90611e45565b915050610c3f565b508080610d8e90611e45565b915050610c0f565b5060005b60008260200151604051610dae9190611dff565b90815260200160405180910390206003015481101561110157610e9660008360200151604051610dde9190611dff565b908152602001604051809103902060040160008381526020019081526020016000206000018054610e0e90611ebc565b80601f0160208091040260200160405190810160405280929190818152602001828054610e3a90611ebc565b8015610e875780601f10610e5c57610100808354040283529160200191610e87565b820191906000526020600020905b815481529060010190602001808311610e6a57829003601f168201915b505050505083604001516111cc565b156110ee576000808360200151604051610eb09190611dff565b908152602001604051809103902060040160008381526020019081526020016000206004016000815480929190610ee690611e45565b919050559050826000015160008460200151604051610f059190611dff565b9081526020016040518091039020600401600084815260200190815260200160002060050160008381526020019081526020016000206000019081610f4a9190612099565b5082606001515160008460200151604051610f659190611dff565b9081526020016040518091039020600401600084815260200190815260200160002060050160008381526020019081526020016000206001018190555060005b8360600151518110156110e75783606001518181518110610fc957610fc861216b565b5b60200260200101516000015160008560200151604051610fe99190611dff565b908152602001604051809103902060040160008581526020019081526020016000206005016000848152602001908152602001600020600201600083815260200190815260200160002060000190816110429190612099565b508360600151818151811061105a5761105961216b565b5b6020026020010151602001516000856020015160405161107a9190611dff565b908152602001604051809103902060040160008581526020019081526020016000206005016000848152602001908152602001600020600201600083815260200190815260200160002060010190816110d39190612099565b5080806110df90611e45565b915050610fa5565b5050611101565b80806110f990611e45565b915050610d9a565b505b50565b80600001516000826000015160405161111f9190611dff565b9081526020016040518091039020600001908161113c9190612099565b508060200151600082600001516040516111569190611dff565b908152602001604051809103902060010190816111739190612099565b5060008082600001516040516111899190611dff565b9081526020016040518091039020600301819055506064600082600001516040516111b49190611dff565b90815260200160405180910390206002018190555050565b600081805190602001208380519060200120036111ec57600190506111f1565b600090505b92915050565b6040518060800160405280606081526020016000815260200160608152602001606081525090565b60405180606001604052806060815260200160008152602001606081525090565b604051806040016040528060608152602001606081525090565b604051806040016040528060608152602001606081525090565b6000604051905090565b600080fd5b600080fd5b600080fd5b6000601f19601f8301169050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b6112d68261128d565b810181811067ffffffffffffffff821117156112f5576112f461129e565b5b80604052505050565b6000611308611274565b905061131482826112cd565b919050565b600080fd5b600080fd5b600080fd5b600067ffffffffffffffff8211156113435761134261129e565b5b61134c8261128d565b9050602081019050919050565b82818337600083830152505050565b600061137b61137684611328565b6112fe565b90508281526020810184848401111561139757611396611323565b5b6113a2848285611359565b509392505050565b600082601f8301126113bf576113be61131e565b5b81356113cf848260208601611368565b91505092915050565b6000819050919050565b6113eb816113d8565b81146113f657600080fd5b50565b600081359050611408816113e2565b92915050565b600067ffffffffffffffff8211156114295761142861129e565b5b602082029050602081019050919050565b600080fd5b600061145261144d8461140e565b6112fe565b905080838252602082019050602084028301858111156114755761147461143a565b5b835b818110156114bc57803567ffffffffffffffff81111561149a5761149961131e565b5b8086016114a789826113aa565b85526020850194505050602081019050611477565b5050509392505050565b600082601f8301126114db576114da61131e565b5b81356114eb84826020860161143f565b91505092915050565b60006080828403121561150a57611509611288565b5b61151460806112fe565b9050600082013567ffffffffffffffff81111561153457611533611319565b5b611540848285016113aa565b600083015250602082013567ffffffffffffffff81111561156457611563611319565b5b611570848285016113aa565b6020830152506040611584848285016113f9565b604083015250606082013567ffffffffffffffff8111156115a8576115a7611319565b5b6115b4848285016114c6565b60608301525092915050565b6000602082840312156115d6576115d561127e565b5b600082013567ffffffffffffffff8111156115f4576115f3611283565b5b611600848285016114f4565b91505092915050565b60006040828403121561161f5761161e611288565b5b61162960406112fe565b9050600082013567ffffffffffffffff81111561164957611648611319565b5b611655848285016113aa565b600083015250602082013567ffffffffffffffff81111561167957611678611319565b5b611685848285016113aa565b60208301525092915050565b6000602082840312156116a7576116a661127e565b5b600082013567ffffffffffffffff8111156116c5576116c4611283565b5b6116d184828501611609565b91505092915050565b600081519050919050565b600082825260208201905092915050565b60005b838110156117145780820151818401526020810190506116f9565b83811115611723576000848401525b50505050565b6000611734826116da565b61173e81856116e5565b935061174e8185602086016116f6565b6117578161128d565b840191505092915050565b61176b816113d8565b82525050565b600081519050919050565b600082825260208201905092915050565b6000819050602082019050919050565b600081519050919050565b600082825260208201905092915050565b6000819050602082019050919050565b600081519050919050565b600082825260208201905092915050565b6000819050602082019050919050565b600060408301600083015184820360008601526118128282611729565b9150506020830151848203602086015261182c8282611729565b9150508091505092915050565b600061184583836117f5565b905092915050565b6000602082019050919050565b6000611865826117c9565b61186f81856117d4565b935083602082028501611881856117e5565b8060005b858110156118bd578484038952815161189e8582611839565b94506118a98361184d565b925060208a01995050600181019050611885565b50829750879550505050505092915050565b600060408301600083015184820360008601526118ec8282611729565b91505060208301518482036020860152611906828261185a565b9150508091505092915050565b600061191f83836118cf565b905092915050565b6000602082019050919050565b600061193f8261179d565b61194981856117a8565b93508360208202850161195b856117b9565b8060005b8581101561199757848403895281516119788582611913565b945061198383611927565b925060208a0199505060018101905061195f565b50829750879550505050505092915050565b600060608301600083015184820360008601526119c68282611729565b91505060208301516119db6020860182611762565b50604083015184820360408601526119f38282611934565b9150508091505092915050565b6000611a0c83836119a9565b905092915050565b6000602082019050919050565b6000611a2c82611771565b611a36818561177c565b935083602082028501611a488561178d565b8060005b85811015611a845784840389528151611a658582611a00565b9450611a7083611a14565b925060208a01995050600181019050611a4c565b50829750879550505050505092915050565b60006080830160008301518482036000860152611ab38282611729565b9150506020830151611ac86020860182611762565b5060408301518482036040860152611ae08282611729565b91505060608301518482036060860152611afa8282611a21565b9150508091505092915050565b60006020820190508181036000830152611b218184611a96565b905092915050565b600067ffffffffffffffff821115611b4457611b4361129e565b5b602082029050602081019050919050565b600060408284031215611b6b57611b6a611288565b5b611b7560406112fe565b9050600082013567ffffffffffffffff811115611b9557611b94611319565b5b611ba1848285016113aa565b600083015250602082013567ffffffffffffffff811115611bc557611bc4611319565b5b611bd1848285016113aa565b60208301525092915050565b6000611bf0611beb84611b29565b6112fe565b90508083825260208201905060208402830185811115611c1357611c1261143a565b5b835b81811015611c5a57803567ffffffffffffffff811115611c3857611c3761131e565b5b808601611c458982611b55565b85526020850194505050602081019050611c15565b5050509392505050565b600082601f830112611c7957611c7861131e565b5b8135611c89848260208601611bdd565b91505092915050565b600060808284031215611ca857611ca7611288565b5b611cb260806112fe565b9050600082013567ffffffffffffffff811115611cd257611cd1611319565b5b611cde848285016113aa565b600083015250602082013567ffffffffffffffff811115611d0257611d01611319565b5b611d0e848285016113aa565b602083015250604082013567ffffffffffffffff811115611d3257611d31611319565b5b611d3e848285016113aa565b604083015250606082013567ffffffffffffffff811115611d6257611d61611319565b5b611d6e84828501611c64565b60608301525092915050565b600060208284031215611d9057611d8f61127e565b5b600082013567ffffffffffffffff811115611dae57611dad611283565b5b611dba84828501611c92565b91505092915050565b600081905092915050565b6000611dd9826116da565b611de38185611dc3565b9350611df38185602086016116f6565b80840191505092915050565b6000611e0b8284611dce565b915081905092915050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b6000611e50826113d8565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8203611e8257611e81611e16565b5b600182019050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052602260045260246000fd5b60006002820490506001821680611ed457607f821691505b602082108103611ee757611ee6611e8d565b5b50919050565b60008190508160005260206000209050919050565b60006020601f8301049050919050565b600082821b905092915050565b600060088302611f4f7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff82611f12565b611f598683611f12565b95508019841693508086168417925050509392505050565b6000819050919050565b6000611f96611f91611f8c846113d8565b611f71565b6113d8565b9050919050565b6000819050919050565b611fb083611f7b565b611fc4611fbc82611f9d565b848454611f1f565b825550505050565b600090565b611fd9611fcc565b611fe4818484611fa7565b505050565b5b8181101561200857611ffd600082611fd1565b600181019050611fea565b5050565b601f82111561204d5761201e81611eed565b61202784611f02565b81016020851015612036578190505b61204a61204285611f02565b830182611fe9565b50505b505050565b600082821c905092915050565b600061207060001984600802612052565b1980831691505092915050565b6000612089838361205f565b9150826002028217905092915050565b6120a2826116da565b67ffffffffffffffff8111156120bb576120ba61129e565b5b6120c58254611ebc565b6120d082828561200c565b600060209050601f83116001811461210357600084156120f1578287015190505b6120fb858261207d565b865550612163565b601f19841661211186611eed565b60005b8281101561213957848901518255600182019150602085019450602081019050612114565b868310156121565784890151612152601f89168261205f565b8355505b6001600288020188555050505b505050505050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052603260045260246000fdfea2646970667358221220289d37755cc9427a9ddd6ef6532aa9788d2e2c5d03ebab1f33cb3ecc3b400d5264736f6c634300080f0033";
        public MarketPlaceDeploymentBase() : base(BYTECODE) { }
        public MarketPlaceDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddDataPackFunction : AddDataPackFunctionBase { }

    [Function("addDataPack")]
    public class AddDataPackFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "request", 1)]
        public virtual AddDataPackRequest Request { get; set; }
    }

    public partial class BuyDataFunction : BuyDataFunctionBase { }

    [Function("buyData")]
    public class BuyDataFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "request", 1)]
        public virtual BuyDataRequest Request { get; set; }
    }

    public partial class CreateOrganizationFunction : CreateOrganizationFunctionBase { }

    [Function("createOrganization")]
    public class CreateOrganizationFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "request", 1)]
        public virtual CreateOrgRequest Request { get; set; }
    }

    public partial class GetOrganizationFunction : GetOrganizationFunctionBase { }

    [Function("getOrganization", typeof(GetOrganizationOutputDTO))]
    public class GetOrganizationFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "request", 1)]
        public virtual CreateOrgRequest Request { get; set; }
    }







    public partial class GetOrganizationOutputDTO : GetOrganizationOutputDTOBase { }

    [FunctionOutput]
    public class GetOrganizationOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple", "", 1)]
        public virtual OrganizationResponse ReturnValue1 { get; set; }
    }

    public partial class AddDataPackRequest : AddDataPackRequestBase { }

    public class AddDataPackRequestBase
    {
        [Parameter("string", "organization", 1)]
        public virtual string Organization { get; set; }
        [Parameter("string", "id", 2)]
        public virtual string Id { get; set; }
        [Parameter("uint256", "pricePerUnit", 3)]
        public virtual BigInteger PricePerUnit { get; set; }
        [Parameter("string[]", "requestedAttributes", 4)]
        public virtual List<string> RequestedAttributes { get; set; }
    }


    public partial class BuyDataRequest : BuyDataRequestBase { }

    public class BuyDataRequestBase
    {
        [Parameter("string", "userID", 1)]
        public virtual string UserID { get; set; }
        [Parameter("string", "organization", 2)]
        public virtual string Organization { get; set; }
        [Parameter("string", "dataPackID", 3)]
        public virtual string DataPackID { get; set; }
        [Parameter("tuple[]", "attributes", 4)]
        public virtual List<Attribute> Attributes { get; set; }
    }

    public partial class CreateOrgRequest : CreateOrgRequestBase { }

    public class CreateOrgRequestBase
    {
        [Parameter("string", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("string", "password", 2)]
        public virtual string Password { get; set; }
    }

    public partial class DataPackReceivedRequest : DataPackReceivedRequestBase { }

    public class DataPackReceivedRequestBase
    {
        [Parameter("string", "userID", 1)]
        public virtual string UserID { get; set; }
        [Parameter("tuple[]", "attributes", 2)]
        public virtual List<Attribute> Attributes { get; set; }
    }

    public partial class DataPackResponse : DataPackResponseBase { }

    public class DataPackResponseBase
    {
        [Parameter("string", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("uint256", "pricePerUnit", 2)]
        public virtual BigInteger PricePerUnit { get; set; }
        [Parameter("tuple[]", "received", 3)]
        public virtual List<DataPackReceivedRequest> Received { get; set; }
    }

    public partial class OrganizationResponse : OrganizationResponseBase { }

    public class OrganizationResponseBase
    {
        [Parameter("string", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("uint256", "balance", 2)]
        public virtual BigInteger Balance { get; set; }
        [Parameter("string", "status", 3)]
        public virtual string Status { get; set; }
        [Parameter("tuple[]", "packs", 4)]
        public virtual List<DataPackResponse> Packs { get; set; }
    }
}
