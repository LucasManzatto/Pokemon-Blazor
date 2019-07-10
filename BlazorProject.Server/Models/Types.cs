﻿using System;
using System.Collections.Generic;

namespace BlazorProject.Server.Models
{
    public partial class Types
    {
        public Types()
        {
            PokemonTypes = new HashSet<PokemonTypes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int GenerationId { get; set; }
        public int? DamageClassId { get; set; }

        public virtual DamageClass DamageClass { get; set; }
        public virtual Generation Generation { get; set; }
        public virtual ICollection<PokemonTypes> PokemonTypes { get; set; }
    }
}