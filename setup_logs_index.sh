#!/bin/bash

echo "=== Настройка индекса логов в Elasticsearch ==="

# Ждем, пока Elasticsearch будет готов
echo "Ожидание готовности Elasticsearch..."
until curl -s http://localhost:9200 > /dev/null; do
    echo "Elasticsearch еще не готов, ждем..."
    sleep 5
done

echo "Elasticsearch готов!"

# Создаем шаблон для логов
echo "Создание шаблона для логов..."
curl -X PUT "localhost:9200/_template/school-logs" \
  -H "Content-Type: application/json" \
  -d '{
    "index_patterns": ["school-logs-*"],
    "settings": {
      "number_of_shards": 1,
      "number_of_replicas": 0,
      "analysis": {
        "analyzer": {
          "log_analyzer": {
            "type": "custom",
            "tokenizer": "standard",
            "filter": ["lowercase", "stop"]
          }
        }
      }
    },
    "mappings": {
      "properties": {
        "@timestamp": {
          "type": "date"
        },
        "level": {
          "type": "keyword"
        },
        "message": {
          "type": "text",
          "analyzer": "log_analyzer"
        },
        "IndexName": {
          "type": "keyword"
        },
        "DocumentType": {
          "type": "keyword"
        },
        "DocumentId": {
          "type": "keyword"
        },
        "SearchTerm": {
          "type": "text"
        },
        "Query": {
          "type": "text"
        },
        "BookId": {
          "type": "integer"
        },
        "BookTitle": {
          "type": "text"
        },
        "BookAuthor": {
          "type": "text"
        },
        "CourseId": {
          "type": "integer"
        },
        "CourseName": {
          "type": "text"
        },
        "UserId": {
          "type": "integer"
        },
        "Email": {
          "type": "keyword"
        },
        "LessonId": {
          "type": "integer"
        },
        "LessonTitle": {
          "type": "text"
        },
        "AssignmentId": {
          "type": "integer"
        },
        "AssignmentTitle": {
          "type": "text"
        },
        "Count": {
          "type": "integer"
        },
        "TotalResults": {
          "type": "integer"
        },
        "Error": {
          "type": "text"
        }
      }
    }
  }'

echo "Шаблон создан!"

# Создаем индекс для сегодняшних логов
TODAY=$(date +%Y.%m.%d)
echo "Создание индекса логов для сегодня: school-logs-$TODAY"
curl -X PUT "localhost:9200/school-logs-$TODAY"

echo "=== Настройка завершена ==="
echo "Индекс логов: school-logs-$TODAY"
echo "Kibana доступен на: http://localhost:5601" 