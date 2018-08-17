using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cryptalyze
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbCiphers.Items.Add("Affine");
            cbCiphers.Items.Add("Atbash");
            cbCiphers.Items.Add("Baconian");
            cbCiphers.Items.Add("Beaufort");
            cbCiphers.Items.Add("Columnar");
            cbCiphers.Items.Add("Homophonic");
            cbCiphers.Items.Add("Polybius");
            cbCiphers.Items.Add("Porta");
            cbCiphers.Items.Add("Rail Fence");
            cbCiphers.Items.Add("Rotation");
            cbCiphers.Items.Add("Skip");
            cbCiphers.Items.Add("Substitution");
            cbCiphers.Items.Add("Vigenere");

            



        }

        private void tbAlphabet_TextChanged(object sender, EventArgs e)
        {
            if (tbAlphabetKey.Text.Length > 0)
            {
                string alphabet = tbAlphabet.Text.ToUpper();
                foreach (char c in tbAlphabetKey.Text.ToUpper())
                {
                    alphabet = alphabet.Replace(c.ToString(), "");
                }
                tbUsedAlphabet.Text = tbAlphabetKey.Text.ToUpper() + alphabet;
            }
            else
            {
                tbUsedAlphabet.Text = tbAlphabet.Text.ToUpper();
            }
        }

        private void tbAlphabetKey_TextChanged(object sender, EventArgs e)
        {
            if (tbAlphabetKey.Text.Length > 0)
            {
                string alphabet = tbAlphabet.Text.ToUpper();
                foreach (char c in tbAlphabetKey.Text.ToUpper())
                {
                    alphabet = alphabet.Replace(c.ToString(), "");
                }
                tbUsedAlphabet.Text = tbAlphabetKey.Text.ToUpper() + alphabet;
            }
            else
            {
                tbUsedAlphabet.Text = tbAlphabet.Text.ToUpper();
            }
        }

        string originalCipher = "";
        private void tbCipher_TextChanged(object sender, EventArgs e)
        {
            StringBuilder cipher = new StringBuilder();

            foreach (char c in tbCipher.Text.ToUpper())
            {
                if (tbUsedAlphabet.Text.IndexOf(c) >= 0)
                {
                    cipher.Append(c.ToString());
                }
            }
            tbCipher.Text = cipher.ToString();

            CalculateStats();
        }

        public void CalculateStats()
        {


            tbCount.Text = tbCipher.Text.Trim().Length + "/" + tbCipher.Text.Trim().Replace(" ", "").Length;

            StringBuilder factors = new StringBuilder();
            for (int i = 2; i < Math.Sqrt(tbCipher.Text.Trim().Length); i++)
            {
                if (tbCipher.Text.Trim().Length % i == 0)
                {
                    factors.Append(i);
                    factors.Append(",");
                }
            }
            if (factors.Length == 0)
            {
                tbFactors.Text = "Prime";
            }
            else
            {
                factors.Length--;
                tbFactors.Text = factors.ToString();
            }
            tbHasJ.Text = ((tbCipher.Text.IndexOf("j") >= 0 || tbCipher.Text.IndexOf("J") >= 0) ? "Yes" : "");
            tbHasPound.Text = ((tbCipher.Text.IndexOf("#") >= 0) ? "Yes" : "");
            tbHasNumbers.Text = ((tbCipher.Text.IndexOf("0") >= 0 || tbCipher.Text.IndexOf("1") >= 0 || tbCipher.Text.IndexOf("2") >= 0 || tbCipher.Text.IndexOf("3") >= 0 || tbCipher.Text.IndexOf("4") >= 0
                || tbCipher.Text.IndexOf("5") >= 0 || tbCipher.Text.IndexOf("6") >= 0 || tbCipher.Text.IndexOf("7") >= 0 || tbCipher.Text.IndexOf("8") >= 0 || tbCipher.Text.IndexOf("9") >= 0) ? "Yes" : "");

            tbIC.Text = CryptSharp.Utility.IndexOfCoincidence(tbCipher.Text, tbUsedAlphabet.Text).ToString();

            lvFrequencies.Items.Clear();
            Dictionary<char, int> frequencies = CryptSharp.Utility.Frequencies(tbCipher.Text.ToCharArray(), tbUsedAlphabet.Text.ToCharArray());
            foreach (char c in frequencies.Keys)
            {
                ListViewItem i = new ListViewItem();

                i.Text = c.ToString();
                i.SubItems.Add(frequencies[c].ToString());

                lvFrequencies.Items.Add(i);
            }

            lvICByKeyLength.Items.Clear();
            for (int i = 1; i < tbCipher.Text.Trim().Length / 2; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Text = i.ToString();
                item.SubItems.Add(CryptSharp.Utility.AvgVigenereIndexOfCoincidence(tbCipher.Text.Trim(), tbUsedAlphabet.Text, i).ToString());

                lvICByKeyLength.Items.Add(item);
            }
        }

        private void btnAttempt_Click(object sender, EventArgs e)
        {
            switch(cbCiphers.SelectedItem.ToString())
            {
                case "Vigenere":
                    CryptSharp.Ciphers.Classical.Vigenere v = new CryptSharp.Ciphers.Classical.Vigenere(tbUsedAlphabet.Text.ToCharArray());
                    v.Key = tbKey.Text;
                    tbClear.Text = v.Decrypt(tbCipher.Text);
                    break;

                case "Beaufort":
                    CryptSharp.Ciphers.Classical.Beaufort b = new CryptSharp.Ciphers.Classical.Beaufort(tbUsedAlphabet.Text.ToCharArray());
                    b.Key = tbKey.Text;
                    tbClear.Text = b.Decrypt(tbCipher.Text);
                    break;

                default:
                    break;
            }
        }
    }
}
