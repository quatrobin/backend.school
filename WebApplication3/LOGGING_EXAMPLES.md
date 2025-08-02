# Примеры использования логирования в проекте

## Настройка логирования

Проект использует Serilog для логирования с настройкой в `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "WebApplication3.Services.Implementations.ElasticsearchService": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/app-.txt",
          "rollingInterval": "Day"
        }
      },
      {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://localhost:9200",
          "indexFormat": "school-logs-{0:yyyy.MM.dd}"
        }
      }
    ]
  }
}
```

## Уровни логирования

- **Debug** - детальная отладочная информация
- **Information** - общая информация о работе приложения
- **Warning** - предупреждения, которые не критичны
- **Error** - ошибки, которые требуют внимания
- **Fatal** - критические ошибки, которые могут привести к остановке приложения

## Примеры использования в коде

### Базовое логирование

```csharp
using Serilog;

// Информационное сообщение
Log.Information("Пользователь {UserId} выполнил действие {Action}", userId, action);

// Предупреждение
Log.Warning("Необычная ситуация: {Details}", details);

// Ошибка с исключением
try
{
    // код
}
catch (Exception ex)
{
    Log.Error(ex, "Ошибка при выполнении операции {Operation}", operationName);
}
```

### Структурированное логирование

```csharp
// Логирование с контекстом
Log.Information("HTTP {Method} {Path} начат. IP: {IP}, User-Agent: {UserAgent}", 
    context.Request.Method, 
    context.Request.Path, 
    context.Connection.RemoteIpAddress,
    context.Request.Headers["User-Agent"].ToString());
```

### Логирование в ElasticsearchService

```csharp
public async Task<bool> IndexDocumentAsync<T>(T document, string indexName) where T : class
{
    try
    {
        Log.Information("Индексация документа в Elasticsearch. Индекс: {IndexName}, Тип: {DocumentType}", 
            indexName, typeof(T).Name);
        
        var response = await _elasticClient.IndexAsync(document, i => i.Index(indexName));
        
        if (response.IsValid)
        {
            Log.Information("Документ успешно проиндексирован. ID: {DocumentId}, Индекс: {IndexName}", 
                response.Id, indexName);
            return true;
        }
        else
        {
            Log.Warning("Ошибка индексации документа. Индекс: {IndexName}, Ошибка: {Error}", 
                indexName, response.ServerError?.Error?.Reason);
            return false;
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Исключение при индексации документа. Индекс: {IndexName}, Тип: {DocumentType}", 
            indexName, typeof(T).Name);
        return false;
    }
}
```

## API для тестирования логирования

### Проверка здоровья Elasticsearch
```http
GET /api/elasticsearch/health
Authorization: Bearer {token}
```

### Получение информации о Elasticsearch
```http
GET /api/elasticsearch/info
Authorization: Bearer {token}
```

### Тестовое логирование
```http
POST /api/elasticsearch/test-logging
Authorization: Bearer {token}
Content-Type: application/json

{
  "level": "Error",
  "message": "Тестовое сообщение об ошибке"
}
```

## Файлы логов

- `logs/app-{date}.txt` - общие логи приложения
- `logs/elasticsearch-errors-{date}.txt` - только ошибки Elasticsearch
- Elasticsearch индекс `school-logs-{date}` - все логи в Elasticsearch

## Мониторинг логов

### В Kibana
1. Откройте Kibana: http://localhost:5601
2. Перейдите в Discover
3. Выберите индекс `school-logs-*`
4. Настройте фильтры для поиска нужных логов

### Поиск ошибок Elasticsearch
```json
{
  "query": {
    "bool": {
      "must": [
        { "match": { "Level": "Error" } },
        { "match": { "SourceContext": "WebApplication3.Services.Implementations.ElasticsearchService" } }
      ]
    }
  }
}
```

## Middleware для логирования HTTP запросов

Автоматически логирует все HTTP запросы с информацией о:
- Методе запроса
- Пути
- IP адресе
- User-Agent
- Времени выполнения
- Статус коде ответа

## Рекомендации

1. **Используйте структурированное логирование** - передавайте параметры как аргументы, а не в строке
2. **Логируйте исключения** - всегда передавайте исключение в Log.Error
3. **Добавляйте контекст** - включайте ID пользователя, ID операции и другие важные данные
4. **Не логируйте чувствительные данные** - пароли, токены, персональные данные
5. **Используйте соответствующие уровни** - не используйте Error для обычных ситуаций 