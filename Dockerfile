# Используем официальный образ .NET 9.0 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файлы проекта
COPY ["WebApplication3/WebApplication3.csproj", "WebApplication3/"]
RUN dotnet restore "WebApplication3/WebApplication3.csproj"

# Копируем весь исходный код
COPY . .
WORKDIR "/src/WebApplication3"

# Собираем приложение
RUN dotnet build "WebApplication3.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "WebApplication3.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ с runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Копируем базу данных
COPY WebApplication3/WebApplication3.db /app/WebApplication3.db

# Открываем порт 8080
EXPOSE 8080

# Запускаем приложение
ENTRYPOINT ["dotnet", "WebApplication3.dll"] 