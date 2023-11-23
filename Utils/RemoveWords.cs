using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ConvertJsonToWord.Utils
{
    public class RemoveWords
    {
        public void Remove(string filePath) 
        {
            // 將紅字移除 紅字出現在文件最開頭的地方 所以打開文件移除第一行
            using WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true);
            var paragraphs = doc.MainDocumentPart.Document.Body.Elements<Paragraph>();

            foreach (var paragraph in paragraphs)
            {
                var text = paragraph.InnerText;
                if (text.Contains("Evaluation Warning"))
                {
                    paragraph.Remove();
                }
            }
            doc.Save();
        }
    }
}
