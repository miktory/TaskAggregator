# TaskAggregator

Пример веб-сервиса на **.NET 10**, демонстрирующий понимание **Clean Architecture** и **DDD**, а также умение работать с актуальным инструментарием.

## 1. Назначение

Сервис агрегирует «заявки» (`Request`) из разных систем Service Desk в единое хранилище.

- **RequestSource** — источник заявок (например, какой-либо Service Desk);
- **Request** — заявка.

Основной сценарий: POST запросом к API создаётся источник заявок -> через Excel-файл в систему загружаются заявки с привязкой к конкретному источнику заявок. 

Если заявка уже существует - данные **обновляются** актуальными значениями из файла.

## 2. Основные технологии, используемые в проекте
ASP.NET Core (.NET 10), PostgreSQL 17, Mediator, Mapster, FluentValidation, Scalar, xUnit, Testcontainers

## 3. Описание слоёв Clean Architecture

```
API             - Входная точка (контроллеры, DTO, DI, обработка ошибок, ...)
Application     - Сценарии использования 
Domain          - Бизнес-логика
Infrastructure  - Внешние адаптеры 
Shared          - Общие абстракции (коды ошибок, ArgumentValue, ...)
Tests           - Тесты 
```

Направление зависимостей: `API → Application/Infrastructure → Domain → Shared`.
Слой с тестами имеет доступ ко всем другим слоям (при необходимости также к их internals).

## 4. Запуск

### 4.1. Запуск backend

Требуется .NET SDK 10, PostgreSQL (можно через Docker).

1. Поднять PostgreSQL
2. Заполнить config файл (appsettings.json / appsettings.Development.json)
3. Запустить backend:
   ```bash
   dotnet run --project API
   ```
   или с указанием конкретного URL:
   ```bash
   dotnet run --project API --urls "http://localhost:5001"
   ``` 
P.S.: миграции применяются автоматически при старте приложения.

### 4.2. Запуск тестов

Интеграционные тесты поднимают свой PostgreSQL-контейнер — для их запуска нужен Docker.

1. Поднять Docker
2. Заполнить config файл (appsettings.json / appsettings.Development.json)
3. Запустить тесты:

```bash
dotnet test
```

## 5. API

API поддерживает версионирование. На данный момент реализована только одна версия - v1.
Версия указывается в URL: `/api/v1/...`. 
Ниже приведена основная информация по использованию API. 
Полный перечень запросов с описанием схем данных можно посмотреть через Scalar, либо напрямую запросив OpenAPI спецификацию.

Доступно **только в окружении Development**:
- Scalar: `/scalar`;
- OpenAPI JSON: `/openapi/v1.json`.

![Scalar request example](/assets/scalar_req_example.png)

### Импорт заявок из Excel

`POST /api/v1/integration/import/requests/from-excel`
`multipart/form-data`, поле `file` — Excel-файл (`.xls` / `.xlsx`).

Ожидаемая структура файла: первая строка — заголовки, далее строки данных.

| RequestSourceExternalId | ExternalId | Name            | Description                              |
|-------------------------|------------|-----------------|------------------------------------------|
| SD-1                    | REQ-1001   | Не работает VPN | Пользователь не может подключиться к VPN  |

- `RequestSourceExternalId`* — внешний идентификатор источника; источник должен существовать.
- `ExternalId`* — внешний идентификатор заявки.
- `Name`, `Description` — опциональны.

`*` — обязательные колонки.

Логика: если найдена заявка по связке (`RequestSourceExternalId`, `ExternalId`) — её `Name` и `Description`
  обновляются значениями из файла. Если заявка не найдена - она создаётся.

### Получение данных через GET запросы

`GET` запрос к коллекции без указания конкретного `id` возвращает offset-пагинированный список:

```json
{
    "entities": [
        {
            "id": 15,
            "externalId": "JIRA"
        },
        {
            "id": 16,
            "externalId": "NaumenSD"
        }
    ],
    "totalCount": 4,
    "pageSize": 2,
    "pageNumber": 1,
    "hasNextPage": true,
    "totalPages": 2
}
```
Для управления пагинацией используются query-параметры pageSize и pageNumber.
Пример: `?pageSize=50&pageNumber=1`.

`GET` по `id` возвращает конкретный экземпляр ресурса.

## 6. Обработка ошибок

### 6.1. Ошибки первичной валидации входных данных

Возникают при неверном формате входящего запроса (проверки модели/FluentValidation).
Ответ обязательно содержит:
- `status` — HTTP-код.

Дополнительные поля:
- `errors` — словарь ошибок валидации по каждому проблемному полю.

```json
{
    "status": 400,
    "errors": {
        "ExternalId": [
            "'External Id' должно быть заполнено."
        ]
    }
}
```

Ключ в errors — имя поля из модели, значение — массив сообщений.
Ошибка только с полем status может прийти, например, при обращении по несуществующему адресу.

`POST /api/v1/null`

```json
{
    "status": 404
}
```

### 6.2. Ошибки валидации внутренней логики

Возникают, когда запрос формально корректен, но нарушает бизнес-правило.
Подобные ошибки содержат всю необходимую информацию для формирования сообщений на клиенте.
Ответ содержит:

- `status` — HTTP-код;
- `detail` — сообщение, сформированное на стороне сервера для конкретного `code` (может быть `null`,
  если для кода не добавлено сообщение, либо произошла ошибка при его формировании);
- `code` — код ошибки;
- `args` — аргументы для данного кода (ключи фиксированы для кода, значения зависят от ситуации).

Пример (источник не найден по идентификатору):

```json
{
    "status": 404,
    "detail": "Сущность 'RequestSource' с Id '-1' не найдена.",
    "args": {
        "entityType": "RequestSource",
        "entityId": -1
    },
    "code": "A-1"
}
```

Коды:

| Code | Категория (ErrorKind)      | HTTP | Ситуация                                       |
|------|----------------------------|------|------------------------------------------------|
| D-0  | ConflictState              | 409  | заявка с таким ExternalId уже есть у источника  |
| D-1  | ConflictState              | 409  | источник с таким ExternalId уже существует      |
| A-0  | UnprocessableState         | 422  | файл повреждён / неподдерживаемый формат        |
| A-1  | RequestedResourceNotExists | 404  | сущность не найдена по Id                       |
| A-2  | UnprocessableState         | 422  | недопустимое значение в колонке Excel           |
| A-3  | UnprocessableState         | 422  | сущность не найдена по внешнему идентификатору  |
| A-4  | UnprocessableState         | 422  | ошибка сохранения в БД                          |
| A-5  | Validation                 | 400  | некорректные параметры пагинации                |

### 6.3. Необработанные ошибки

Все необработанные исключения, возникающие в приложении, преобразуются в ошибку одного вида.
Ответ содержит:
- `status` — HTTP-код;

Статус-код таких ошибок всегда 500.

## 7. Локализация

Сообщение `detail` для ошибок с кодом формируется с учётом культуры запроса (указывается в заголовке `Accept-Language`).
Поддерживаются `ru` и `en` (по умолчанию `en`). Ресурсы — в `API/Resources/Localization/ErrorMessages`
(`ErrorMessagesResource.resx` и `ErrorMessagesResource.ru.resx`).

## 8. Тесты

Для примера написано несколько тестов (Unit, Architecture, Integration).

## 9. Конфигурация

Настройки читаются из `appsettings.json` (и переопределяются `appsettings.{Environment}.json`, переменными окружения и т.д. — стандартный механизм `IConfiguration`).

**`appsettings.json`**

| Параметр | Назначение |
|---|---|
| `DatabaseConnection` | Секция настроек подключения к БД. Данные для подключения указываются в поле ConnectionString. Поле ConnectionString **обязательно** для заполнения. |
| `Logging` | Стандартные настройки логирования ASP.NET Core. |
| `AllowedHosts` | Стандартные настройки хостинга ASP.NET Core: `*` |

```json
{
  "DatabaseConnection": {
    "ConnectionString": "Host=localhost;Port=5432;Database=taskaggregator;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 10. Лицензия

MIT — см. `LICENSE`. Список сторонних компонентов — `THIRD-PARTY-NOTICES.md`.
