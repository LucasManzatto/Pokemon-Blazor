﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorProject.Server.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO = BlazorProject.Shared.DTO;

namespace BlazorProject.Server.Services
{
    public class PokemonsService : IPokemonsService
    {
        private readonly RepositoryContext context;
        private readonly IMapper mapper;
        public PokemonsService(RepositoryContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper; 
        }

        public async Task<DTO.FullPokemon> Get(int id)
        {
            var pokemon = await context.Pokemons
                .Include(m => m.Species)
                .Include(p => p.PokemonStats)
                .Include("PokemonTypes.Type")
                .SingleAsync(p => p.Id == id);
            var fullPokemonDTO = mapper.Map<DTO.FullPokemon>(pokemon);

            //var typeEfficacies = await context.TypeEfficacy
            //    .Where(p => pokemon.PokemonTypes.Select(s => s.TypeId).Contains(p.TargetType.TypeId))
            //    .Include("TargetType.Type")
            //    .Include("DamageType.Type")
            //    .ToListAsync();

            var typeEfficacies = await context.TypeEfficacy
                .Where(p => p.TargetTypeId == pokemon.PokemonTypes.ElementAt(0).Type.Id).ToListAsync();

            fullPokemonDTO.PokemonTypeEfficacy = mapper.Map<List<DTO.TypeEfficacy>>(typeEfficacies);

            return fullPokemonDTO;
        }

        public Task<List<DTO.DropdownPokemon>> GetAll()
        {
            return context.Pokemons
                .ProjectTo<DTO.DropdownPokemon>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<DTO.PokemonStats> GetMaxStats()
        {
            DTO.PokemonStats pokeStats = new DTO.PokemonStats
            {
                Id= 0,
                Hp = await context.PokemonStats.MaxAsync(p => p.Hp),
                Attack = await context.PokemonStats.MaxAsync(p => p.Attack),
                Defense = await context.PokemonStats.MaxAsync(p => p.Defense),
                SpAttack = await context.PokemonStats.MaxAsync(p => p.SpAttack),
                SpDefense = await context.PokemonStats.MaxAsync(p => p.SpDefense),
                Speed = await context.PokemonStats.MaxAsync(p => p.Speed),
            };
            return pokeStats;
        }
    }
}
