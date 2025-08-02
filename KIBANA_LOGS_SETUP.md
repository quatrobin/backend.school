# 📊 Настройка Kibana для отображения логов

## 🎯 Цель

Настроить Kibana для визуализации логов приложения School API, которые автоматически отправляются в Elasticsearch.

## 🔧 Что уже настроено

### ✅ Логирование в приложении
- **Serilog** настроен для отправки логов в Elasticsearch
- **Структурированное логирование** с контекстными данными
- **Автоматическая индексация** логов в `school-logs-YYYY.MM.DD`

### ✅ Elasticsearch
- **Шаблон индекса** `school-logs` создан
- **Маппинг полей** настроен для всех типов логов
- **Индекс для сегодня** `school-logs-2025.08.02` создан

### ✅ Kibana
- **Конфигурация** `kibana.yml` настроена
- **Контейнер** запущен на порту 5601

## 🚀 Пошаговая настройка Kibana

### 1. Откройте Kibana
```
http://localhost:5601
```

### 2. Создайте Index Pattern

1. Перейдите в **Stack Management** → **Index Patterns**
2. Нажмите **Create index pattern**
3. Введите **Index pattern name**: `school-logs-*`
4. Нажмите **Next step**
5. Выберите **Time field**: `@timestamp`
6. Нажмите **Create index pattern**

### 3. Настройте Discover для просмотра логов

1. Перейдите в **Discover**
2. Выберите **Index pattern**: `school-logs-*`
3. Настройте **Time range** (например, последние 24 часа)
4. Нажмите **Refresh**

### 4. Создайте дашборд

#### 4.1 Создайте визуализацию "Logs Overview"

1. Перейдите в **Visualize Library**
2. Нажмите **Create visualization**
3. Выберите **Data Table**
4. Выберите **Index pattern**: `school-logs-*`
5. Настройте метрики:
   - **Metric**: Count
   - **Bucket**: Split Rows
   - **Aggregation**: Terms
   - **Field**: `level` (или `level.raw` если доступно)
6. Сохраните как "Log Levels"

#### 4.2 Создайте визуализацию "Elasticsearch Operations"

1. Создайте новую визуализацию **Data Table**
2. Настройте метрики:
   - **Metric**: Count
   - **Bucket**: Split Rows
   - **Aggregation**: Terms
   - **Field**: `message` (или `message.raw` если доступно)
   - **Filter**: `message: "Индексация" OR message: "Поиск" OR message: "Удаление"`
3. Сохраните как "ES Operations"

#### 4.3 Создайте визуализацию "API Requests"

1. Создайте **Line Chart**
2. Настройте:
   - **Y-axis**: Count
   - **X-axis**: Date Histogram
   - **Field**: `@timestamp`
   - **Filter**: `message: "Запрос"`
3. Сохраните как "API Requests Over Time"

#### 4.4 Создайте визуализацию "Errors"

1. Создайте **Pie Chart**
2. Настройте:
   - **Slice Size**: Count
   - **Slice Size**: Terms
   - **Field**: `message` (или `message.raw` если доступно)
   - **Filter**: `level: "Error"`
3. Сохраните как "Error Types"

### 5. Создайте дашборд

1. Перейдите в **Dashboard**
2. Нажмите **Create dashboard**
3. Добавьте все созданные визуализации:
   - Log Levels
   - ES Operations
   - API Requests Over Time
   - Error Types
4. Сохраните как "School API Logs Dashboard"

## 📊 Полезные запросы для Discover

### Все логи за последний час
```
@timestamp:[now-1h TO now]
```

### Только ошибки
```
level: "Error"
```

### Логи Elasticsearch операций
```
message: "Индексация" OR message: "Поиск" OR message: "Удаление"
```

### Логи конкретной книги
```
BookId: 1
```

### Логи поиска
```
SearchTerm: "программирование"
```

### Логи API запросов
```
message: "Запрос"
```

## 🔍 Как найти правильные названия полей

### 1. В Discover
1. Перейдите в **Discover**
2. Выберите **Index pattern**: `school-logs-*`
3. В левой панели нажмите **Add** рядом с полем
4. Посмотрите на доступные поля в списке

### 2. В Index Pattern
1. Перейдите в **Stack Management** → **Index Patterns**
2. Выберите `school-logs-*`
3. Посмотрите на список полей и их типы

### 3. Через Elasticsearch API
```bash
# Получить маппинг индекса
curl "http://localhost:9200/school-logs-$(date +%Y.%m.%d)/_mapping" | jq .
```

## 🎨 Настройка внешнего вида

### Цветовая схема для уровней логов
- **Information**: Зеленый
- **Warning**: Желтый  
- **Error**: Красный
- **Debug**: Синий

### Полезные поля для отображения
- `@timestamp` - время события
- `level` - уровень логирования
- `message` - сообщение
- `IndexName` - название индекса Elasticsearch
- `DocumentType` - тип документа
- `BookId`, `BookTitle` - информация о книгах
- `SearchTerm`, `Query` - поисковые запросы
- `Count`, `TotalResults` - статистика

## 🔍 Мониторинг в реальном времени

### 1. Настройте Auto-refresh
1. В **Discover** нажмите **Auto-refresh**
2. Выберите интервал (например, 10 секунд)

### 2. Создайте алерты
1. Перейдите в **Stack Management** → **Rules and Alerts**
2. Создайте правило для отслеживания ошибок
3. Настройте уведомления

### 3. Настройте мониторинг производительности
1. Создайте визуализацию времени ответа API
2. Настройте алерты при медленных запросах

## 🛠️ Устранение неполадок

### Логи не отображаются
1. Проверьте, что индекс создан: `curl http://localhost:9200/_cat/indices`
2. Проверьте шаблон: `curl http://localhost:9200/_template/school-logs`
3. Проверьте логи приложения: `docker-compose logs webapi`

### Kibana не подключается к Elasticsearch
1. Проверьте статус Elasticsearch: `curl http://localhost:9200`
2. Проверьте конфигурацию Kibana: `docker-compose logs kibana`

### Поля не найдены
1. Проверьте маппинг индекса: `curl "http://localhost:9200/school-logs-$(date +%Y.%m.%d)/_mapping"`
2. Убедитесь, что логи записались: `curl "http://localhost:9200/school-logs-$(date +%Y.%m.%d)/_count"`
3. Проверьте, что Index Pattern создан правильно

### Логи не отправляются в Elasticsearch
1. Проверьте настройки Serilog в `appsettings.json`
2. Проверьте подключение к Elasticsearch в коде
3. Проверьте логи приложения на ошибки

## 📈 Расширенные возможности

### 1. Создание кастомных визуализаций
- **Heat Map** для анализа активности по времени
- **Tag Cloud** для частых поисковых запросов
- **Gauge** для мониторинга производительности

### 2. Настройка алертов
- Алерт при большом количестве ошибок
- Алерт при медленных запросах
- Алерт при недоступности Elasticsearch

### 3. Экспорт данных
- Экспорт логов в CSV
- Создание отчетов
- Интеграция с внешними системами

## 🎯 Результат

После настройки у вас будет:
- ✅ **Дашборд логов** в реальном времени
- ✅ **Структурированное логирование** всех операций
- ✅ **Мониторинг производительности** API
- ✅ **Отслеживание ошибок** и проблем
- ✅ **Аналитика использования** Elasticsearch

**Kibana готов к использованию для мониторинга логов!** 🚀 