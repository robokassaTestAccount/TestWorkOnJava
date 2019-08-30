using OpenPop.Pop3;
using LoggerHelperSpace;
using OpenPop.Pop3.Exceptions;

namespace Core
{
    public class PostClient
    {
        private static PostClient instance;
        private LoggerHelper Logger { get; set; }

        private string Login { get; set; } = "futuracomesivan@gmail.com";
        private string Pass { get; set; } = "654321ytrewQ";

        private PostClient(LoggerHelper Logger)
        {
            this.Logger = Logger;
        }

        public static PostClient GetInstance(LoggerHelper logger)
        {
            if (instance == null)
            {
                instance = new PostClient(logger);
            }
            return instance;
        }        
        public void Auth()
        {
            using (Pop3Client client = new Pop3Client())
            {
                try
                {
                    client.Connect("pop.gmail.com", 995, true);
                    client.Authenticate(Login, Pass, AuthenticationMethod.UsernameAndPassword);
                    Logger.WriteInfo("Удалось авторизоваться");
                }
                catch(InvalidLoginException e)
                {
                    Logger.WriteError("Авторизоваться не удалось");
                }
            }
        }
    }
}
