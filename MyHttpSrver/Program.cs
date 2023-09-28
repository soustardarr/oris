using MyHttpSrver;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        AppSettings appSet = ConfigApp.SettingTheConfig();
        if (appSet == null)
        {
            Console.WriteLine("ошибка при создании директории, возможно не найден файл");
            return;
        }
        

        string prefix = $"{appSet.Address}:{appSet.Port}/";
        HttpListener server = new HttpListener();
        server.Prefixes.Add(prefix);
        server.Start();
        Console.WriteLine($"Сервер запущен на {prefix} чтобы очстановить введите 'stop' в консоль и нажмите Enter.");
        var myServer = new MyServer(appSet, server);
        Task.Run(() =>
        {
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (consoleInput == "stop")
                {
                    myServer.Stop();
                    break;
                }
            }
        });
        myServer.Start();

    }
}
