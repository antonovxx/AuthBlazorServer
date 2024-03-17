namespace AuthTutorial.Http
{
    public class HttpClientOptions
    {
        public string Url { get; set; }

        public int TimeOutMilliseconds { get; set; } = 30000;
    }
}
