# Настройка Elasticsearch в School API

## ✅ Успешно настроено

Elasticsearch успешно интегрирован в проект School API!

### Что было сделано:

1. **Добавлен Elasticsearch в Docker Compose**
   - Версия: 7.17.0
   - Порт: 9200 (HTTP), 9300 (Transport)
   - Режим: single-node
   - Безопасность: отключена для разработки

2. **Добавлены NuGet пакеты**
   - NEST 7.17.0
   - Elasticsearch.Net 7.17.0

3. **Создан сервис Elasticsearch**
   - `IElasticsearchService` - интерфейс
   - `ElasticsearchService` - реализация
   - Автоматическая индексация при CRUD операциях

4. **Создан контроллер поиска**
   - Глобальный поиск по всем индексам
   - Поиск по конкретным типам данных
   - Проверка статуса
   - Создание индексов

5. **Обновлен BookService**
   - Автоматическая индексация при создании/обновлении/удалении книг

## 🚀 Запуск

```bash
# Запуск всех сервисов
docker-compose up -d

# Проверка статуса
curl http://localhost:9200
curl http://localhost:3000/api/search/status
```

## 📊 Доступные endpoints

### Поиск
- `GET /api/search/global?query=поисковый_запрос` - глобальный поиск
- `GET /api/search/{indexName}?query=поисковый_запрос` - поиск по индексу
- `GET /api/search/status` - статус Elasticsearch
- `POST /api/search/setup` - создание индексов (Admin)

### Индексы
- `users` - пользователи
- `courses` - курсы  
- `books` - книги
- `lessons` - уроки
- `assignments` - задания

## 🔧 Конфигурация

### Docker Compose
```yaml
elasticsearch:
  image: elasticsearch:7.17.0
  ports:
    - "9200:9200"
    - "9300:9300"
  environment:
    - discovery.type=single-node
    - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
    - xpack.security.enabled=false
```

### appsettings.json
```json
{
  "Elasticsearch": {
    "Url": "http://localhost:9200",
    "DefaultIndex": "school"
  }
}
```

## 📝 Примеры использования

### Проверка статуса
```bash
curl http://localhost:3000/api/search/status
```

### Создание индексов
```bash
curl -X POST http://localhost:3000/api/search/setup \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Глобальный поиск
```bash
curl "http://localhost:3000/api/search/global?query=программирование" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## 🎯 Следующие шаги

1. **Добавить индексацию в другие сервисы**
   - CourseService
   - LessonService  
   - UserService
   - AssignmentService

2. **Настроить Kibana для визуализации**
   ```yaml
   kibana:
     image: kibana:7.17.0
     ports:
       - "5601:5601"
   ```

3. **Добавить более сложные запросы**
   - Фильтрация по датам
   - Агрегации
   - Фасетный поиск

## 🔍 Мониторинг

- Elasticsearch: http://localhost:9200
- API: http://localhost:3000
- Индексы: `curl http://localhost:9200/_cat/indices`

## ✅ Статус

- ✅ Elasticsearch запущен и работает
- ✅ API подключен к Elasticsearch
- ✅ Базовый поиск работает
- ✅ Автоматическая индексация книг настроена
- ⏳ Остальные сервисы требуют обновления 