﻿using System;
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
    public partial class TimeInterval : Form
    {
        public TimeInterval()
        {
            InitializeComponent();
        }
        public int getTime()
        {
            return (int)numericUpDown1.Value;
        }
        public void setTime(int time)
        {
            numericUpDown1.Value = time;
        }
    }
}
