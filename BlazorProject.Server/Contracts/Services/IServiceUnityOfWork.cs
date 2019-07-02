﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Contracts.Services
{
    public interface IServiceUnityOfWork
    {
        IMangaService MangaService { get; }
        IPokemonsService PokemonsService { get; }
    }
}
