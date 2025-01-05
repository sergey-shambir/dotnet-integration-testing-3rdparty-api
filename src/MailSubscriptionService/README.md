# MailSubscriptionService

Это маленький сервис на Node.js (Express)

- он имитирует работу смежного микросервиса, находящегося за пределами тестируемой системы
- он написан на JavaScript (Node.js) вместо C# и ASP Net Core, чтобы гарантировать отсутствие влияния на тестируемую
  систему

## Минимальная проверка работоспособности

```bash
# Запуск
node index.js

# Добавление подписки
curl -v -X POST \
  -H "Content-Type: application/json" \
  -d '{"name":"Фёдор Достоевский", "email":"fedor@example.com", "customData": {"currencyCodes": ["AUD", "BRL"]}}' \
   http://localhost:5025/mail-subscription/

curl -v -X POST \
  -H "Content-Type: application/json" \
  -d '{"name":"Александр Раскин", "email":"a.ruskin@company.local", "customData": {"currencyCodes": ["AED", "BRL", "INR"]}}' \
   http://localhost:5025/mail-subscription/

# Получение подписок
curl -v http://localhost:5025/mail-subscription/
```
