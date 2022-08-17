using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SortHexColors
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Clear Previous Text
            richTextBox2.Clear();

            // Create a List from each color within the list
            List<Tuple<string, string, Color>> tiledata = new List<Tuple<string, string, Color>>();
            foreach (string line in richTextBox1.Lines)
            {
                string[] array10 = line.Split(new char[] { '	' });
                string tile = array10[0];
                string paint = array10[1];
                string color = array10[2];

                tiledata.Add(new Tuple<string, string, Color>(tile, paint, System.Drawing.ColorTranslator.FromHtml("#" + color)));
            }

            // Sort colors based on HUE
            // https://stackoverflow.com/a/62203405/8667430
            if (radioButton1.Checked)
            {
                // Create An IOrderEnumerable List
                var tiledatalist = tiledata.OrderBy(color => color.Item3.GetHue()).ThenBy(o => o.Item3.R * 3 + o.Item3.G * 2 + o.Item3.B * 1);

                // Expand Each Item Of The List
                foreach (var tiledatainfo in tiledatalist)
                {
                    // Define info from the list item
                    string tile = tiledatainfo.Item1;
                    string paint = tiledatainfo.Item2;
                    Color color = tiledatainfo.Item3;

                    // Output the data
                    richTextBox2.AppendText(tile + "	" + paint + "	" + ColorConverterExtensions.ToHexString(color).Replace("#", "") + Environment.NewLine);
                }

                // End Sub
                return;
            }

            // Sort colors based on HSV
            // https://stackoverflow.com/a/72474150/8667430
            if (radioButton2.Checked)
            {
                ColorRampComparer CRC = new ColorRampComparer
                {
                    Repetitions = (int)numericUpDown1.Value,
                    Invert = false
                };
                tiledata.Sort(CRC);
            }

            // Sort colors based on inverted HSV
            // https://stackoverflow.com/a/72474150/8667430
            if (radioButton3.Checked)
            {
                ColorRampComparer CRC = new ColorRampComparer
                {
                    Repetitions = (int)numericUpDown1.Value,
                    Invert = true
                };
                tiledata.Sort(CRC);
            }

            // Expand Each Item Of The List
            foreach (var tiledatainfo in tiledata)
            {
                // Define info from the list item
                string tile = tiledatainfo.Item1;
                string paint = tiledatainfo.Item2;
                Color color = tiledatainfo.Item3;

                // Output the data
                richTextBox2.AppendText(tile + "	" + paint + "	" + ColorConverterExtensions.ToHexString(color).Replace("#", "") + Environment.NewLine);
            }
        }

        // Copy Input Data
        private void Button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                Clipboard.SetText(richTextBox1.Text);
                MessageBox.Show("Input data saved to clipboard.");
            }
            else
            {
                MessageBox.Show("Input data is empty!");
            }
        }

        // Copy Output Data
        private void Button3_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "")
            {
                Clipboard.SetText(richTextBox2.Text);
                MessageBox.Show("Output data saved to clipboard.");
            }
            else
            {
                MessageBox.Show("Output data is empty!");
            }
        }
    }
}
