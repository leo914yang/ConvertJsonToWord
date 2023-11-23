using ConvertJsonToWord.Model;
using RazorEngine;
using RazorEngine.Templating;

namespace ConvertJsonToWord.Helper
{
    /// <summary>
    /// HtmlHelper
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// 遍歷數據
        /// </summary>
        /// <param name="templatePath">Word檔的html模板路徑</param>
        /// <param name="model">Json資料寫入OpenApiDocument的物件</param>
        /// <returns></returns>
        public static string GeneratorSwaggerHtml(string templatePath, MyOpenApiObject model)
        {
            var template = System.IO.File.ReadAllText(templatePath);

            var result = Engine.Razor.RunCompile(template, "cshtmlTemplate", typeof(MyOpenApiObject), model);
            return result;

        }

    }
}
