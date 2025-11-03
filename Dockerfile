# 1️⃣ Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Collab_Platform.sln .
COPY Collab_Platform.ApplicationLayer/Collab_Platform.ApplicationLayer.csproj ./Collab_Platform.ApplicationLayer/
COPY Collab_Platform.DomainLayer/Collab_Platform.DomainLayer.csproj ./Collab_Platform.DomainLayer/
COPY Collab_Platform.InfrastructureLayer/Collab_Platform.InfrastructureLayer.csproj ./Collab_Platform.InfrastructureLayer/
COPY Collab_Platform.PresentationLayer/Collab_Platform.PresentationLayer.csproj ./Collab_Platform.PresentationLayer/

# Restore all dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Publish the PresentationLayer project (this will include references to other projects)
WORKDIR /src/Collab_Platform.PresentationLayer
RUN dotnet publish -c Release -o /app/out

# 2️⃣ Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose port (adjust if needed)
EXPOSE 5000
ENTRYPOINT ["dotnet", "Collab_Platform.PresentationLayer.dll"]
