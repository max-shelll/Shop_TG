using Pastel;
using Shop_TG.DAL.Configs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Shop_TG.BLL.Telegram.Services
{
    public class LoggerService
    {
        public static void LogErrorAsync(Exception exception, long? botId)
        {
            string currentTime = DateTime.Now.ToString("dd.MM.yy | HH:mm");

            Console.WriteLine(
                $"[{currentTime.Pastel(Color.DarkOrange)}] | " +
                $"[{"API Error".Pastel(Color.Red)}] > " +
                $"\n{exception.Message.Pastel(Color.GhostWhite)}");
        }

        public static void LogCommonAsync(string msg, Enum? typeEvent, ConsoleColor color)
        {
            string currentTime = DateTime.Now.ToString("dd.MM.yy | HH:mm");

            Console.WriteLine(
                $"[{currentTime.Pastel(Color.DarkOrange)}] | " +
                   $"[{"Info".Pastel(Color.Cyan)} | {(typeEvent == null ? "Message".Pastel(Color.GreenYellow) : typeEvent.ToString().Pastel(color))}] > " +
                $"{msg.Pastel(Color.GhostWhite)}");
        }
    }
}
