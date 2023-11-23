using Newtonsoft.Json;
using ConvertJsonToWord.Helper;
using ConvertJsonToWord.Model;
using ConvertJsonToWord.Utils;

namespace ConvertJsonToWord
{
    public partial class json2WordForm : Form
    {
        private readonly SpireDocHelper _spireDocHelper;
        private readonly RemoveWords _removeWords;
        public json2WordForm(SpireDocHelper spireDocHelper, RemoveWords removeWords)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this._spireDocHelper = spireDocHelper;
            this._removeWords = removeWords;
        }

        // 此按鈕為選擇檔案按鈕
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            // 清空暫存文字
            localLabel.Text = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "請選擇檔案";
            dialog.Filter = "所有檔案(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dialog.FileName;

                filePathTextBox.Text = filename;
            }
        }

        // 此按鈕為設定目標資料夾按鈕
        private void targetFolderButton_Click(object sender, EventArgs e)
        {
            // 清空暫存文字
            localLabel.Text = string.Empty;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "請選擇資料夾";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = dialog.SelectedPath;

                targetFolderTextBox.Text = filepath;

            }
        }

        private void filePathTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void localLabel_Click(object sender, EventArgs e)
        {

        }

        // 此按鈕會啟動json2word功能
        private void localButton_Click(object sender, EventArgs e)
        {
            // 清空暫存文字
            localLabel.Text = string.Empty;

            // 1. 根據指定資料夾獲得資料夾路徑。
            string desPath = targetFolderTextBox.Text;
            if (!Directory.Exists(desPath))
            {
                MessageBox.Show("請提供有效的目的地資料夾路徑", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2.根據指定路徑獲取指定json，此fileName包含完整路徑。
            string fileName = filePathTextBox.Text;

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName) && Path.GetExtension(fileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // fileName送入主要流程產生Word
                    j2wMainFunction(fileName, desPath);
                    localLabel.Text = "狀態: Word產生成功!";
                }
                catch (Exception ex)
                {
                    localLabel.Text = $"狀態: Word產生失敗!Error:{ex}";
                }
            }
            else if (!File.Exists(fileName) && !string.IsNullOrEmpty(fileName) && Directory.Exists(fileName))
            {
                try
                {
                    int count = 0;
                    foreach (string fname in System.IO.Directory.GetFiles(fileName))
                    {
                        if (Path.GetExtension(fname).Equals(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            // fname送入主要流程產生Word
                            j2wMainFunction(fname, desPath);
                            count++;
                        }

                    }
                    if (count > 0)
                    {
                        localLabel.Text = $"狀態: Word產生完成!成功:{count}";
                    }
                    else
                    {
                        localLabel.Text = "狀態: 資料夾底下沒有Json檔!";
                    }

                }
                catch (Exception ex)
                {
                    localLabel.Text = $"狀態: Word產生失敗!Error:{ex}";
                }
            }
            else
            {
                MessageBox.Show("請提供有效的來源檔案或來源資料夾路徑", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private void json2WordForm_Load(object sender, EventArgs e)
        {

        }

        private void targetFolderTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        // json來源是資料夾
        private void DirectoryButton_Click(object sender, EventArgs e)
        {
            // 清空暫存文字
            localLabel.Text = string.Empty;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "請選擇資料夾";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = dialog.SelectedPath;

                filePathTextBox.Text = filepath;

            }

        }

        private void j2wMainFunction(string fileName, string desPath)
        {
            string contenttype = string.Empty;
            string type = ".docx";

            // 這個路徑指向/bin/Debug/net6.0-windows
            var projectDirectory = Directory.GetCurrentDirectory();
            // targetName為文件檔名，重新組合成制式檔名。
            string targetName = Path.GetFileName(fileName);
            DateTime currentDate = DateTime.Today;
            targetName = currentDate.Year + "_" + currentDate.Month + "_" + currentDate.Day + "_API文件_" + targetName.Replace(".json", "");

            // 3. 讀取指定路徑json。
            string jsonFile = System.IO.File.ReadAllText(fileName);
            ProcessObject processObject = new ProcessObject();
            var refExtendedString = processObject.ExpandRefs(jsonFile);

            // 4. json資料Deserialize成自定義物件

            var model = JsonConvert.DeserializeObject<MyOpenApiObject>(refExtendedString);
            processObject.ProcessMethod(model);
            processObject.ProcessFormat(model);
            
            // 5. 自定義物件轉換成OpenApiDocument
            var html = "";
            // 6. 抓取模板並寫入資料

            html = HtmlHelper.GeneratorSwaggerHtml($"{projectDirectory}\\Template\\SwaggerDoc.cshtml", model);

            // 7. 資料存成Word文件
            this._spireDocHelper.SwaggerConversHtml(html, type, targetName, desPath, out contenttype);

            // 移除Spire.Doc附加的文字
            //new RemoveWords(Path.Combine(desPath, targetName + type));
            this._removeWords.Remove(Path.Combine(desPath, targetName + type));

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }
}