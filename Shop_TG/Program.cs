using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using Shop_TG.BLL.Telegram.Handlers;
using Shop_TG.BLL.Telegram.Services;
using Shop_TG.DAL.Configs;
using Shop_TG.DAL.Models;
using Shop_TG.DAL.Repositories;

namespace Shop_TG
{
    public class Program
    {
        private readonly IServiceProvider _service;
        private readonly PRBot _client;

        private readonly Config _config;

        public Program()
        {
            string json = File.ReadAllText("config.json");
            _config = JsonConvert.DeserializeObject<Config>(json) ?? throw new InvalidOperationException("Failed to load configuration.");

            _service = new ServiceCollection()
                 .AddBotHandlers()
                 .AddSingleton(_config)
                 /// Handlers
                 .AddSingleton<MissingCommandHandler>()
                 .AddSingleton<CheckUserPrivilegeHandler>()
                 /// Repositories
                 .AddSingleton<ShopCategoryRepository>()
                 .AddSingleton<ShopItemRepository>()
                 .AddSingleton<PaymentRepository>()
                 /// Client
                 .AddSingleton(provider => new PRBot(options =>
                 {
                     options.Token = _config.BotToken;
                     options.ClearUpdatesOnStart = true;
                     options.WhiteListUsers = new();
                     options.Admins = _config.Staff.AdminIds;
                     options.BotId = 0;
                 }, provider))
                 .BuildServiceProvider();

            _client = _service.GetService<PRBot>();
        }

        static void Main(string[] args)
           => new Program().RunAsync().GetAwaiter().GetResult();

        private async Task RunAsync()
        {
            _client.OnLogError += LoggerService.LogErrorAsync;
            _client.OnLogCommon += LoggerService.LogCommonAsync;

            await HandlerInts();

            await _client.Start();

            await Task.Delay(Timeout.Infinite);
        }
        private async Task HandlerInts()
        {
            if (_client == null)
                return;

            _client.Handler.Router.OnMissingCommand += _service.GetService<MissingCommandHandler>().Execute;
            _client.Handler.Router.OnCheckPrivilege += _service.GetService<CheckUserPrivilegeHandler>().Execute;
        }
    }
}
