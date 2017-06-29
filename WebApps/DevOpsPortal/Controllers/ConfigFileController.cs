//using ApiLib;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModel;

namespace DevOpsPortal.Controllers
{
    public class ConfigFileController : BaseController
    {
        //public async Task<FileResult> GetConfigFile(int componentId, string environment)
        //{
        //    string parameters = "requestType=configFile&componentId=" + componentId.ToString() + "&environment=" + environment;
        //    FileHelper fh = await ClientApi<ConfigXml>.GetFileAsync(ClientApi<ConfigXml>.ArgType.TypeParamsFromURI, parameters);
        //    UTF8Encoding encoder = new UTF8Encoding();
        //    return File(fh.FileBytes, fh.ContentType, fh.FileName);
        //}

        //public async Task<FileResult> GetConfigFile(string componentName, string environment)
        //{
        //    string parameters = "requestType=configFile&componentName=" + componentName + "&environment=" + environment;
        //    FileHelper fh = await ClientApi<ConfigXml>.GetFileAsync(ClientApi<ConfigXml>.ArgType.TypeParamsFromURI, parameters);
        //    UTF8Encoding encoder = new UTF8Encoding();
        //    return File(fh.FileBytes, fh.ContentType, fh.FileName);
        //}
    }
}
