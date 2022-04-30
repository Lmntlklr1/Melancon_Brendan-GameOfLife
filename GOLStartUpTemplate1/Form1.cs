using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUpTemplate1
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[10,10];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        int seed;
        int cells = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            bool[,] sratchpad = new bool[10, 10];
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighborsFinite(x,y);
                    if (count < 2 && universe[x,y] == true)
                    {
                        sratchpad[x, y] = false;
                    }
                    if (count > 3 && universe[x,y] == true)
                    {
                        sratchpad[x, y] = false;
                    }
                    if ((count == 2 || count == 3) && universe[x,y] == true)
                    {
                        sratchpad[x,y] = true;
                    }
                    if (count == 3 && universe[x,y] == false)
                    {
                        sratchpad[x, y] = true;
                    }
                }
            }
            bool[,] temp = universe;
            universe = sratchpad;
            sratchpad = temp;

            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            LivingCellsClass();
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Play(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void Pause(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }
                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        private void NewClass(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void Next(object sender, EventArgs e)
        {
            if (timer.Enabled == false)
            {
                NextGeneration();
            }
        }

        private void RandomizeClass(object sender, EventArgs e)
        {
            //randomizes the seed variable up to 9999, still interactable
            Randomize dlg = new Randomize();
            Random rndSeed = new Random();
            dlg.setNumber(rndSeed.Next(0, 9999));
            dlg.ShowDialog();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                Random rnduniverse = new Random();
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        int rndnum = rnduniverse.Next(0, 2);
                        if (rndnum == 0)
                        {
                            universe[x, y] = true;
                        }
                        else
                        {
                            universe[x, y] = false;
                        }
                    }
                    seed = dlg.getNumber();
                }

                graphicsPanel1.Invalidate();
            }
        }

        private void SaveFileClass(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|PlainText|*.txt";
            dlg.FilterIndex = 2;
            dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);
                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                { 
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;
                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    { 
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x,y] == true)
                        {
                            currentRow = currentRow + "O";
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else if (universe[x,y] == false)
                        {
                            currentRow = currentRow + ".";
                        }
                    }
                    writer.WriteLine(currentRow);
                }
                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void NewDropDownFile(object sender, EventArgs e)
        {
            //Mimic the new on the tool bar to the one under the File Tab
            NewClass(sender, e);
        }

        private void SaveDropDownFile(object sender, EventArgs e)
        {
            //Mimic the save file on the tool bar to the one under the File tab
            SaveFileClass(sender, e);
        }

        //Does not copy the file to the form
        private void OpenFileClass(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|PlainText|*.txt";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);
                int maxWidth = 0;
                int maxHeight = 0;
                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();
                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!") == true)
                    {
                        continue;
                    }
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (row.StartsWith("!") == false)
                    {
                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }
                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    int y = 0;
                    // Read one row at a time.
                    string row = reader.ReadLine();
                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!") == true)
                    {
                        continue;
                    }
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if (row.Substring(xPos) == "O")
                        {
                            universe[xPos, y] = true;
                        }
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row.Substring(xPos) == ".")
                        {
                            universe[xPos,y] =false;
                        }
                        graphicsPanel1.Invalidate();
                    }
                    y++;
                }
                // Close the file.
                reader.Close();
            }
        }
        //does not work
        private void LivingCellsClass()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x,y] == true)
                    {
                        cells++;
                    }
                }
            }
            statusStrip1.Text = "Living Cells = " + cells.ToString();
        }

        private void cellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //the color of filling in the cells
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            cellColor = dlg.Color;
            graphicsPanel1.Invalidate();
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //the color of the background
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            graphicsPanel1.BackColor = dlg.Color;
            graphicsPanel1.Invalidate();
        }

        private void MillisecondChangeClass(object sender, EventArgs e)
        {
            //the change of Milliseconds, max is 10000
            TimeInterval dlg = new TimeInterval();
            dlg.setTime(timer.Interval);
            dlg.ShowDialog();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                timer.Interval = dlg.getTime();
            }
            graphicsPanel1.Invalidate();
        }

        private void GridSizeClass(object sender, EventArgs e)
        {
            //the change of the size of the grid, max size is 100 each
            GridSize dlg = new GridSize();
            dlg.setWidth(universe.GetLength(0));
            dlg.setHeight(universe.GetLength(1));
            dlg.ShowDialog();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                universe = new bool[dlg.getWidth(), dlg.getHeight()];
            }
            graphicsPanel1.Invalidate();
        }
    }
}
