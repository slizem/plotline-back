# Plotline Backend

Демонстрационный вариант. Небольшое приложение на фреймворке ASP.NET Core 8. Реализация чистой архитектуры с разделением слоев и инверсией зависимостей.
Цель: написание серверного приложения, взаимодействующего с БД, с легким горизонтальным масштабированием, базовой валидацией(Data Annotations), безопасной аутентификацией с refresh-токенами и логированием.

---

## Стек технологий

| Технология | Назначение |
|------------|------------|
| ASP.NET | Фреймворк для построения Web API |
| PostgreSQL | Реляционная база данных |
| EF Core | ORM для работы с БД |
| JWT | Аутентификация |
| AutoMapper | Маппинг между моделями и DTO |
| Serilog | Структурированное логирование |
| Swagger | Документация и тестирование API |
| Docker | Контейнеризация |

---

## Архитектура проекта

- ![API](https://img.shields.io/badge/API-blue) — входная точка, контроллеры, middleware  
- ![Application](https://img.shields.io/badge/Application-blue) — бизнес-логика, сервисы, DTO  
- ![Core](https://img.shields.io/badge/Core-blue) — модели, интерфейсы, доменные сущности  
- ![Infrastructure](https://img.shields.io/badge/Infrastructure-blue) — доступ к данным, репозитории, миграции  

---

## Запуск проекта

Запустить проект можно несколькими способами:
1. Из IDE.
2. Через собранный исполняемый файл или командой в консоли: cd Plotline.API && dotnet run.
3. Командой, используя докер-файл(секрет должен быть длинным, желательно, 32 символа):
    ```bash
    docker build -t plotline-api -f PlotlineAPI/Dockerfile .
    docker run -d \
      --name plotline-api \
      -p 8080:8080 \
      -p 8081:8081 \
      -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=;Username=;Password=" \
      -e Jwt__Secret="" \
      -e Jwt__Issuer="PlotlineAPI" \
      -e Jwt__Audience="PlotlineClient" \
      plotline-api:latest
    ```

  ---

### Требования
- ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4) — [Скачать](https://dotnet.microsoft.com/download)
- ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?logo=postgresql) — [Скачать](https://www.postgresql.org/download/)
- ![Docker](https://img.shields.io/badge/Docker-Desktop-2496ED?logo=docker) — [Скачать](https://www.docker.com/products/docker-desktop/) (опционально)
