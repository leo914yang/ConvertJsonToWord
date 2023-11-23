namespace ConvertJsonToWord
{
    partial class json2WordForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            selectFileButton = new Button();
            targetFolderButton = new Button();
            filePathTextBox = new TextBox();
            targetFolderTextBox = new TextBox();
            localButton = new Button();
            localLabel = new Label();
            DirectoryButton = new Button();
            introdoctionLabelStep1 = new Label();
            introdoctionLabelStep2 = new Label();
            introdoctionLabelStep3 = new Label();
            SuspendLayout();
            // 
            // selectFileButton
            // 
            selectFileButton.Location = new Point(17, 17);
            selectFileButton.Name = "selectFileButton";
            selectFileButton.Size = new Size(56, 34);
            selectFileButton.TabIndex = 0;
            selectFileButton.Text = "檔案";
            selectFileButton.UseVisualStyleBackColor = true;
            selectFileButton.Click += selectFileButton_Click;
            // 
            // targetFolderButton
            // 
            targetFolderButton.Location = new Point(17, 71);
            targetFolderButton.Name = "targetFolderButton";
            targetFolderButton.Size = new Size(112, 34);
            targetFolderButton.TabIndex = 4;
            targetFolderButton.Text = "目標資料夾";
            targetFolderButton.UseVisualStyleBackColor = true;
            targetFolderButton.Click += targetFolderButton_Click;
            // 
            // filePathTextBox
            // 
            filePathTextBox.Location = new Point(135, 20);
            filePathTextBox.Name = "filePathTextBox";
            filePathTextBox.Size = new Size(580, 30);
            filePathTextBox.TabIndex = 5;
            filePathTextBox.TextChanged += filePathTextBox_TextChanged;
            // 
            // targetFolderTextBox
            // 
            targetFolderTextBox.Location = new Point(135, 74);
            targetFolderTextBox.Name = "targetFolderTextBox";
            targetFolderTextBox.Size = new Size(580, 30);
            targetFolderTextBox.TabIndex = 6;
            targetFolderTextBox.TextChanged += targetFolderTextBox_TextChanged;
            // 
            // localButton
            // 
            localButton.Location = new Point(17, 235);
            localButton.Name = "localButton";
            localButton.Size = new Size(112, 34);
            localButton.TabIndex = 7;
            localButton.Text = "啟動";
            localButton.UseVisualStyleBackColor = true;
            localButton.Click += localButton_Click;
            // 
            // localLabel
            // 
            localLabel.AutoSize = true;
            localLabel.Location = new Point(135, 241);
            localLabel.Name = "localLabel";
            localLabel.Size = new Size(0, 23);
            localLabel.TabIndex = 8;
            localLabel.Click += localLabel_Click;
            // 
            // DirectoryButton
            // 
            DirectoryButton.Location = new Point(74, 17);
            DirectoryButton.Name = "DirectoryButton";
            DirectoryButton.Size = new Size(55, 34);
            DirectoryButton.TabIndex = 9;
            DirectoryButton.Text = "目錄";
            DirectoryButton.UseVisualStyleBackColor = true;
            DirectoryButton.Click += DirectoryButton_Click;
            // 
            // introdoctionLabelStep1
            // 
            introdoctionLabelStep1.AutoSize = true;
            introdoctionLabelStep1.Location = new Point(26, 123);
            introdoctionLabelStep1.Name = "introdoctionLabelStep1";
            introdoctionLabelStep1.Size = new Size(523, 23);
            introdoctionLabelStep1.TabIndex = 10;
            introdoctionLabelStep1.Text = "Step 1. 點選檔案或目錄來指定目標Json檔或目標資料夾所在位置";
            introdoctionLabelStep1.Click += label1_Click;
            // 
            // introdoctionLabelStep2
            // 
            introdoctionLabelStep2.AutoSize = true;
            introdoctionLabelStep2.Location = new Point(26, 159);
            introdoctionLabelStep2.Name = "introdoctionLabelStep2";
            introdoctionLabelStep2.Size = new Size(322, 23);
            introdoctionLabelStep2.TabIndex = 11;
            introdoctionLabelStep2.Text = "Step 2. 選擇放置Word檔的資料夾位置 ";
            // 
            // introdoctionLabelStep3
            // 
            introdoctionLabelStep3.AutoSize = true;
            introdoctionLabelStep3.Location = new Point(26, 196);
            introdoctionLabelStep3.Name = "introdoctionLabelStep3";
            introdoctionLabelStep3.Size = new Size(367, 23);
            introdoctionLabelStep3.TabIndex = 12;
            introdoctionLabelStep3.Text = "Step 3. 點啟動執行Json To Word的格式轉換";
            // 
            // json2WordForm
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(727, 281);
            Controls.Add(introdoctionLabelStep3);
            Controls.Add(introdoctionLabelStep2);
            Controls.Add(introdoctionLabelStep1);
            Controls.Add(DirectoryButton);
            Controls.Add(localLabel);
            Controls.Add(localButton);
            Controls.Add(targetFolderTextBox);
            Controls.Add(filePathTextBox);
            Controls.Add(targetFolderButton);
            Controls.Add(selectFileButton);
            Name = "json2WordForm";
            Text = "Json格式API文件轉換成Word文件";
            Load += json2WordForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button selectFileButton;
        private Button targetFolderButton;
        private TextBox filePathTextBox;
        private TextBox targetFolderTextBox;
        private Button localButton;
        private Label localLabel;
        private Button DirectoryButton;
        private Label introdoctionLabelStep1;
        private Label introdoctionLabelStep2;
        private Label introdoctionLabelStep3;
    }
}