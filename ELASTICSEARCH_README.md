# Elasticsearch в проекте School API

## Описание

В проект добавлен Elasticsearch для полнотекстового поиска по всем сущностям системы (пользователи, курсы, книги, уроки, задания).

## Конфигурация

### Docker Compose

Elasticsearch настроен в `docker-compose.yml`:

```yaml
elasticsearch:
  image: docker.elastic.co/elasticsearch/elasticsearch:8.12.0
  container_name: elasticsearch
  environment:
    - discovery.type=single-node
    - xpack.security.enabled=false
    - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
  ports:
    - "9200:9200"
    - "9300:9300"
  volumes:
    - elasticsearch_data:/usr/share/elasticsearch/data
```

### Настройки приложения

В `appsettings.json` добавлены настройки Elasticsearch:

```json
{
  "Elasticsearch": {
    "Url": "http://localhost:9200",
    "DefaultIndex": "school"
  }
}
```

## Запуск

1. Запустите все сервисы:
```bash
docker-compose up -d
```

2. Проверьте статус Elasticsearch:
```bash
curl http://localhost:9200
```

3. Создайте индексы (требуется авторизация Admin):
```bash
curl -X POST http://localhost:3000/api/search/setup \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## API Endpoints

### Поиск

- `GET /api/search/global?query=поисковый_запрос` - глобальный поиск по всем индексам
- `GET /api/search/{indexName}?query=поисковый_запрос` - поиск по конкретному индексу
- `GET /api/search/status` - проверка статуса Elasticsearch (без авторизации)
- `POST /api/search/setup` - создание индексов (только для Admin)

### Доступные индексы

- `users` - пользователи
- `courses` - курсы
- `books` - книги
- `lessons` - уроки
- `assignments` - задания

## Автоматическая индексация

При создании, обновлении или удалении сущностей они автоматически индексируются в Elasticsearch:

- **Создание**: данные автоматически добавляются в соответствующий индекс
- **Обновление**: данные обновляются в индексе
- **Удаление**: данные удаляются из индекса

## Примеры использования

### Глобальный поиск
```bash
curl "http://localhost:3000/api/search/global?query=программирование" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Поиск по книгам
```bash
curl "http://localhost:3000/api/search/books?query=python" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Проверка статуса
```bash
curl http://localhost:3000/api/search/status
```

## Мониторинг

### Elasticsearch API
- `http://localhost:9200` - основная информация
- `http://localhost:9200/_cat/indices` - список индексов
- `http://localhost:9200/_cat/health` - состояние кластера

### Kibana (опционально)

Для визуализации данных можно добавить Kibana в `docker-compose.yml`:

```yaml
kibana:
  image: docker.elastic.co/kibana/kibana:8.12.0
  container_name: kibana
  environment:
    - ELASTICSEARCH_HOSTS=http://elasticsearch:9200
  ports:
    - "5601:5601"
  depends_on:
    - elasticsearch
```

## Troubleshooting

### Elasticsearch не запускается
1. Проверьте доступность памяти (требуется минимум 2GB)
2. Увеличьте лимиты Docker:
```bash
docker system prune -a
```

### Ошибки подключения
1. Убедитесь, что Elasticsearch запущен: `docker-compose ps`
2. Проверьте логи: `docker-compose logs elasticsearch`
3. Убедитесь, что порт 9200 не занят другим процессом

### Индексы не создаются
1. Проверьте права доступа (требуется роль Admin)
2. Убедитесь, что Elasticsearch доступен из приложения
3. Проверьте настройки в `appsettings.json` 