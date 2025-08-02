#!/bin/bash

# Скрипт для запуска веб-интерфейсов базы данных
echo "🚀 Запуск веб-интерфейсов для работы с базой данных..."

# Проверяем, существует ли файл базы данных
if [ ! -f "WebApplication3/WebApplication3.db" ]; then
    echo "⚠️  Файл базы данных не найден. Создаем базу данных..."
    cd WebApplication3
    dotnet ef database update
    cd ..
fi

# Запускаем только веб-интерфейсы для базы данных
echo "📊 Запуск SQLite Web..."
docker-compose up -d sqlite-web

echo "🔧 Запуск Adminer..."
docker-compose up -d adminer

echo "💻 Запуск DBeaver Community..."
docker-compose up -d dbeaver

echo ""
echo "✅ Веб-интерфейсы запущены!"
echo ""
echo "🌐 Доступные интерфейсы:"
echo "   • SQLite Web: http://localhost:8080"
echo "   • Adminer: http://localhost:8081"
echo "   • DBeaver Community: http://localhost:8978"
echo ""
echo "📋 Полезные команды:"
echo "   • Просмотр логов: docker-compose logs sqlite-web"
echo "   • Остановка: docker-compose stop sqlite-web adminer dbeaver"
echo "   • Перезапуск: docker-compose restart sqlite-web"
echo ""
echo "📖 Подробная документация: DATABASE_WEB_INTERFACES.md" 