FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.WebAPI/PetFamily.WebAPI.csproj", "src/PetFamily.WebAPI/"]
COPY ["src/Accounts/PetFamily.AccountsManagement.Application/PetFamily.AccountsManagement.Application.csproj", "src/Accounts/PetFamily.AccountsManagement.Application/"]
COPY ["src/Shared/PetFamily.Shared.Core/PetFamily.Shared.Core.csproj", "src/Shared/PetFamily.Shared.Core/"]
COPY ["src/Shared/PetFamily.Shared.SharedKernel/PetFamily.Shared.SharedKernel.csproj", "src/Shared/PetFamily.Shared.SharedKernel/"]
COPY ["src/Shared/PetFamily.Shared.Framework/PetFamily.Shared.Framework.csproj", "src/Shared/PetFamily.Shared.Framework/"]
COPY ["src/Accounts/PetFamily.AccountsManagement.Contracts/PetFamily.AccountsManagement.Contracts.csproj", "src/Accounts/PetFamily.AccountsManagement.Contracts/"]
COPY ["src/VolunteersManagement/PetFamily.VolunteersManagement.Application/PetFamily.VolunteersManagement.Application.csproj", "src/VolunteersManagement/PetFamily.VolunteersManagement.Application/"]
COPY ["src/BreedsManagement/PetFamily.BreedsManagement.Contracts/PetFamily.BreedsManagement.Contracts.csproj", "src/BreedsManagement/PetFamily.BreedsManagement.Contracts/"]
COPY ["src/VolunteersManagement/PetFamily.VolunteersManagement.Domain/PetFamily.VolunteersManagement.Domain.csproj", "src/VolunteersManagement/PetFamily.VolunteersManagement.Domain/"]
COPY ["src/Accounts/PetFamily.AccountsManagement.Domain/PetFamily.AccountsManagement.Domain.csproj", "src/Accounts/PetFamily.AccountsManagement.Domain/"]
COPY ["src/Accounts/PetFamily.AccountsManagement.Infrastructure/PetFamily.AccountsManagement.Infrastructure.csproj", "src/Accounts/PetFamily.AccountsManagement.Infrastructure/"]
COPY ["src/Accounts/PetFamily.AccountsManagement.Presentation/PetFamily.AccountsManagement.Presentation.csproj", "src/Accounts/PetFamily.AccountsManagement.Presentation/"]
COPY ["src/BreedsManagement/PetFamily.BreedsManagement.Application/PetFamily.BreedsManagement.Application.csproj", "src/BreedsManagement/PetFamily.BreedsManagement.Application/"]
COPY ["src/BreedsManagement/PetFamily.BreedsManagement.Domain/PetFamily.BreedsManagement.Domain.csproj", "src/BreedsManagement/PetFamily.BreedsManagement.Domain/"]
COPY ["src/BreedsManagement/PetFamily.BreedsManagement.Infrastructure/PetFamily.BreedsManagement.Infrastructure.csproj", "src/BreedsManagement/PetFamily.BreedsManagement.Infrastructure/"]
COPY ["src/BreedsManagement/PetFamily.BreedsManagement.Presentation/PetFamily.BreedsManagement.Presentation.csproj", "src/BreedsManagement/PetFamily.BreedsManagement.Presentation/"]
COPY ["src/VolunteersManagement/PetFamily.VolunteersManagement.Infrastructure/PetFamily.VolunteersManagement.Infrastructure.csproj", "src/VolunteersManagement/PetFamily.VolunteersManagement.Infrastructure/"]
COPY ["src/VolunteersManagement/PetFamily.VolunteersManagement.Presentation/PetFamily.VolunteersManagement.Presentation.csproj", "src/VolunteersManagement/PetFamily.VolunteersManagement.Presentation/"]
COPY ["src/VolunteersManagement/PetFamily.VolunteersManagement.Contracts/PetFamily.VolunteersManagement.Contracts.csproj", "src/VolunteersManagement/PetFamily.VolunteersManagement.Contracts/"]
RUN dotnet restore "src/PetFamily.WebAPI/PetFamily.WebAPI.csproj"
COPY . .
COPY src/PetFamily.WebAPI/etc/.env ./etc/.env
WORKDIR "/src/src/PetFamily.WebAPI"
RUN dotnet build "PetFamily.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetFamily.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.WebAPI.dll"]
