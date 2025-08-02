#!/bin/bash

echo "=== Тестирование School API с Elasticsearch, Kibana и логированием ==="

# Проверка статуса Elasticsearch
echo "1. Проверка статуса Elasticsearch..."
curl -s http://localhost:9200 | jq .

# Проверка статуса API
echo -e "\n2. Проверка статуса API..."
curl -s http://localhost:3000/api/search/status | jq .

# Проверка Swagger
echo -e "\n3. Проверка Swagger..."
curl -s -I http://localhost:3000/swagger/index.html | head -1

# Проверка индексов Elasticsearch
echo -e "\n4. Проверка индексов Elasticsearch..."
curl -s http://localhost:9200/_cat/indices

# Проверка индекса логов
echo -e "\n5. Проверка индекса логов..."
TODAY=$(date +%Y.%m.%d)
curl -s "http://localhost:9200/school-logs-$TODAY/_count" | jq .

# Проверка Kibana
echo -e "\n6. Проверка Kibana..."
curl -s -I http://localhost:5601 | head -1

# Проверка контейнеров
echo -e "\n7. Статус контейнеров..."
docker-compose ps

# Тестовый запрос для генерации логов
echo -e "\n8. Генерация тестовых логов..."
curl -s "http://localhost:3000/api/search/global?query=test" > /dev/null

echo -e "\n=== Тестирование завершено ==="
echo "🌐 Доступные сервисы:"
echo "   • API: http://localhost:3000"
echo "   • Swagger UI: http://localhost:3000/swagger"
echo "   • Elasticsearch: http://localhost:9200"
echo "   • Kibana: http://localhost:5601"
echo ""
echo "📊 Логирование:"
echo "   • Индекс логов: school-logs-$TODAY"
echo "   • Kibana Discover: http://localhost:5601/app/discover"
echo "   • Настройка индекса: ./setup_logs_index.sh"
echo ""
echo "📚 Документация:"
echo "   • API Documentation: API_DOCUMENTATION.md"
echo "   • Elasticsearch Setup: ELASTICSEARCH_SETUP.md"
echo "   • Elasticsearch README: ELASTICSEARCH_README.md" 