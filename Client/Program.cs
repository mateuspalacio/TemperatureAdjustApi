HttpClient http = new HttpClient();
http.BaseAddress = new Uri("https://localhost:7023");
while (true)
{
    Task.Delay(10000).Wait();
    http.GetAsync("/temperature");
    Task.Delay(50000).Wait();
}
