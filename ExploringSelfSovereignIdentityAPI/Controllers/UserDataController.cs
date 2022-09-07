﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExploringSelfSovereignIdentityAPI.Services.NetheriumBlockChain;
using System;
using ExploringSelfSovereignIdentityAPI.Services.blockChain;
using ExploringSelfSovereignIdentityAPI.Models.Request;

namespace ExploringSelfSovereignIdentityAPI.Controllers.UserData
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : Controller
    {

        private readonly IUserDataService uds;

        private UserDataResponse response = new UserDataResponse();

        public UserDataController(IBlockchainService blockchainService, IUserDataService uds)
        {
            this.uds = uds;
        }

        [HttpPost]
        [Route("create")]
        public async Task<string> Register([FromBody] RegisterRequest request)
        {
            return await uds.createUser(request.id);
        }

        [HttpPost]
        [Route("get")]
        public async Task<GetUserDataOutputDTO> GetUserData([FromBody] RegisterRequest request)
        {
            return await uds.getUserData(request.id);
        }

        [HttpPost]
        [Route("updateAttribute")]
        public UserDataResponse UpdateAttributes([FromBody] UserDataResponse request)
        {

            for (int i = 0; i < request.Attributes.Count; i++)
            {

                if (i < response.Attributes.Count)
                {
                    response.Attributes[i].Name = request.Attributes[i].Name;
                    response.Attributes[i].Value = request.Attributes[i].Value;
                    continue;
                }

                Services.NetheriumBlockChain.Attribute a1 = new Services.NetheriumBlockChain.Attribute();
                a1.Name = request.Attributes[i].Name;
                a1.Value = request.Attributes[i].Value;

                response.Attributes.Add(a1);

            }

            return response;
        }

        [HttpPost]
        [Route("updateCredential")]
        public async Task<GetUserDataOutputDTO> UpdateCredentials([FromBody] UpdateGen2 request )
        {
            return await uds.updateUserData(request);
        }

        //Transactions

        [HttpPost]
        [Route("newTransaction")]
        public async Task<String> newTransaction([FromBody] TransactionRequest request)
        {
            return await uds.newTransactionRequest(request);
        }
    }
}
