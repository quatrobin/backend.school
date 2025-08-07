# Мониторинг с Grafana и Prometheus

## Обзор

Данный проект включает в себя полноценную систему мониторинга с использованием:
- **Prometheus** - сбор метрик
- **Grafana** - визуализация и дашборды
- **Node Exporter** - системные метрики

## Запуск системы мониторинга

### 1. Запуск всех сервисов
```bash
docker-compose up -d
```

### 2. Доступ к сервисам

| Сервис | URL | Описание |
|--------|-----|----------|
| **Grafana** | http://localhost:3001 | Основной интерфейс мониторинга |
| **Prometheus** | http://localhost:9090 | Сбор и хранение метрик |
| **Web API** | http://localhost:3000 | .NET приложение |
| **Kibana** | http://localhost:5601 | Логи и поиск |
| **SQLite Web** | http://localhost:8080 | Управление базой данных |

### 3. Вход в Grafana
- **URL**: http://localhost:3001
- **Логин**: `admin`
- **Пароль**: `admin123`

## Дашборды

### 1. System Monitoring Dashboard
Основной дашборд для мониторинга системы включает:
- **CPU Usage** - использование процессора
- **Memory Usage** - использование памяти
- **Disk Usage** - использование диска
- **Network Traffic** - сетевой трафик
- **HTTP Requests** - количество HTTP запросов
- **Response Time** - время отклика

### 2. .NET Application Dashboard
Специализированный дашборд для .NET приложения:
- **Request Rate** - скорость запросов
- **Error Rate** - частота ошибок
- **Response Time** - время отклика
- **Active Connections** - активные соединения
- **Memory Usage** - использование памяти приложением
- **CPU Usage** - использование CPU приложением

## Метрики

### Системные метрики (Node Exporter)
- CPU, Memory, Disk usage
- Network traffic
- System load
- File system statistics

### Метрики приложения (.NET)
- HTTP request rate
- Response times
- Error rates
- Memory usage
- CPU usage

## Настройка алертов

### 1. Создание алертов в Grafana
1. Откройте дашборд
2. Нажмите на панель
3. Выберите "Alert" → "Create Alert"
4. Настройте условия и уведомления

### 2. Примеры алертов
- **CPU > 80%** - высокое использование процессора
- **Memory > 85%** - высокое использование памяти
- **Error Rate > 5%** - высокая частота ошибок
- **Response Time > 1s** - медленные ответы

## Полезные запросы Prometheus

### Системные метрики
```promql
# CPU Usage
100 - (avg by (instance) (irate(node_cpu_seconds_total{mode="idle"}[5m])) * 100)

# Memory Usage
(node_memory_MemTotal_bytes - node_memory_MemAvailable_bytes) / node_memory_MemTotal_bytes * 100

# Disk Usage
(node_filesystem_size_bytes - node_filesystem_free_bytes) / node_filesystem_size_bytes * 100
```

### Метрики приложения
```promql
# Request Rate
rate(http_requests_total[5m])

# Error Rate
rate(http_requests_total{status=~"4..|5.."}[5m])

# Response Time (95th percentile)
histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m]))
```

## Устранение неполадок

### 1. Grafana не загружается
- Проверьте, что контейнер запущен: `docker-compose ps`
- Проверьте логи: `docker-compose logs grafana`

### 2. Нет данных в дашбордах
- Убедитесь, что Prometheus собирает метрики
- Проверьте настройки источника данных в Grafana
- Убедитесь, что Node Exporter работает

### 3. Метрики приложения не отображаются
- Проверьте, что .NET приложение запущено
- Убедитесь, что endpoint `/metrics` доступен
- Проверьте конфигурацию Prometheus

## Расширение мониторинга

### Добавление новых метрик
1. Добавьте метрики в код приложения
2. Обновите конфигурацию Prometheus
3. Создайте новые панели в Grafana

### Добавление новых дашбордов
1. Создайте JSON файл дашборда
2. Поместите в папку `grafana/dashboards/`
3. Перезапустите Grafana

## Производительность

### Рекомендации
- Используйте фильтры в запросах для оптимизации
- Настройте правильные интервалы обновления
- Мониторьте использование ресурсов самих сервисов мониторинга

### Масштабирование
- Для продакшена используйте внешние базы данных
- Настройте резервное копирование данных
- Рассмотрите использование Grafana Enterprise
