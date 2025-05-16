![.NET](https://img.shields.io/badge/.NET-8-blue )
![gRPC](https://img.shields.io/badge/gRPC-orange )
![Kafka](https://img.shields.io/badge/Kafka-lightgrey )
![MongoDB](https://img.shields.io/badge/MongoDB-green )
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-blueviolet )
![Redis](https://img.shields.io/badge/Redis-red )

# 🚀 Нагруженный сервис с RateLimit

> Распределённая система на C#, включающая gRPC-сервисы, Kafka, Redis, MongoDB и механизмы ограничения частоты запросов.

**Этот проект демонстрирует реализацию распределённой системы из нескольких микросервисов, обменивающихся данными через Apache Kafka, использующих кеширование в Redis, работу с PostgreSQL и MongoDB, а также механизмы ограничения частоты запросов.**

---

## 🧱 Архитектура

Проект состоит из следующих компонентов:

| Сервис | Описание |
|--------|----------|
| `UserService` | gRPC-сервис для работы с пользователями через PostgreSQL |
| `WriterService` | Управляет лимитами RPM (Rate Limits) через MongoDB |
| `ReaderService` | Загружает и отслеживает лимиты в реальном времени через MongoDB Change Streams |
| `Redis` | Хранение флагов о превышении лимитов |
| `gRPC Interceptors` | Реализация авторизации и проверки Rate Limit на уровне запросов |

---

## 🚀 Технологии

- **.NET 8**
- **gRPC** (с Interceptor'ами)
- **PostgreSQL**
- **MongoDB**
- **Apache Kafka** (Confluent.Kafka)
- **Redis**
- **Dapper** (для работы с PostgreSQL)
- **MongoDb.Driver**
- **IMemoryCache**
- **FluentValidation**
- **IHostedService, IAsyncEnumerable**
- **Dependency Injection**

---

## 🔨 Возможности

- Полноценный CRUD над пользователями через gRPC и PostgreSQL.
- Создание и управление лимитами RPM через MongoDB.
- Автоматическая загрузка лимитов в память при старте сервиса батчами по 1000 записей.
- Отслеживание изменений в MongoDB через Change Streams.
- Асинхронное взаимодействие между сервисами через Apache Kafka.
- Кеширование данных в Redis и IMemoryCache.
- Проверка авторизации и ограничений на уровне gRPC Interceptor’ов.
  
---

## 🧪 Тестирование

- Все методы gRPC протестированы с помощью **Postman**.
- Работа с Kafka проверена через консьюмеров и продьюсеров.
- Валидация входных данных выполнена через FluentValidation.
- Модели уровня Domain, DbModel и Request/Response строго разделены.

---

