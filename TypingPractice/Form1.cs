using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace TypingPractice
{
	public partial class Form1 : Form
	{
		private DateTime startTime;
		private double elapsedSeconds;
		private RichTextBox oldText = new RichTextBox();
		private int numMins;
		private bool running;
		private List<string> correctStrings = new List<string>();
		private List<string> typedStrings = new List<string>();
		private string currentCorrectLine;
		private MatchCollection currentCorrectWords;
		private MatchCollection currentTypedWords;
		private int totalWords;
		private int totalErrorWords;
		private Color nextScreenReadyColor = Color.LimeGreen;
		private Random random;
		private string highscorePath = "D:\\Dylan\\Projects\\C#\\TypingPractice\\TypingPractice\\Highscores\\";
		private string highscoreFile = "highscores.txt";
		private string wordListpath = "D:\\Dylan\\Projects\\C#\\TypingPractice\\TypingPractice\\WordLists\\";
		private string wordListfile = "google.txt";
		private int highscoreQWERTY = 0;
		private int highscoreDVORAK = 0;
		private bool errorColoring = false;
		private List<string> wordList = new List<string>();

		public Form1()
		{
			InitializeComponent();
			this.running = false;
			this.layoutPicker.Items.Add("Dvorak");
			this.layoutPicker.Items.Add("Qwerty");
			this.layoutPicker.SelectedIndex = 0;
			this.random = new Random();
			this.showHighscores();
			this.currentTypedWords = Regex.Matches("", "a");
			this.loadWords();
			this.oldText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

			this.displayedText.BorderStyle = BorderStyle.None;
			this.displayedText.Enabled = false;
			
			this.blackenDisplayedText();
		}

		private void blackenDisplayedText()
		{
			this.displayedText.SelectAll();
			this.displayedText.SelectionColor = Color.Black;
		}

		private void showHighscores()
		{
			List<int> highscores = this.readHighscore();
			if (highscores.Count == 2)
			{
				this.highscoreQWERTY = highscores[0];
				this.highscoreDVORAK = highscores[1];
			}
			this.bestQwertyWPM.Text = this.highscoreQWERTY.ToString();
			this.bestDvorakWPM.Text = this.highscoreDVORAK.ToString();
		}

		private void layoutPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.layoutPicker.SelectedIndex == 0)
			{
				this.keyboardPicture.Image = Properties.Resources.dvorak;
			}
			else if (this.layoutPicker.SelectedIndex == 1)
			{
				this.keyboardPicture.Image = Properties.Resources.qwerty;
			}
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (InputLanguage.CurrentInputLanguage.LayoutName.Equals("US"))
			{
				this.layoutPicker.SelectedIndex = 1;
			}
			else if (InputLanguage.CurrentInputLanguage.LayoutName.Equals("United States-Dvorak"))
			{
				this.layoutPicker.SelectedIndex = 0;
			}

			this.resetFields();
			this.currentCorrectLine = getRandomSentence();
			this.displayedText.Text = this.currentCorrectLine;
			this.currentCorrectWords = Regex.Matches(this.currentCorrectLine, @"[\S]+");
			this.inputText.Text = String.Empty;
			this.inputText.Focus();
			this.startTime = DateTime.Now;
			this.wpm.Text = "0";
			this.wordsTyped.Text = "0";
			this.errors.Text = "0";
			this.numMins = (int)this.timePicker.Value;
			this.timeRemaining.Text = ((int)(this.numMins * 60.0)).ToString();
			this.unsetReadyNextLine();
			this.timer1.Start();
			this.inputText.Enabled = true;
			this.colorNextCharacter();
			this.running = true;
			this.layoutPicker.Enabled = false;
			this.buttonStart.Enabled = false;
			this.buttonStop.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.elapsedSeconds = (double)(DateTime.Now - this.startTime).TotalSeconds;
			double maxSeconds = this.numMins * 60.0;
			double timeRemaining = maxSeconds - this.elapsedSeconds;
			this.timeRemaining.Text = ((int)timeRemaining).ToString();
			this.updateStats();
			if (timeRemaining <= 0)
			{
				this.timeUp();
			}
		}

		private int getErrors(MatchCollection typedWords)
		{
			int errors = 0;
			int numWords = Math.Min(typedWords.Count, currentCorrectWords.Count);
			for (int i = 0; i < numWords - 1; i++)
			{
				var typed = typedWords[i].Value;
				var correct = currentCorrectWords[i].Value;
				if (!typed.Equals(correct))
				{
					errors++;
				}
			}

			if (numWords > 0)
			{
				var typedLast = typedWords[numWords - 1].Value;
				var correctLast = currentCorrectWords[numWords - 1].Value;
				if (typedLast.Length >= correctLast.Length && !typedLast.Equals(correctLast))
				{
					errors++;
				}
			}

			return errors;
		}

		private void updateStats()
		{
			this.inputText.SelectionStart = this.inputText.Text.Length;
			int currentTypedWords = this.currentTypedWords.Count;
			int errors = getErrors(this.currentTypedWords);
						
			int overallTotalWords = (this.totalWords + currentTypedWords);
			int overallTotalErrors = (this.totalErrorWords + errors);
			this.wordsTyped.Text = overallTotalWords.ToString();
			this.errors.Text = overallTotalErrors.ToString();
			this.elapsedSeconds = (double)((DateTime.Now - this.startTime).TotalMilliseconds) / 1000;
			int currentWPM = (int)(((double)(overallTotalWords - overallTotalErrors) / this.elapsedSeconds * 60) + 0.5);
			//this.wpm.Text = currentWPM.ToString();

			if (this.currentCorrectWords.Count == this.currentTypedWords.Count &&
				this.currentTypedWords[currentTypedWords - 1].Length == this.currentCorrectWords[currentTypedWords - 1].Length &&
				!this.isReadyNextLine())
			{
				this.setReadyNextLine();
			}
		}

		private void finishRun()
		{

		}

		private List<int> readHighscore()
		{
			List<int> nums = new List<int>();
			string filePath = this.highscorePath + this.highscoreFile;
			System.IO.StreamReader reader = null;
			try
			{
				reader = new System.IO.StreamReader(@filePath);
				int qwertyWPM = int.Parse(reader.ReadLine());
				int dvorakWPM = int.Parse(reader.ReadLine());
				nums.Add(qwertyWPM);
				nums.Add(dvorakWPM);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (reader != null) reader.Dispose();
			}
			return nums;
		}


		private void newLine()
		{
			this.currentTypedWords = Regex.Matches(this.inputText.Text, @"[\S]+");
			int errors = getErrors(this.currentTypedWords);
			this.totalErrorWords += errors;
			this.totalWords += this.currentTypedWords.Count;
			this.correctStrings.Add(this.currentCorrectLine);
			this.typedStrings.Add(this.inputText.Text);
			this.currentCorrectLine = getRandomSentence();
			this.currentCorrectWords = Regex.Matches(this.currentCorrectLine, @"[\S]+");
			this.displayedText.Text = this.currentCorrectLine;
			this.inputText.Text = String.Empty;
			this.oldText.Text = String.Empty;
			this.inputText.SelectionStart = this.inputText.Text.Length;
			this.currentTypedWords = Regex.Matches("", "a");
			this.colorNextCharacter();
		}

		private void inputText_CursorChanged(object sender, EventArgs e)
		{
			if (!this.errorColoring)
			{
				this.inputText.SelectionLength = 0;
				this.inputText.SelectionStart = this.inputText.Text.Length;
			}
		}

		private bool checkTooLong()
		{
			bool tooLong = false;
			int numTyped = this.currentTypedWords.Count;
			int correctNum = this.currentCorrectWords.Count;
			if (numTyped == correctNum &&
				this.currentTypedWords[numTyped - 1].Length > this.currentCorrectWords[correctNum - 1].Length)
			{
				tooLong = true;
			}

			if (numTyped == correctNum &&
				this.inputText.Text.Last().Equals(' '))
			{
				tooLong = true;
			}

			return tooLong;
		}

		private void inputText_TextChanged(object sender, EventArgs e)
		{
			if (!this.running)
			{
				return;
			}

			// Remove leading whitespace
			if (this.inputText.Text.Equals(" ") || this.inputText.Text.Equals("\t"))
			{
				this.inputText.Rtf = this.oldText.Rtf;
				this.updateStats();
				return;
			}
			if (this.inputText.Text.Length > 1 && this.inputText.Text.Last().Equals(" ") &&
				this.inputText.Text[this.inputText.Text.Length - 2].Equals(" "))
			{
				this.inputText.Rtf = this.oldText.Rtf;
				this.updateStats();
				return;
			}

			this.currentTypedWords = Regex.Matches(this.inputText.Text, @"[\S]+");
			// Ensure not typing longer than last word.
			if (this.checkTooLong())
			{
				this.inputText.Rtf = this.oldText.Rtf.TrimEnd(' ');
				this.oldText.Rtf = this.oldText.Rtf.TrimEnd(' ');
				this.updateStats();
				return;
			}
			// Handle typing a newline character
			else if (this.inputText.Text.Length > 0 && this.inputText.Text.Last().Equals('\n'))
			{
				this.inputText.Rtf = this.oldText.Rtf;
				if (this.isReadyNextLine())
				{
					this.unsetReadyNextLine();
					this.newLine();
				}
				this.updateStats();
				return;
			}

			// Handle when background is green
			if (this.isReadyNextLine())
			{
				if (this.inputText.Text.Length < this.oldText.Text.Length)
				{
					this.oldText.Rtf = this.inputText.Rtf;
					this.unsetReadyNextLine();
					this.colorNextCharacter();
				}
				else
				{
					this.inputText.Rtf = this.oldText.Rtf;
				}
			}
			// Handle everything else
			else if (this.inputText.Text.All(chr => (char.IsLetter(chr) ||
												char.IsDigit(chr) ||
												char.IsWhiteSpace(chr) ||
												char.IsPunctuation(chr))))
			{
				this.makeErrorsRed();
				this.colorNextCharacter();
				this.oldText.Rtf = this.inputText.Rtf;
			}
			else
			{
				this.inputText.Rtf = this.oldText.Rtf;
			}
			this.updateStats();
		}

		private void makeErrorsRed()
		{
			this.errorColoring = true;

			int numWords = this.currentTypedWords.Count;
			if (numWords == 0)
			{
				return;
			}
			var currentWord = this.currentTypedWords[numWords - 1];
			var correctWord = this.currentCorrectWords[numWords - 1];

			int index = currentWord.Length - 1;

			bool makeRed = false;
			if (index > correctWord.Length - 1)
			{
				//this.debugTextBox.Text += "WAS TOO LONG!";
				makeRed = true;
			}
			else if (currentWord.Value[index] != correctWord.Value[index])
			{
				//this.debugTextBox.Text += "WAS NOT LETTER";
				makeRed = true;
			}
			//this.debugTextBox.Text += "makeRed is " + makeRed.ToString();

			Color toColor = Color.Black;
			if (makeRed)
			{
				toColor = Color.Red;
			}


			int lastIndex = this.inputText.Text.Length - 1;
			while (this.inputText.Text[lastIndex] == ' ' && lastIndex >= 0)
			{
				lastIndex--;
			}
			inputText.Select(lastIndex, 1);
			inputText.SelectionColor = toColor;
			inputText.SelectionStart = inputText.Text.Length;

			this.errorColoring = false;
		}

		private void colorNextCharacter()
		{
			displayedText.Select(0, displayedText.Text.Length);
			displayedText.SelectionColor = Color.Black;
			displayedText.SelectionBackColor = Color.White;

			if (this.isReadyNextLine())
			{
				return;
			}

			int index = 0;
			int numWords = Math.Min(this.currentTypedWords.Count, this.currentCorrectWords.Count);
			for (int i = 0; i < numWords - 1; i++)
			{
				var correct = currentCorrectWords[i].Value;
				index += correct.Length;
				index += 1;
			}

			if (numWords > 0)
			{
				var currentTyped = this.currentTypedWords[numWords - 1];
				var currentCorrect = this.currentCorrectWords[numWords - 1];
				index += Math.Min(currentTyped.Length, currentCorrect.Length);
				if (this.inputText.Text.Last().Equals(' '))
				{
					index++;
				}
			}

			displayedText.Select(0, displayedText.Text.Length);
			displayedText.SelectionColor = Color.Black;
			displayedText.SelectionBackColor = Color.White;
			displayedText.Select(index, 1);
			displayedText.SelectionColor = Color.Black;
			displayedText.SelectionBackColor = this.nextScreenReadyColor;
			this.inputText.SelectionStart = this.inputText.Text.Length;
		}

		private void outputWPM()
		{
			int totalChars = 0;
			foreach (var sentence in this.typedStrings)
			{
				int chars = sentence.Length;
				totalChars += chars;
			}

			totalChars += this.inputText.Text.Length;

			int wpm = totalChars / 5;
			int errors = getErrors(this.currentTypedWords);
			wpm -= (this.totalErrorWords + errors) * 2;
			wpm = Math.Max(0, wpm);
			this.wpm.Text = wpm.ToString();
		}

		private void timeUp()
		{
			this.timer1.Stop();
			this.running = false;
			this.inputText.Enabled = false;
			this.outputWPM();
			System.Threading.Thread.Sleep(1000);
			MessageBox.Show("Typing session complete!", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
			int wpmTyped = int.Parse(this.wpm.Text);
			this.resetFields();
			if (this.layoutPicker.SelectedIndex == 0)
			{
				if (wpmTyped > this.highscoreDVORAK)
				{
					this.highscoreDVORAK = wpmTyped;
					this.writeHighscores();
					MessageBox.Show("New Highscore!");
				}
			}
			else
			{
				if (wpmTyped > this.highscoreQWERTY)
				{
					this.highscoreQWERTY = wpmTyped;
					this.writeHighscores();
					MessageBox.Show("New Highscore!");
				}
			}
			this.displayedText.Text = string.Empty;
			this.inputText.Text = string.Empty;
			this.inputText.SelectionStart = this.inputText.Text.Length;
			this.bestQwertyWPM.Text = this.highscoreQWERTY.ToString();
			this.bestDvorakWPM.Text = this.highscoreDVORAK.ToString();
			this.inputText.Enabled = true;
			this.unsetReadyNextLine();
			this.buttonStart.Enabled = true;
			this.buttonStop.Enabled = false;
			this.layoutPicker.Enabled = true;
		}

		private void loadWords()
		{
			string filePath = this.wordListpath + this.wordListfile;
			System.IO.StreamReader reader = null;
			try
			{	
				reader = new System.IO.StreamReader(@filePath);
				while (reader.EndOfStream != true)
				{
					string word = reader.ReadLine();
					this.wordList.Add(word);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (reader != null) reader.Dispose();
			}
		}

		private string getRandomSentence()
		{
			string sentence = string.Empty;
			int numWords = random.Next(4, 10);
			for (int i = 0; i < numWords; i++)
			{
				string word = this.getRandomWord();
				if (i < numWords && i > 0)
				{
					sentence += " ";
				}
				sentence += word;
			}
			sentence += ".";
			return sentence;
		}

		private string getRandomWord()
		{
			int index = random.Next(0, this.wordList.Count); // Zero to 25
			string word = this.wordList[index];
			return word;
		}

		private void resetFields()
		{
			this.elapsedSeconds = 0;
			this.numMins = 0;
			this.running = false;
			this.correctStrings.Clear();
			this.typedStrings.Clear();
			this.currentCorrectLine = string.Empty;
			this.currentCorrectWords = Regex.Matches("", "a");
			this.currentTypedWords = Regex.Matches("", "a");
			this.totalWords = 0;
			this.totalErrorWords = 0;
			this.errorColoring = false;
			this.oldText.Text = string.Empty;
		}

		private void writeHighscores()
		{
			string filePath = this.highscorePath + this.highscoreFile;
			System.IO.StreamWriter writer = null;
			try
			{
				writer = new System.IO.StreamWriter(@filePath);
				writer.WriteLine(this.highscoreQWERTY);
				writer.WriteLine(this.highscoreDVORAK);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (writer != null) writer.Dispose();
			}
		}

		private string generateRandomSentence()
		{
			string sentence = String.Empty;
			int numWords = random.Next(4, 12);
			for (int i = 0; i < numWords; i++)
			{
				if (i > 0)
				{
					sentence += " ";
				}

				int wordLength = random.Next(2, 9);
				for (int j = 0; j < wordLength; j++)
				{
					char letter = getRandomLetter();
					sentence += letter.ToString();
				}
				
			}
			sentence += ".";
			return sentence;
		}

		private char getRandomLetter()
		{
			int num = random.Next(0, 26); // Zero to 25
			char let = (char)('a' + num);
			return let;
		}

		private bool isReadyNextLine()
		{
			if (this.inputText.BackColor == this.nextScreenReadyColor)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void setReadyNextLine()
		{
			this.inputText.BackColor = this.nextScreenReadyColor;
		}

		private void unsetReadyNextLine()
		{
			this.inputText.BackColor = Color.White;
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			this.updateStats();
			this.timeUp();
		}
	}
}
