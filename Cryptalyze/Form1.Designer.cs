namespace Cryptalyze
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbAlphabet = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCipher = new System.Windows.Forms.TextBox();
            this.tbKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCiphers = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbIC = new System.Windows.Forms.TextBox();
            this.lvFrequencies = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label7 = new System.Windows.Forms.Label();
            this.tbHasJ = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbHasPound = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbHasNumbers = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lvICByKeyLength = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbFactors = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbAlphabetKey = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tbUsedAlphabet = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbClear = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnAttempt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Alphabet";
            // 
            // tbAlphabet
            // 
            this.tbAlphabet.Location = new System.Drawing.Point(17, 35);
            this.tbAlphabet.Margin = new System.Windows.Forms.Padding(4);
            this.tbAlphabet.Name = "tbAlphabet";
            this.tbAlphabet.Size = new System.Drawing.Size(299, 31);
            this.tbAlphabet.TabIndex = 1;
            this.tbAlphabet.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.tbAlphabet.TextChanged += new System.EventHandler(this.tbAlphabet_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cipher";
            // 
            // tbCipher
            // 
            this.tbCipher.Location = new System.Drawing.Point(17, 107);
            this.tbCipher.Margin = new System.Windows.Forms.Padding(4);
            this.tbCipher.Multiline = true;
            this.tbCipher.Name = "tbCipher";
            this.tbCipher.Size = new System.Drawing.Size(415, 203);
            this.tbCipher.TabIndex = 3;
            this.tbCipher.TextChanged += new System.EventHandler(this.tbCipher_TextChanged);
            // 
            // tbKey
            // 
            this.tbKey.Location = new System.Drawing.Point(17, 348);
            this.tbKey.Margin = new System.Windows.Forms.Padding(4);
            this.tbKey.Name = "tbKey";
            this.tbKey.Size = new System.Drawing.Size(248, 31);
            this.tbKey.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 322);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Key";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 381);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(298, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "I think this cipher is a";
            // 
            // cbCiphers
            // 
            this.cbCiphers.FormattingEnabled = true;
            this.cbCiphers.Location = new System.Drawing.Point(17, 407);
            this.cbCiphers.Name = "cbCiphers";
            this.cbCiphers.Size = new System.Drawing.Size(248, 31);
            this.cbCiphers.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(416, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "N";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(440, 315);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(100, 31);
            this.tbCount.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(407, 388);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 24);
            this.label6.TabIndex = 12;
            this.label6.Text = "IC";
            // 
            // tbIC
            // 
            this.tbIC.Location = new System.Drawing.Point(440, 381);
            this.tbIC.Name = "tbIC";
            this.tbIC.Size = new System.Drawing.Size(100, 31);
            this.tbIC.TabIndex = 13;
            // 
            // lvFrequencies
            // 
            this.lvFrequencies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvFrequencies.FullRowSelect = true;
            this.lvFrequencies.Location = new System.Drawing.Point(831, 108);
            this.lvFrequencies.Name = "lvFrequencies";
            this.lvFrequencies.Size = new System.Drawing.Size(157, 399);
            this.lvFrequencies.TabIndex = 14;
            this.lvFrequencies.UseCompatibleStateImageBehavior = false;
            this.lvFrequencies.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Alphabet";
            this.columnHeader1.Width = 93;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Count";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(832, 290);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frequencies";
            // 
            // tbHasJ
            // 
            this.tbHasJ.Location = new System.Drawing.Point(440, 414);
            this.tbHasJ.Name = "tbHasJ";
            this.tbHasJ.Size = new System.Drawing.Size(100, 31);
            this.tbHasJ.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(380, 417);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 24);
            this.label8.TabIndex = 16;
            this.label8.Text = "Has J";
            // 
            // tbHasPound
            // 
            this.tbHasPound.Location = new System.Drawing.Point(440, 447);
            this.tbHasPound.Name = "tbHasPound";
            this.tbHasPound.Size = new System.Drawing.Size(100, 31);
            this.tbHasPound.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(380, 450);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 24);
            this.label9.TabIndex = 18;
            this.label9.Text = "Has #";
            // 
            // tbHasNumbers
            // 
            this.tbHasNumbers.Location = new System.Drawing.Point(440, 480);
            this.tbHasNumbers.Name = "tbHasNumbers";
            this.tbHasNumbers.Size = new System.Drawing.Size(100, 31);
            this.tbHasNumbers.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(362, 483);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 24);
            this.label10.TabIndex = 20;
            this.label10.Text = "Has #\'s";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(998, 83);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(202, 24);
            this.label11.TabIndex = 23;
            this.label11.Text = "IC by Key Length";
            // 
            // lvICByKeyLength
            // 
            this.lvICByKeyLength.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvICByKeyLength.FullRowSelect = true;
            this.lvICByKeyLength.Location = new System.Drawing.Point(994, 108);
            this.lvICByKeyLength.Name = "lvICByKeyLength";
            this.lvICByKeyLength.Size = new System.Drawing.Size(157, 399);
            this.lvICByKeyLength.TabIndex = 22;
            this.lvICByKeyLength.UseCompatibleStateImageBehavior = false;
            this.lvICByKeyLength.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Key Len";
            this.columnHeader3.Width = 93;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "I.C.";
            // 
            // tbFactors
            // 
            this.tbFactors.Location = new System.Drawing.Point(440, 348);
            this.tbFactors.Name = "tbFactors";
            this.tbFactors.Size = new System.Drawing.Size(100, 31);
            this.tbFactors.TabIndex = 25;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(161, 480);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 23);
            this.label20.TabIndex = 26;
            // 
            // tbAlphabetKey
            // 
            this.tbAlphabetKey.Location = new System.Drawing.Point(324, 35);
            this.tbAlphabetKey.Margin = new System.Windows.Forms.Padding(4);
            this.tbAlphabetKey.Name = "tbAlphabetKey";
            this.tbAlphabetKey.Size = new System.Drawing.Size(299, 31);
            this.tbAlphabetKey.TabIndex = 17;
            this.tbAlphabetKey.TextChanged += new System.EventHandler(this.tbAlphabetKey_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(320, 9);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(154, 24);
            this.label18.TabIndex = 16;
            this.label18.Text = "Alphabet Key";
            // 
            // tbUsedAlphabet
            // 
            this.tbUsedAlphabet.Location = new System.Drawing.Point(631, 35);
            this.tbUsedAlphabet.Name = "tbUsedAlphabet";
            this.tbUsedAlphabet.ReadOnly = true;
            this.tbUsedAlphabet.Size = new System.Drawing.Size(358, 31);
            this.tbUsedAlphabet.TabIndex = 0;
            this.tbUsedAlphabet.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(627, 9);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(166, 24);
            this.label19.TabIndex = 18;
            this.label19.Text = "Used Alphabet";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(831, 83);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(142, 24);
            this.label13.TabIndex = 27;
            this.label13.Text = "Frequencies";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(317, 351);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 24);
            this.label12.TabIndex = 28;
            this.label12.Text = "Factors of N";
            // 
            // tbClear
            // 
            this.tbClear.Location = new System.Drawing.Point(440, 105);
            this.tbClear.Margin = new System.Windows.Forms.Padding(4);
            this.tbClear.Multiline = true;
            this.tbClear.Name = "tbClear";
            this.tbClear.Size = new System.Drawing.Size(384, 203);
            this.tbClear.TabIndex = 29;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(439, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 24);
            this.label14.TabIndex = 30;
            this.label14.Text = "Clear";
            // 
            // btnAttempt
            // 
            this.btnAttempt.Location = new System.Drawing.Point(17, 455);
            this.btnAttempt.Name = "btnAttempt";
            this.btnAttempt.Size = new System.Drawing.Size(244, 48);
            this.btnAttempt.TabIndex = 31;
            this.btnAttempt.Text = "Attempt";
            this.btnAttempt.UseVisualStyleBackColor = true;
            this.btnAttempt.Click += new System.EventHandler(this.btnAttempt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 523);
            this.Controls.Add(this.btnAttempt);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbClear);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbUsedAlphabet);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbAlphabetKey);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbFactors);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lvICByKeyLength);
            this.Controls.Add(this.tbHasNumbers);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbHasPound);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.tbHasJ);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.lvFrequencies);
            this.Controls.Add(this.tbIC);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCiphers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbCipher);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbAlphabet);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAlphabet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCipher;
        private System.Windows.Forms.TextBox tbKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCiphers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbIC;
        private System.Windows.Forms.ListView lvFrequencies;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbHasJ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbHasPound;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbHasNumbers;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListView lvICByKeyLength;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox tbFactors;
        private System.Windows.Forms.TextBox tbAlphabetKey;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbUsedAlphabet;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbClear;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnAttempt;
    }
}

