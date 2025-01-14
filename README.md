# Сервис DailyRates и его тесты

Это пример интеграционного теста для backend-сервиса, использующего API других сервисов.

## Ветки

- ветка `main` содержит тестируемую систему и тесты
- ветка `baseline` содержит тестируемую систему без тестов

Вы можете переключиться на ветку `baseline` и выполнять все шаги, описанные в статье.

## Как собрать сервис

```bash
dotnet build
dotnet test
```

## Как проверить сервис вручную

> Едва ли это нужно — сервис сделан как пример для автоматизированных тестов, нет причин запускать его вручную.

Сначала надо настроить отправку почты: создайте файл `appsettings.Development.json` с соответствующими настройками.

Например, отправить через личный ящик на Yandex Mail можно так (см.
также [документацию по настройке SMTP для Yandex Mail](https://yandex.ru/support/yandex-360/customers/mail/ru/mail-clients/others#smtpsetting)):

```json
{
  "Mailing": {
    "Service": {
      "NoReplyName": "<имя отправителя>",
      "NoReplyEmail": "<ваш логин>@yandex.ru"
    },
    "Smtp": {
      "Host": "smtp.yandex.ru",
      "Port": 587,
      "Username": "<ваш логин>@yandex.ru",
      "Password": "<пароль приложения>"
    }
  }
}
```

Запустите сервисы:

```bash
# В отдельном терминале запустите "сервис другой команды" - MailSubscription.WebService
pushd src/MailSubscription.WebService/
node index.js

# Запустите тестируемую систему
dotnet run --project src/DailyRates.WebService
```

Затем откройте файл `src/DailyRates.WebService/CurrencyRates.http` и выполните запросы

- в VS Code используйте расширение [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- в IDE от JetBrains можно исполнять запросы напрямую
