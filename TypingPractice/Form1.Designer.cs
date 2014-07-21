namespace TypingPractice
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.keyboardPicture = new System.Windows.Forms.PictureBox();
            this.layoutPicker = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statisticsLabel = new System.Windows.Forms.Label();
            this.timeRemainingLabel = new System.Windows.Forms.Label();
            this.wordsTypedLabel = new System.Windows.Forms.Label();
            this.wpmLabel = new System.Windows.Forms.Label();
            this.errorsLabel = new System.Windows.Forms.Label();
            this.timeRemaining = new System.Windows.Forms.Label();
            this.wordsTyped = new System.Windows.Forms.Label();
            this.wpm = new System.Windows.Forms.Label();
            this.errors = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.inputText = new System.Windows.Forms.RichTextBox();
            this.bestQwertyLabel = new System.Windows.Forms.Label();
            this.bestQwertyWPM = new System.Windows.Forms.Label();
            this.bestDvorakLabel = new System.Windows.Forms.Label();
            this.bestDvorakWPM = new System.Windows.Forms.Label();
            this.timePicker = new System.Windows.Forms.NumericUpDown();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.displayedText = new System.Windows.Forms.RichTextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonWordList = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.keyboardPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timePicker)).BeginInit();
            this.SuspendLayout();
            // 
            // keyboardPicture
            // 
            this.keyboardPicture.Location = new System.Drawing.Point(16, 13);
            this.keyboardPicture.Name = "keyboardPicture";
            this.keyboardPicture.Size = new System.Drawing.Size(519, 127);
            this.keyboardPicture.TabIndex = 2;
            this.keyboardPicture.TabStop = false;
            // 
            // layoutPicker
            // 
            this.layoutPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.layoutPicker.FormattingEnabled = true;
            this.layoutPicker.Location = new System.Drawing.Point(16, 160);
            this.layoutPicker.Name = "layoutPicker";
            this.layoutPicker.Size = new System.Drawing.Size(121, 21);
            this.layoutPicker.TabIndex = 0;
            this.layoutPicker.SelectedIndexChanged += new System.EventHandler(this.layoutPicker_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statisticsLabel
            // 
            this.statisticsLabel.AutoSize = true;
            this.statisticsLabel.Location = new System.Drawing.Point(638, 13);
            this.statisticsLabel.Name = "statisticsLabel";
            this.statisticsLabel.Size = new System.Drawing.Size(49, 13);
            this.statisticsLabel.TabIndex = 3;
            this.statisticsLabel.Text = "Statistics";
            // 
            // timeRemainingLabel
            // 
            this.timeRemainingLabel.AutoSize = true;
            this.timeRemainingLabel.Location = new System.Drawing.Point(555, 39);
            this.timeRemainingLabel.Name = "timeRemainingLabel";
            this.timeRemainingLabel.Size = new System.Drawing.Size(73, 13);
            this.timeRemainingLabel.TabIndex = 4;
            this.timeRemainingLabel.Text = "Seconds Left:";
            // 
            // wordsTypedLabel
            // 
            this.wordsTypedLabel.AutoSize = true;
            this.wordsTypedLabel.Location = new System.Drawing.Point(555, 63);
            this.wordsTypedLabel.Name = "wordsTypedLabel";
            this.wordsTypedLabel.Size = new System.Drawing.Size(74, 13);
            this.wordsTypedLabel.TabIndex = 5;
            this.wordsTypedLabel.Text = "Words Typed:";
            // 
            // wpmLabel
            // 
            this.wpmLabel.AutoSize = true;
            this.wpmLabel.Location = new System.Drawing.Point(555, 90);
            this.wpmLabel.Name = "wpmLabel";
            this.wpmLabel.Size = new System.Drawing.Size(37, 13);
            this.wpmLabel.TabIndex = 6;
            this.wpmLabel.Text = "WPM:";
            // 
            // errorsLabel
            // 
            this.errorsLabel.AutoSize = true;
            this.errorsLabel.Location = new System.Drawing.Point(555, 113);
            this.errorsLabel.Name = "errorsLabel";
            this.errorsLabel.Size = new System.Drawing.Size(37, 13);
            this.errorsLabel.TabIndex = 7;
            this.errorsLabel.Text = "Errors:";
            // 
            // timeRemaining
            // 
            this.timeRemaining.AutoSize = true;
            this.timeRemaining.Location = new System.Drawing.Point(674, 39);
            this.timeRemaining.Name = "timeRemaining";
            this.timeRemaining.Size = new System.Drawing.Size(13, 13);
            this.timeRemaining.TabIndex = 8;
            this.timeRemaining.Text = "0";
            // 
            // wordsTyped
            // 
            this.wordsTyped.AutoSize = true;
            this.wordsTyped.Location = new System.Drawing.Point(674, 63);
            this.wordsTyped.Name = "wordsTyped";
            this.wordsTyped.Size = new System.Drawing.Size(13, 13);
            this.wordsTyped.TabIndex = 9;
            this.wordsTyped.Text = "0";
            // 
            // wpm
            // 
            this.wpm.AutoSize = true;
            this.wpm.Location = new System.Drawing.Point(674, 90);
            this.wpm.Name = "wpm";
            this.wpm.Size = new System.Drawing.Size(13, 13);
            this.wpm.TabIndex = 10;
            this.wpm.Text = "0";
            // 
            // errors
            // 
            this.errors.AutoSize = true;
            this.errors.Location = new System.Drawing.Point(674, 113);
            this.errors.Name = "errors";
            this.errors.Size = new System.Drawing.Size(13, 13);
            this.errors.TabIndex = 11;
            this.errors.Text = "0";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(337, 161);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 12;
            this.buttonStart.TabStop = false;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // inputText
            // 
            this.inputText.AcceptsTab = true;
            this.inputText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputText.DetectUrls = false;
            this.inputText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputText.Location = new System.Drawing.Point(16, 276);
            this.inputText.Name = "inputText";
            this.inputText.ShortcutsEnabled = false;
            this.inputText.Size = new System.Drawing.Size(727, 57);
            this.inputText.TabIndex = 2;
            this.inputText.Text = "";
            this.inputText.SelectionChanged += new System.EventHandler(this.inputText_CursorChanged);
            this.inputText.TextChanged += new System.EventHandler(this.inputText_TextChanged);
            // 
            // bestQwertyLabel
            // 
            this.bestQwertyLabel.AutoSize = true;
            this.bestQwertyLabel.Location = new System.Drawing.Point(555, 159);
            this.bestQwertyLabel.Name = "bestQwertyLabel";
            this.bestQwertyLabel.Size = new System.Drawing.Size(112, 13);
            this.bestQwertyLabel.TabIndex = 14;
            this.bestQwertyLabel.Text = "Best QWERTY WPM:";
            // 
            // bestQwertyWPM
            // 
            this.bestQwertyWPM.AutoSize = true;
            this.bestQwertyWPM.Location = new System.Drawing.Point(674, 159);
            this.bestQwertyWPM.Name = "bestQwertyWPM";
            this.bestQwertyWPM.Size = new System.Drawing.Size(13, 13);
            this.bestQwertyWPM.TabIndex = 15;
            this.bestQwertyWPM.Text = "0";
            // 
            // bestDvorakLabel
            // 
            this.bestDvorakLabel.AutoSize = true;
            this.bestDvorakLabel.Location = new System.Drawing.Point(555, 189);
            this.bestDvorakLabel.Name = "bestDvorakLabel";
            this.bestDvorakLabel.Size = new System.Drawing.Size(109, 13);
            this.bestDvorakLabel.TabIndex = 16;
            this.bestDvorakLabel.Text = "Best DVORAK WPM:";
            // 
            // bestDvorakWPM
            // 
            this.bestDvorakWPM.AutoSize = true;
            this.bestDvorakWPM.Location = new System.Drawing.Point(674, 189);
            this.bestDvorakWPM.Name = "bestDvorakWPM";
            this.bestDvorakWPM.Size = new System.Drawing.Size(13, 13);
            this.bestDvorakWPM.TabIndex = 17;
            this.bestDvorakWPM.Text = "0";
            // 
            // timePicker
            // 
            this.timePicker.Location = new System.Drawing.Point(253, 161);
            this.timePicker.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.timePicker.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(64, 20);
            this.timePicker.TabIndex = 1;
            this.timePicker.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // minutesLabel
            // 
            this.minutesLabel.AutoSize = true;
            this.minutesLabel.Location = new System.Drawing.Point(200, 166);
            this.minutesLabel.Name = "minutesLabel";
            this.minutesLabel.Size = new System.Drawing.Size(47, 13);
            this.minutesLabel.TabIndex = 18;
            this.minutesLabel.Text = "Minutes:";
            // 
            // debugTextBox
            // 
            this.debugTextBox.Location = new System.Drawing.Point(248, 202);
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.Size = new System.Drawing.Size(257, 20);
            this.debugTextBox.TabIndex = 19;
            // 
            // displayedText
            // 
            this.displayedText.Cursor = System.Windows.Forms.Cursors.No;
            this.displayedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayedText.Location = new System.Drawing.Point(18, 248);
            this.displayedText.Name = "displayedText";
            this.displayedText.ReadOnly = true;
            this.displayedText.Size = new System.Drawing.Size(727, 22);
            this.displayedText.TabIndex = 20;
            this.displayedText.TabStop = false;
            this.displayedText.Text = "";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(430, 161);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 21;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonWordList
            // 
            this.buttonWordList.Location = new System.Drawing.Point(16, 189);
            this.buttonWordList.Name = "buttonWordList";
            this.buttonWordList.Size = new System.Drawing.Size(75, 23);
            this.buttonWordList.TabIndex = 22;
            this.buttonWordList.Text = "Word List...";
            this.buttonWordList.UseVisualStyleBackColor = true;
            this.buttonWordList.Click += new System.EventHandler(this.buttonWordList_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text Files|*.txt";
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "Word List";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 345);
            this.Controls.Add(this.buttonWordList);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.displayedText);
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.minutesLabel);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.bestDvorakWPM);
            this.Controls.Add(this.bestDvorakLabel);
            this.Controls.Add(this.bestQwertyWPM);
            this.Controls.Add(this.bestQwertyLabel);
            this.Controls.Add(this.inputText);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.errors);
            this.Controls.Add(this.wpm);
            this.Controls.Add(this.wordsTyped);
            this.Controls.Add(this.timeRemaining);
            this.Controls.Add(this.errorsLabel);
            this.Controls.Add(this.wpmLabel);
            this.Controls.Add(this.wordsTypedLabel);
            this.Controls.Add(this.timeRemainingLabel);
            this.Controls.Add(this.statisticsLabel);
            this.Controls.Add(this.layoutPicker);
            this.Controls.Add(this.keyboardPicture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Typing Practice";
            ((System.ComponentModel.ISupportInitialize)(this.keyboardPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timePicker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox keyboardPicture;
		private System.Windows.Forms.ComboBox layoutPicker;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label statisticsLabel;
		private System.Windows.Forms.Label timeRemainingLabel;
		private System.Windows.Forms.Label wordsTypedLabel;
		private System.Windows.Forms.Label wpmLabel;
		private System.Windows.Forms.Label errorsLabel;
		private System.Windows.Forms.Label timeRemaining;
		private System.Windows.Forms.Label wordsTyped;
		private System.Windows.Forms.Label wpm;
		private System.Windows.Forms.Label errors;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.RichTextBox inputText;
		private System.Windows.Forms.Label bestQwertyLabel;
		private System.Windows.Forms.Label bestQwertyWPM;
		private System.Windows.Forms.Label bestDvorakLabel;
		private System.Windows.Forms.Label bestDvorakWPM;
		private System.Windows.Forms.NumericUpDown timePicker;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.TextBox debugTextBox;
		private System.Windows.Forms.RichTextBox displayedText;
		private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonWordList;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}

