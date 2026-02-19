# Plotline Backend

Демонстрационный вариант. Небольшое приложение на фреймворке ASP.NET Core 8. Реализация чистой архитектуры с разделением слоев и инверсией зависимостей.
Цель: написание серверного приложения, взаимодействующего с БД, с легким горизонтальным масштабированием, базовой валидацией(Data Annotations), безопасной аутентификацией с refresh-токенами и логированием.

---

## ![#](https://img.shields.io/badge/stack-технологии-blue) Стек технологий

| Технология | Назначение |
|------------|------------|
| ![ASP.NET](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?logo=dotnet) | Фреймворк для построения Web API |
| ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?logo=postgresql) | Реляционная база данных |
| ![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?logo=dotnet) | ORM для работы с БД |
| ![JWT](https://img.shields.io/badge/JWT-Auth-black?logo=jsonwebtokens) | Аутентификация |
| ![AutoMapper](https://img.shields.io/badge/AutoMapper-12.0-ff69b4) | Маппинг между моделями и DTO |
| ![Serilog](https://img.shields.io/badge/Serilog-Logging-2C2C2C?logo=serilog) | Структурированное логирование |
| ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger) | Документация и тестирование API |
| ![Docker](https://img.shields.io/badge/Docker-Container-2496ED?logo=docker) | Контейнеризация |

---

## ![#](https://img.shields.io/badge/arch-архитектура-purple) Архитектура проекта

- ![API](https://img.shields.io/badge/API-Plotline.API-blue) — входная точка, контроллеры, middleware  
- ![Application](https://img.shields.io/badge/Application-Plotline.Application-green) — бизнес-логика, сервисы, DTO  
- ![Core](https://img.shields.io/badge/Core-Plotline.Core-red) — модели, интерфейсы, доменные сущности  
- ![Infrastructure](https://img.shields.io/badge/Infrastructure-Plotline.Infrastructure-orange) — доступ к данным, репозитории, миграции  

---

## ![#](https://img.shields.io/badge/run-запуск-success) Запуск проекта

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

### ![#](https://img.shields.io/badge/req-требования-yellow) Требования
- ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet) — [Скачать](https://dotnet.microsoft.com/download)
- ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?logo=postgresql) — [Скачать](https://www.postgresql.org/download/)
- ![Docker](https://img.shields.io/badge/Docker-Desktop-2496ED?logo=docker) — [Скачать](https://www.docker.com/products/docker-desktop/) (опционально)
