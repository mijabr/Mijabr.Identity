FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore lib/Mijabr.Identity/Mijabr.Identity.csproj
RUN dotnet publish lib/Mijabr.Identity/Mijabr.Identity.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod 0755 .
ENTRYPOINT ["dotnet", "Mijabr.Identity.dll"]
