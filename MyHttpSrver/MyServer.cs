using System;
using System.Net;
using System.Text;


namespace MyHttpSrver
{
	public class MyServer
	{
		private bool exit = false;
        private AppSettings config;
        private HttpListener server = new HttpListener();
        

        public MyServer(AppSettings config, HttpListener server)
		{
			this.config = config;
            this.server = server;
        }

        public void Start()
        {
            while (!exit)
            {
                try
                {
                    var context = server.GetContext();
                    var request = context.Request;
                    var response = context.Response;

                    string requestUrl = request.Url.LocalPath;

                    if (requestUrl.StartsWith("/static/") && requestUrl.EndsWith(".html"))
                    {
                        string filePath = Path.Combine(config.StaticFilesPath, requestUrl.Substring(8));
                        if (File.Exists(filePath))
                        {
                            byte[] buffer = File.ReadAllBytes(filePath);
                            response.ContentLength64 = buffer.Length;
                            string contentType = "text/html; charset=utf-8";
                            response.ContentType = contentType;
                            using Stream output = response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Flush();
                            Console.WriteLine($"Запрос обработан: {requestUrl}");
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            response.ContentType = "text/plain; charset=utf-8";
                            string notFoundMessage = "404 Файл не найден";
                            byte[] notFoundBuffer = Encoding.UTF8.GetBytes(notFoundMessage);
                            response.ContentLength64 = notFoundBuffer.Length;
                            using Stream output = response.OutputStream;
                            output.Write(notFoundBuffer, 0, notFoundBuffer.Length);
                            output.Flush();
                            Console.WriteLine($"Файл не найден: {requestUrl}");
                        }
                    }
                    else
                    {
                        string filePath = Path.Combine(config.StaticFilesPath, "/Users/ruslan/Projects/MyHttpSrver/MyHttpSrver/index.html");
                        if (File.Exists(filePath))
                        {
                            byte[] buffer = File.ReadAllBytes(filePath);
                            response.ContentLength64 = buffer.Length;
                            response.ContentType = "text/html; charset=utf-8";
                            using Stream output = response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Flush();
                            Console.WriteLine($"Запрос обработан: {requestUrl}");
                        }
                    }
                }
                catch (HttpListenerException ex) when (ex.ErrorCode == 995)
                {
                    break;
                }
            }
        }

        public void Stop()
        { 
            server.Close();
            exit = true;
            Console.WriteLine("Сервер остановлен");
        }
    }
}

