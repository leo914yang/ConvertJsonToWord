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

        // �����s������ɮ׫��s
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            // �M�żȦs��r
            localLabel.Text = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "�п���ɮ�";
            dialog.Filter = "�Ҧ��ɮ�(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dialog.FileName;

                filePathTextBox.Text = filename;
            }
        }

        // �����s���]�w�ؼи�Ƨ����s
        private void targetFolderButton_Click(object sender, EventArgs e)
        {
            // �M�żȦs��r
            localLabel.Text = string.Empty;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "�п�ܸ�Ƨ�";
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

        // �����s�|�Ұ�json2word�\��
        private void localButton_Click(object sender, EventArgs e)
        {
            // �M�żȦs��r
            localLabel.Text = string.Empty;

            // 1. �ھګ��w��Ƨ���o��Ƨ����|�C
            string desPath = targetFolderTextBox.Text;
            if (!Directory.Exists(desPath))
            {
                MessageBox.Show("�д��Ѧ��Ī��ت��a��Ƨ����|", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2.�ھګ��w���|������wjson�A��fileName�]�t������|�C
            string fileName = filePathTextBox.Text;

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName) && Path.GetExtension(fileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // fileName�e�J�D�n�y�{����Word
                    j2wMainFunction(fileName, desPath);
                    localLabel.Text = "���A: Word���ͦ��\!";
                }
                catch (Exception ex)
                {
                    localLabel.Text = $"���A: Word���ͥ���!Error:{ex}";
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
                            // fname�e�J�D�n�y�{����Word
                            j2wMainFunction(fname, desPath);
                            count++;
                        }

                    }
                    if (count > 0)
                    {
                        localLabel.Text = $"���A: Word���ͧ���!���\:{count}";
                    }
                    else
                    {
                        localLabel.Text = "���A: ��Ƨ����U�S��Json��!";
                    }

                }
                catch (Exception ex)
                {
                    localLabel.Text = $"���A: Word���ͥ���!Error:{ex}";
                }
            }
            else
            {
                MessageBox.Show("�д��Ѧ��Ī��ӷ��ɮשΨӷ���Ƨ����|", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private void json2WordForm_Load(object sender, EventArgs e)
        {

        }

        private void targetFolderTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        // json�ӷ��O��Ƨ�
        private void DirectoryButton_Click(object sender, EventArgs e)
        {
            // �M�żȦs��r
            localLabel.Text = string.Empty;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "�п�ܸ�Ƨ�";
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

            // �o�Ӹ��|���V/bin/Debug/net6.0-windows
            var projectDirectory = Directory.GetCurrentDirectory();
            // targetName������ɦW�A���s�զX����ɦW�C
            string targetName = Path.GetFileName(fileName);
            DateTime currentDate = DateTime.Today;
            targetName = currentDate.Year + "_" + currentDate.Month + "_" + currentDate.Day + "_API���_" + targetName.Replace(".json", "");

            // 3. Ū�����w���|json�C
            string jsonFile = System.IO.File.ReadAllText(fileName);
            ProcessObject processObject = new ProcessObject();
            var refExtendedString = processObject.ExpandRefs(jsonFile);

            // 4. json���Deserialize���۩w�q����

            var model = JsonConvert.DeserializeObject<MyOpenApiObject>(refExtendedString);
            processObject.ProcessMethod(model);
            processObject.ProcessFormat(model);
            
            // 5. �۩w�q�����ഫ��OpenApiDocument
            var html = "";
            // 6. ����ҪO�üg�J���

            html = HtmlHelper.GeneratorSwaggerHtml($"{projectDirectory}\\Template\\SwaggerDoc.cshtml", model);

            // 7. ��Ʀs��Word���
            this._spireDocHelper.SwaggerConversHtml(html, type, targetName, desPath, out contenttype);

            // ����Spire.Doc���[����r
            //new RemoveWords(Path.Combine(desPath, targetName + type));
            this._removeWords.Remove(Path.Combine(desPath, targetName + type));

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }
}