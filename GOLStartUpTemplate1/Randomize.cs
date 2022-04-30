using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate1
{
    public partial class Randomize : Form
    {
        public Randomize()
        {
            InitializeComponent();
        }
        public int getNumber()
        {
            return (int)numericUpDown1.Value;
        }
        public void setNumber(int number)
        {
            numericUpDown1.Value = number;
        }
    }
}
