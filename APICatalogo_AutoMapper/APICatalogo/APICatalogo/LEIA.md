# AutoMapper

Serve para transferir dados de DTO para Model e vice versa.

```bash
    dotnet new package AutoMapper.Extensions.Microsoft.DependencyInjection;
```

```cs
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Model, ModelDTO>().ReverseMap();
        }
    }


    var mappingConfig = new MapperConfiguration(mc => {
        mc.AddProfile(new MappingProfile());
    });

    IMapper mapper = mappingConfig.CreateMapper();
    builder.Services.AddScoped(mapper);
```