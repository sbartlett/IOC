namespace IOC.Web.QuoteProvider
{
    public interface IQuoteProvider
    {
        string GetQuote();
    }

    public class QuoteProvider : IQuoteProvider
    {
        public string GetQuote()
        {
            return "I am prepared for the worst, but hope for the best - Benjamin Disraeli";
        }
    }
}