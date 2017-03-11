using HtmlAgilityPack;
using System.Linq;

namespace CommonUtils.Html
{
    public class ValidateHtml
    {
        public static bool IsValidHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.ParseErrors.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public static bool IsValidHtmlFile(string path)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(path);
            if (doc.ParseErrors.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
