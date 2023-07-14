using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;
using Application.Models.Identity;
using Application.Models.Item;
using Domain.Entities.Identity;
using Domain.Entities.Items;
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

            TypeAdapterConfig<AddCollectionRequest, Collection>
                .NewConfig()
                .Ignore(nameof(AddCollectionRequest.Fields));

            TypeAdapterConfig<Collection, CollectionDto>
                .NewConfig()
                .Map(dest => dest.Author, src => src.User);

            TypeAdapterConfig<CollectionDto, EditCollectionRequest>
                .NewConfig()
                .Map(dest => dest.CollectionThemeId, src => src.Theme.Id);

            TypeAdapterConfig<CollectionFieldDto, CollectionField>
                .NewConfig()
                .Map(dest => dest.CollectionFieldTypeId, src => src.FieldType.Id);

            TypeAdapterConfig<Item, ItemDto>
                .NewConfig()
                .Map(dest => dest.Likes, src => src.Likes.Count);

            TypeAdapterConfig<ItemDto, Item>
                .NewConfig()
                .Map(dest => dest.Likes, src => new List<Like>());

            TypeAdapterConfig<Comment, CommentDto>
                .NewConfig()
                .Map(dest => dest.Author, src => src.User);
        }
    }
}
