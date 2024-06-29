using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Fractals.FractalPlotters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
namespace Fractals
{
    public partial class FractalForm : Form
    {
        public FractalForm()
        {
            InitializeComponent();
        }
        decimal scaleChange;
        bool fullscreen;
        IFractalPlotter plotter;
        ParamsForFractal parameters;
        delegate bool Funct();
        Funct SaveParameters;
        Stopwatch stopwatch = new Stopwatch();
        private void Graphics1_Load(object sender, EventArgs e)
        {
            InitializeStuff();
            DrawSomething();
        }
        private void InitializeStuff()
        {
            SizeChanged += handleResize;
            Bitmap bitmap = new Bitmap(drawingBox1.Width, drawingBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            drawingBox1.Image = bitmap;
            comboBoxPalette.SelectedIndex = 0;
            comboBoxPaletteBrot.SelectedIndex = 0;
            comboBoxFuncJul.SelectedIndex = 0;
            comboBoxFuncBrot.SelectedIndex = 0;

            scaleChange = numericUpDownScale.Value;
            fullscreen = false;
            plotter = new MultijuliaPlotter();
            SaveParameters = MultijuliaParamSave;

            parameters = new ParamsForFractal();
            parameters.screenCenter.X = drawingBox1.Width / 2;
            parameters.screenCenter.Y = drawingBox1.Height / 2;
            parameters.offset.X = 0;
            parameters.offset.Y = 0;
            parameters.scaleFactor = numericUpDownZoom.Value;
            parameters.palette = comboBoxPalette.SelectedIndex;
            parameters.powerOfZ = numericUpDownJuliaP.Value;
            parameters.appliedFunc = comboBoxFuncJul.SelectedIndex;
            parameters.escapeNumber = numericUpDownCircleJulia.Value;
            parameters.maxIterations = (int)numericUpDownJuliaIt.Value;
            parameters.cx = numericUpDownCx.Value;
            parameters.cy = numericUpDownCy.Value;
            parameters.amountOfPoints = Convert.ToInt32(numericUpDownIFSIterations.Value);

            decimal[] a = { 0.0M, 0.85M, 0.20M, -0.15M };
            decimal[] b = { 0.0M, 0.04M, -0.26M, 0.28M };
            decimal[] c = { 0.0M, -0.04M, 0.23M, 0.26M };
            decimal[] d = { 0.16M, 0.85M, 0.22M, 0.24M };
            decimal[] e = { 0.0M, 0.0M, 0.0M, 0.0M, };
            decimal[] f = { 0.0M, 1.6M, 1.6M, 0.44M };
            decimal[] p = { 0.01M, 0.85M, 0.07M, 0.07M };
            SetTableValues(a, b, c, d, e, f, p);
        }
        private void DrawSomething()
        {
            Bitmap bitmap = (Bitmap)drawingBox1.Image;
            Graphics drawingSpace = Graphics.FromImage(bitmap);
            stopwatch.Reset();
            stopwatch.Start();
            plotter.DrawFractalMultithread(drawingSpace, parameters); //There is also an obsolete one-thread version
            stopwatch.Stop();
            labelTimeSpent.Text = stopwatch.ElapsedMilliseconds.ToString()+" ms";

            drawingSpace.Dispose();
            drawingBox1.Invalidate();
        }
        private void DrawScene()
        {
            (parameters.offset.X, parameters.offset.Y) = CoordinateShenanigans.GetDistanceFromZero(new PointD(numericUpDownCenterX.Value, numericUpDownCenterY.Value), parameters.scaleFactor);
            if (numericUpDownZoom.Value < 0.00001M)
                parameters.scaleFactor = 0.1M;
            else
                parameters.scaleFactor = numericUpDownZoom.Value;
            if(SaveParameters())
                DrawSomething();
            UpdateUnitSize();
        }
        private void SaveImage(Bitmap pic)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PNG Image|*.png";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                pic.Save(saveFileDialog1.FileName);
            }
        }
        private void SaveImageFullscreen()
        {
            Bitmap bitmap = new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Point tmp = parameters.screenCenter;
            parameters.screenCenter.X = bitmap.Width / 2;
            parameters.screenCenter.Y = bitmap.Height / 2;
            Graphics graphics = Graphics.FromImage(bitmap);
            plotter.DrawFractal(graphics, parameters);
           
            SaveImage(bitmap);
            parameters.screenCenter = tmp;
            graphics.Dispose();
            bitmap.Dispose();
        }
        private void HandleTabChange(int index)
        {
            parameters.offset.X = 0;
            parameters.offset.Y = 0;
            numericUpDownCenterX.Value = 0;
            numericUpDownCenterY.Value = 0;
            switch (index)
            {
                //Scaling will be reset when moving between tabs, not ideal
                case 0:
                    plotter = new MultijuliaPlotter();
                    SaveParameters = MultijuliaParamSave;
                    parameters.scaleFactor = 300;
                    UpdateUnitSize();
                    DrawScene();
                    break;
                case 1:
                    plotter = new MultibrotPlotter();
                    SaveParameters = MultibrotParamSave;
                    parameters.scaleFactor = 300;
                    UpdateUnitSize();
                    DrawScene();
                    break;
                case 2:
                    plotter = new IFSPlotter(GetIFSValues());
                    SaveParameters = IFSParamSave;
                    parameters.scaleFactor = 50;
                    UpdateUnitSize();
                    if (!ValidateTable())
                    {
                        MessageBox.Show("Invalid data in IFS table", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    DrawScene();
                    break;
                default:
                    MessageBox.Show("Oh no");
                    break;
            }
        }
        private void UpscaleButton_Click(object sender, EventArgs e)
        {
            scaleChange = numericUpDownScale.Value;
            parameters.scaleFactor *= scaleChange;
            parameters.offset.X += parameters.offset.X * (scaleChange - 1.0M);
            parameters.offset.Y += parameters.offset.Y * (scaleChange - 1.0M);
            UpdateUnitSize();
            DrawScene();
        }
        private void DownscaleButton_Click(object sender, EventArgs e)
        {
            scaleChange = numericUpDownScale.Value;
            if (parameters.scaleFactor / scaleChange < 5.0M)
                parameters.scaleFactor = 5.0M;
            else
            {
                parameters.scaleFactor /= scaleChange;
            }
            parameters.offset.X -= parameters.offset.X - parameters.offset.X / scaleChange;
            parameters.offset.Y -= parameters.offset.Y - parameters.offset.Y / scaleChange;
            UpdateUnitSize();
            DrawScene();
        }
        private void saveImageButton_Click(object sender, EventArgs e)
        {
            SaveImage((Bitmap)drawingBox1.Image);
        }
        private void FractalForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Should add list of hot keys, maybe later
            if (e.KeyCode == Keys.F11)
            {
                if (!fullscreen)
                {
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    this.Bounds = Screen.PrimaryScreen.Bounds;
                    fullscreen = true;
                }
                else
                {
                    this.Bounds = new Rectangle(0, 0, 1200, 800);
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    fullscreen = false;
                }
                DrawScene();
            }
            if (e.Alt && e.KeyCode == Keys.Left)
            {
                parameters. offset.X += 10;
                DrawScene();
            }
            if (e.Alt && e.KeyCode == Keys.Up)
            {
                parameters.offset.Y += 10;
                DrawScene();
            }
            if (e.Alt && e.KeyCode == Keys.Right)
            {
                parameters.offset.X += -10;
                DrawScene();
            }
            if (e.Alt && e.KeyCode == Keys.Down)
            {
                parameters.offset.Y += -10;
                DrawScene();
            }
            if (e.Alt && e.KeyCode == Keys.Add)
            {
                UpscaleButton.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.Subtract)
            {
                DownscaleButton.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.D)
            {
                redrawButton.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.S)
            {
                saveImageButton.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.F)
            {
                SaveImageFullscreen();
            }
        }
        private void handleResize(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(drawingBox1.Width, drawingBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            drawingBox1.Image = bitmap;
            parameters.screenCenter.X = bitmap.Width / 2;
            parameters.screenCenter.Y = bitmap.Height / 2;
            DrawScene();
        }
        private void drawingBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Point clickPos = e.Location;
                parameters.offset.X -= (clickPos.X - parameters.screenCenter.X);
                parameters.offset.Y -= (clickPos.Y - parameters.screenCenter.Y);
                showOffset();
                DrawScene();
            }
        }
        private void redrawButton_Click(object sender, EventArgs e)
        {
            DrawScene();
        }
        private void buttonIFSBack_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;
            dialog.ShowHelp = true;
            dialog.Color = buttonIFSBack.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                buttonIFSBack.BackColor = dialog.Color;
                DrawScene();
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleTabChange(tabControl1.SelectedIndex);
        }
        private void showOffset()
        {
            (numericUpDownCenterX.Value, numericUpDownCenterY.Value) =
                CoordinateShenanigans.GetWorldCoords(parameters.screenCenter, parameters.screenCenter, parameters.offset, parameters.scaleFactor);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                ColorDialog dialog = new ColorDialog();
                dialog.AllowFullOpen = true;
                dialog.ShowHelp = true;
                dialog.Color = Color.Red;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    senderGrid.Rows[e.RowIndex].Cells[0].Style.SelectionBackColor = dialog.Color;
                    senderGrid.Rows[e.RowIndex].Cells[0].Style.BackColor = dialog.Color;
                    senderGrid.Rows[e.RowIndex].Cells[0].Style.SelectionForeColor = dialog.Color;
                    senderGrid.Rows[e.RowIndex].Cells[0].Style.ForeColor = dialog.Color;
                    DrawScene();
                }
            }
        }
        private bool ValidateTable()
        {
            if (dataGridView1.Rows.Count == 1)
            {

                return false;
            }
            bool isTableCorrect = true;
            double parseCheck;        //Needed for '.' as decimal and ',' as digit separator
            System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 1; j < dataGridView1.Rows[i].Cells.Count - 1; j++)
                {
                    dataGridView1.Rows[i].Cells[j].ErrorText = "";
                    dataGridView1.Rows[i].Cells[j].ErrorText = null;
                    if (dataGridView1.Rows[i].Cells[j].Value == null || (dataGridView1.Rows[i].Cells[j].Value) == "")
                    {
                        isTableCorrect = false;
                        dataGridView1.Rows[i].Cells[j].ErrorText = "No value";
                        continue;
                    }
                    string val = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    if (!double.TryParse(val, System.Globalization.NumberStyles.Number, cult, out parseCheck))
                    {
                        isTableCorrect = false;
                        dataGridView1.Rows[i].Cells[j].ErrorText = "Invalid value";
                        continue;
                    }
                }
            }
            decimal sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[7].Value == null || (dataGridView1.Rows[i].Cells[7].Value) == "")
                {
                    isTableCorrect = false;
                    dataGridView1.Rows[i].Cells[7].ErrorText = "No value";
                    continue;
                }
                dataGridView1.Rows[i].Cells[7].ErrorText = "";
                string val = dataGridView1.Rows[i].Cells[7].Value.ToString();
                if (!double.TryParse(val, System.Globalization.NumberStyles.Number, cult, out parseCheck))
                {
                    isTableCorrect = false;
                    dataGridView1.Rows[i].Cells[7].ErrorText = "Invalid value";
                    continue;
                }
                else if (parseCheck < 0 || parseCheck > 1.0)
                {
                    isTableCorrect = false;
                    dataGridView1.Rows[i].Cells[7].ErrorText = "Invalid value";
                    continue;
                }
                sum += (decimal)parseCheck;
            }
            if (sum > 1.001M || sum < 0.999M)
            {
                isTableCorrect = false;
                dataGridView1.Rows[0].Cells[7].ErrorText = "Invalid values, sum of probabilities should be 1";
            }
            return isTableCorrect;
        }
        private void SetTableValues(decimal[] a, decimal[] b, decimal[] c, decimal[] d, decimal[] e, decimal[] f, decimal[] p)
        {
            for (int i = 0; i < a.Length; i++)
                dataGridView1.Rows.Add();
            for (int i = 0; i < a.Length; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = a[i];
            }
            for (int i = 0; i < b.Length; i++)
            {
                dataGridView1.Rows[i].Cells[2].Value = b[i];
            }
            for (int i = 0; i < c.Length; i++)
            {
                dataGridView1.Rows[i].Cells[3].Value = c[i];
            }
            for (int i = 0; i < d.Length; i++)
            {
                dataGridView1.Rows[i].Cells[4].Value = d[i];
            }
            for (int i = 0; i < e.Length; i++)
            {
                dataGridView1.Rows[i].Cells[5].Value = e[i];
            }
            for (int i = 0; i < f.Length; i++)
            {
                dataGridView1.Rows[i].Cells[6].Value = f[i];
            }
            for (int i = 0; i < p.Length; i++)
            {
                dataGridView1.Rows[i].Cells[7].Value = p[i];
            }
            for (int i = 0; i < p.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.SelectionBackColor = Color.Red;
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
                dataGridView1.Rows[i].Cells[0].Style.SelectionForeColor = Color.Red;
                dataGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Red;
            }
        }
        private IFS GetIFSValues()
        {
            List<decimal> a = new List<decimal>();
            List<decimal> b = new List<decimal>();
            List<decimal> c = new List<decimal>();
            List<decimal> d = new List<decimal>();
            List<decimal> e = new List<decimal>();
            List<decimal> f = new List<decimal>();
            List<decimal> p = new List<decimal>();
            List<Color> colors = new List<Color>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                a.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                b.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                c.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                d.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                e.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                f.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                p.Add(Convert.ToDecimal(dataGridView1.Rows[i].Cells[7].Value.ToString()));
            }
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                colors.Add(dataGridView1.Rows[i].Cells[0].Style.BackColor);
            }
            return new IFS(a, b, c, d, e, f, p, colors, buttonIFSBack.BackColor);
        }
        private void UpdateUnitSize()
        {
            numericUpDownZoom.Value = parameters.scaleFactor;
            numericUpDownZoom.Text = parameters.scaleFactor.ToString();
        }
        private bool MultijuliaParamSave()
        {
            parameters.maxIterations = Convert.ToInt32(numericUpDownJuliaIt.Value);
            parameters.cx = numericUpDownCx.Value;
            parameters.cy = numericUpDownCy.Value;
            parameters.powerOfZ = numericUpDownJuliaP.Value;
            parameters.palette = comboBoxPalette.SelectedIndex;
            parameters.appliedFunc = comboBoxFuncJul.SelectedIndex;
            parameters.escapeNumber = numericUpDownCircleJulia.Value;
            return true;
        }
        private bool MultibrotParamSave()
        {
            parameters.maxIterations = Convert.ToInt32(numericUpDownBrotIt.Value);
            parameters.powerOfZ = numericUpDownBrotP.Value;
            parameters.palette = comboBoxPaletteBrot.SelectedIndex;
            parameters.appliedFunc = comboBoxFuncBrot.SelectedIndex;
            parameters.escapeNumber = numericUpDownCircleBrot.Value;
            return true;
        }
        private bool IFSParamSave()
        {
            parameters.amountOfPoints = Convert.ToInt32(numericUpDownIFSIterations.Value);
            if (!ValidateTable())
            {
                MessageBox.Show("Invalid data in IFS table", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            plotter = new IFSPlotter(GetIFSValues());
            return true;
        }
    }
}