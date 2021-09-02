# aspnet:5.0 vs aspnet:5.0-buster-slim vs aspnet:5.0-apline
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

# This shortens the time needed to build the image and reduces the download size 
# since the image layer dependencies are only rebuilt if the dependencies get changed.
COPY ["Sandbox.TelegramBot.Webhook/Sandbox.TelegramBot.Webhook.csproj", "Sandbox.TelegramBot.Webhook/"]
COPY ["Sandbox.TelegramBot.Core/Sandbox.TelegramBot.Core.csproj", "Sandbox.TelegramBot.Core/"]
RUN dotnet restore "Sandbox.TelegramBot.Webhook/Sandbox.TelegramBot.Webhook.csproj"

# COPY . .
COPY ["Sandbox.TelegramBot.Webhook/", "Sandbox.TelegramBot.Webhook/"]
COPY ["Sandbox.TelegramBot.Core/", "Sandbox.TelegramBot.Core/"]
WORKDIR "/src/Sandbox.TelegramBot.Webhook"
RUN dotnet build "Sandbox.TelegramBot.Webhook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sandbox.TelegramBot.Webhook.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sandbox.TelegramBot.Webhook.dll"]