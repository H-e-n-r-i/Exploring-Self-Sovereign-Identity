﻿using ExploringSelfSovereignIdentityAPI.Models.Example;
using System.Threading.Tasks;

namespace ExploringSelfSovereignIdentityAPI.Services.Example
{
    public interface IExampleService
    {
        Task<string> Get();

        Task Add(ExampleModel e);
    }
}
