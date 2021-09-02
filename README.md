# Sandbox

## Sandbox Telegram Bot

#### Bot Features:
- Commands list
- Contacts information
- Survey
    - Single-option answer
    - Manual-option answer
    - User input answer
- Catalog
    - Category
    - Items

#### TODO Features (next version):
- survey result
    - UI viewer
    - send email (https://www.mailgun.com/pricing/)
    - send to CRM 
- add validation for name, email, phone, custom answer option is required
- multi-option question

#### Useful links:
- Telegram Bot documentation
    - https://core.telegram.org/bots
    - https://core.telegram.org/bots/api
- .net sdk for Telegram Bot Api: 
    - https://github.com/TelegramBots
    - https://github.com/TelegramBots/Telegram.Bot.Examples
    - https://telegrambots.github.io/book/index.html

#### Deploy
```sh
docker build -f Dockerfile -t boikoandrei1996/sandbox ../
```
```sh
docker push boikoandrei1996/sandbox
```

#### Development
- **Sandbox.TelegramBot.Polling** - development only
- **Sandbox.TelegramBot.Webhook** - development and production
- Used:
    - .Net 5.0
    - Telegram Bot Api
    - Mongo DB
    - Redis Cache
    - Serilog
- ngrok
    - https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.WebHook#ngrok
    - https://ngrok.com/download
    - `.\ngrok.exe http 8443`
- use _docker-compose.yml_ to start local instances of Mongo and Redis
