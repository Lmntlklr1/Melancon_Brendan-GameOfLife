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
    public partial class GridSize : Form
    {
        public GridSize()
        {
            InitializeComponent();
        }
        public int getWidth()
        {
            return (int)numericUpDown1.Value;
        }
        public int getHeight()
        {
            return (int)numericUpDown2.Value;
        }
        public void setWidth(int width)
        {
            numericUpDown1.Value = width;
        }
        public void setHeight(int height)
        {
            numericUpDown2.Value = height;
        }
    }
}
