using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiAutotests
{
    [TestClass]
    public class PostsAndFunction : BasicActions
    {       
        [TestMethod]
        public void SeasonCheck()
        {
            for(int i = -2; i<20; i++)
            {
                Logger.WriteInfo(GetSeason(i));
            }            
        }      
        
        [TestMethod]
        public void PostCheck()
        {
            PostClient.Auth();
        }
    }
}
