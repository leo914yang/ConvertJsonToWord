using Microsoft.AspNetCore.StaticFiles;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Diagnostics;
using System.Text;

namespace ConvertJsonToWord.Helper
{
    /// <summary>
    /// SpireDocHelper
    /// </summary>
    public class SpireDocHelper
    {
        public void SwaggerConversHtml(string html, string type, string fName, string desPath, out string contenttype)
        {
            // 產生唯一編碼，避免產生檔案時檔名重複
            var fileName = Guid.NewGuid().ToString() + type;

            // 指定word暫存路徑。
            // 原先使用WebAPI，暫存路徑是wwwroot，
            // 後續WebAPI拔掉，路徑在bin\Debug\net6.0-windows底下
            string webRootPath = Directory.GetCurrentDirectory();
            string path = Path.Combine(webRootPath, "TempFiles");
            var addrUrl = path + $"{fileName}";

            FileStream? fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            contenttype = provider.Mappings[type];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var data = Encoding.Default.GetBytes(html);
                var stream = ByteHelper.BytesToStream(data);
                Document document = new Document();
                // 加載html文件
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);

                document.SaveToFile(addrUrl, FileFormat.Docx);
                
                document.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                fileStream?.Close();

                if (File.Exists(addrUrl))
                {
                    string finalPath = Path.Combine(desPath, fName + type);

                    if (!File.Exists(finalPath))
                    {
                        File.Move(addrUrl, finalPath);
                    }
                    else
                    {
                        File.Delete(finalPath);
                        File.Move(addrUrl, finalPath);
                    }
                }
            }
        }

        

    }

    
}
