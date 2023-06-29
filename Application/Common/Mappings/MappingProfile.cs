﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Identity;
using Domain.Entities.Identity;
using Mapster;

namespace Application.Common.Mappings {

    public static class MappingProfile {

        public static void ApplyMappings() {
            TypeAdapterConfig<SignUpExternalRequest, ApplicationUser>
                .NewConfig()
                .Map(dest => dest.UserName, src => src.Username);

            TypeAdapterConfig<SignUpRequest, ApplicationUser>
                .NewConfig()
                .Map(dest => dest.UserName, src => src.Username);
        }
    }
}
