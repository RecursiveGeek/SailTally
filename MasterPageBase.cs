using SailTally.Classes;

namespace SailTally
{
    public class MasterPageBase : System.Web.UI.MasterPage
    {
        public void RegisterScriptFile(string pathAndFilename, string scriptLanguage = "javascript")
        {
            if (Page.ClientScript.IsClientScriptBlockRegistered(pathAndFilename)) return;
            var includeScript = string.Format(Constant.Scripts.IncludeScriptFormat, scriptLanguage, pathAndFilename, string.Empty);
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), pathAndFilename, includeScript, false);
        }

        public void RegisterJavascriptBlock(string blockName, string codeBlock)
        {
            if (Page.ClientScript.IsClientScriptBlockRegistered(blockName)) return;
            var includeBlock = string.Format(Constant.Scripts.BlockScriptFormat, "javascript", codeBlock);
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), blockName, includeBlock, false);
        }
    }
}
