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
using System.IO;

namespace TypingPractice
{
	public partial class Form1 : Form
	{
		private DateTime startTime; // The time the user clicks start
		private double elapsedSeconds; // Number of seconds since user clicked start
		private RichTextBox oldText = new RichTextBox(); // To revert recently typed character
		private int numMins; // Number of minutes user chose to type for
		private bool running; // Whether or not a typing session is currently occurring
		private List<string> correctStrings = new List<string>(); // List of all correct strings to type during a session
		private List<string> typedStrings = new List<string>(); // List of all strings that were typed during a session
		private string currentCorrectLine; // The current line that the user should be trying to type
		private MatchCollection currentCorrectWords; // The words of the current line to be typed
		private MatchCollection currentTypedWords; // The words of the current line that the user has typed
		private int totalWords; // Number of total words of previous sentences for current session
		private int totalErrorWords; // Number of total errors of previous sentences for current session
		private Color nextScreenReadyColor = Color.LimeGreen; // Color to signal the user that they have finished a sentence
		private Random random; // A random generator to be used to generate random sentences
        private string highscoreFile = "..\\..\\..\\Highscores\\highscores.txt"; // Name of the high score file
        private string wordListFile = "..\\..\\..\\..\\WordLists\\wordlist.txt"; // Default word list file
		private int highscoreQWERTY = 0; // Current high score for QWERTY words per minute
		private int highscoreDVORAK = 0; // Current high score for Dvorak words per minute
		private bool errorColoring = false; // If the input box is currently being colored for errors
		private List<string> wordList = new List<string>(); // A list of all possible words to generate a sentence from

        /// <summary>
        /// Initializes the form for typing practice
        /// </summary>
		public Form1()
		{
			InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            if (Properties.Settings.Default.WordListFile != null && Properties.Settings.Default.WordListFile.Length > 0)
            {
                this.wordListFile = Properties.Settings.Default.WordListFile;
            }
            else
            {
                var absolute_path = Path.Combine(Application.ExecutablePath, this.wordListFile);
                this.wordListFile = Path.GetFullPath((new Uri(absolute_path)).LocalPath);
            }

            // Comment this out to use a textbox to debug with
            this.debugTextBox.Visible = false;

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

        /// <summary>
        /// Makes the color of all the displayed text black
        /// </summary>
		private void blackenDisplayedText()
		{
			this.displayedText.SelectAll();
			this.displayedText.SelectionColor = Color.Black;
		}

        /// <summary>
        /// Reads and displays all the current high scores for typing speed
        /// </summary>
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

        /// <summary>
        /// Toggle the displayed keyboard layout depending on which option is picked
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
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

        /// <summary>
        /// When the user clicks start, it begins the typing session, resetting various information
        /// and displaying a sentence to be typed, and starts the timer
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
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

        /// <summary>
        /// Called in increments of 100 milliseconds to check for timeout, and update timed information
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
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

        /// <summary>
        /// Calculates the number of errors given a set of typed words, by comparing the
        /// sequential words
        /// </summary>
        /// <param name="typedWords">
        /// A match collection of the words that have been typed
        /// </param>
        /// <returns>
        /// The number of errors between the typed words and current correct words
        /// </returns>
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

        /// <summary>
        /// Updates statistics regarding errors and words typed
        /// </summary>
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

        /// <summary>
        /// Reads the highscore file and returns a list of the 2 highscores, or
        /// displays a messagebox if there was an exception
        /// </summary>
        /// <returns>
        /// A list of the 2 highscores, the QWERTY highscore first
        /// </returns>
		private List<int> readHighscore()
		{
			List<int> nums = new List<int>();
			string filePath = this.highscoreFile;
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

        /// <summary>
        /// Called when the user has finished a sentence and presses enter, it will
        /// display a new sentence of words to be typed.
        /// </summary>
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

        /// <summary>
        /// Ensures that the cursor is always at the end of the textbox, unless letters are currently
        /// being colored for errors. This is so the user can't start typing in the middle of their
        /// sentence on accident, which would cause many problems.
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
		private void inputText_CursorChanged(object sender, EventArgs e)
		{
			if (!this.errorColoring)
			{
				this.inputText.SelectionLength = 0;
				this.inputText.SelectionStart = this.inputText.Text.Length;
			}
		}

        /// <summary>
        /// Checks if the user is currently typing the last word, and is typing it too long, or
        /// is typing spaces after the last word
        /// </summary>
        /// <returns>
        /// True if the user is typing past the last word
        /// </returns>
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

        /// <summary>
        /// Called when text in the input text box changes, it ensures the textbox doesn't start with
        /// whitespace, isn't too long, and it updates the colors of text depending on if it was typed
        /// correctly or incorrectly
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
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

            // Supposed to compact ending whitespace? Doesn't work?
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

        /// <summary>
        /// Colors the last typed letter red or black depending on if it was
        /// correct or not
        /// </summary>
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
				makeRed = true;
			}
			else if (currentWord.Value[index] != correctWord.Value[index])
			{
				makeRed = true;
			}

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

        /// <summary>
        /// Finds the next character that should be typed by the user, and gives it
        /// a green background
        /// </summary>
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

        /// <summary>
        /// Calculates the words typed per minute and displays it
        /// </summary>
		private void outputWPM()
		{
			int totalChars = 0;
			foreach (var sentence in this.typedStrings)
			{
				int chars = sentence.Length;
				totalChars += chars;
			}

			totalChars += this.inputText.Text.Length;

            // Standard word lengths are 5 characters, including spaces
			int wpm = totalChars / 5;
			int errors = getErrors(this.currentTypedWords);

            // Subtract double the number of error words typed
			wpm -= (this.totalErrorWords + errors) * 2;
			wpm = Math.Max(0, wpm);
            wpm /= (int)this.timePicker.Value;
			this.wpm.Text = wpm.ToString();
		}

        /// <summary>
        /// Called when the timer expires, tries to stop user input, calculates the WPM, and
        /// updates highscores
        /// </summary>
		private void timeUp()
		{
			this.timer1.Stop();
			this.running = false;
			this.inputText.Enabled = false;
			this.outputWPM();
            var infoBox = new InfoBox("Typing session complete!", "Finished");
            infoBox.ShowDialog();
			int wpmTyped = int.Parse(this.wpm.Text);
			this.resetFields();
			if (this.layoutPicker.SelectedIndex == 0)
			{
				if (wpmTyped > this.highscoreDVORAK)
				{
                    var dvorakBox = new InfoBox("You typed " + wpmTyped + " WPM in Dvorak - a new high score!", "Dvorak High Score");
                    dvorakBox.ShowDialog();

                    this.highscoreDVORAK = wpmTyped;
                    this.writeHighscores();
				}
			}
			else
			{
				if (wpmTyped > this.highscoreQWERTY)
				{
                    var qwertyBox = new InfoBox("You typed " + wpmTyped + " WPM in QWERTY - a new high score!", "QWERTY High Score");
                    qwertyBox.ShowDialog();

					this.highscoreQWERTY = wpmTyped;
					this.writeHighscores();
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

        /// <summary>
        /// Reads from the word list file to be used to generate random sentences.
        /// </summary>
		private void loadWords()
		{
			string filePath = this.wordListFile;
			System.IO.StreamReader reader = null;
            this.wordList.Clear();
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

        /// <summary>
        /// Generates a sequence of words from the word list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Picks a random word from the saved word list
        /// </summary>
        /// <returns>
        /// The random word
        /// </returns>
		private string getRandomWord()
		{
			int index = random.Next(0, this.wordList.Count);
            string word;
            if (this.wordList.Count == 0)
            {
                word = "empty";
            }
            else
            {
                word = this.wordList[index];
            }

			return word;
		}

        /// <summary>
        /// Resets all the information associated with a typing session
        /// </summary>
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

        /// <summary>
        /// Writes the new highscores back to the highscore file
        /// </summary>
		private void writeHighscores()
		{
			string filePath = this.highscoreFile;
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

        /// <summary>
        /// Checks if the user has completed the current sentence, if the back color has been
        /// changed to green
        /// </summary>
        /// <returns>
        /// True if the user has finished the current sentence, false otherwise
        /// </returns>
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

        /// <summary>
        /// Sets the back color of the input box to green to signal to the user that they
        /// can press enter to get the next sentence.
        /// </summary>
		private void setReadyNextLine()
		{
			this.inputText.BackColor = this.nextScreenReadyColor;
		}

        /// <summary>
        /// Sets the back color of the input box to white to signal to the user that they
        /// still need to type more to be able to get to the next sentence.
        /// </summary>
		private void unsetReadyNextLine()
		{
			this.inputText.BackColor = Color.White;
		}

        /// <summary>
        /// Called when the user clicks the stop button, displays the stats, but doesn't
        /// write to a file incase the user tried to cheat, they must complete the full
        /// time session
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
		private void buttonStop_Click(object sender, EventArgs e)
		{
			this.updateStats();
			this.timeUp();
		}

        /// <summary>
        /// Called when the user clicks the word list button. It sets the initial directory and
        /// filename of the open file dialog, and then displays the dialog
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
        private void buttonWordList_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Path.GetDirectoryName(this.wordListFile);
            this.openFileDialog1.FileName = Path.GetFileName(this.wordListFile);
            this.openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Called when the user selects a word list file. It saves the file as the default
        /// word list, and then will load the words in the file.
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.wordListFile = this.openFileDialog1.FileName;
            Properties.Settings.Default.WordListFile = this.wordListFile;
            Properties.Settings.Default.Save();
            this.loadWords();
        }

        /// <summary>
        /// Displays a message box for the user to confirm they want to reset their QWERTY wpm highscore. It then
        /// sets the highscore back to 0 and saves it to the highscore file.
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
        private void buttonResetQWERTY_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("QWERTY highscore will be set back to 0.", "Reset QWERTY", MessageBoxButtons.OKCancel);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.highscoreQWERTY = 0;
                this.bestQwertyWPM.Text = this.highscoreQWERTY.ToString();
                this.writeHighscores();
            }
        }

        /// <summary>
        /// Displays a message box for the user to confirm they want to reset their DVORAK wpm highscore. It then
        /// sets the highscore back to 0 and saves it to the highscore file.
        /// </summary>
        /// <param name="sender">
        /// The object calling this method
        /// </param>
        /// <param name="e">
        /// Arguments associated with this event
        /// </param>
        private void buttonResetDVORAK_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("DVORAK highscore will be set back to 0.", "Reset DVORAK", MessageBoxButtons.OKCancel);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.highscoreDVORAK = 0;
                this.bestDvorakWPM.Text = this.highscoreDVORAK.ToString();
                this.writeHighscores();
            }
        }
	}
}
