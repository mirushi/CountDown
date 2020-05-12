using System;
using System.Windows.Forms;
using System.Media;
using System.Drawing;

namespace CountDown
{
    public partial class Form1 : Form
    {
        private int timeLeft;

        private bool mov;
        private int movX;
        private int movY;

        //Context menu cho form.
        ContextMenuStrip cm = new ContextMenuStrip();

        public Form1()
        {
            this.TopMost = true;

            //Chỉnh cái này để background của chúng ta đươc trong suốt.
            //this.TransparencyKey = Color.White;

            InitializeComponent();

            //Setup cho form của chúng ta không cần title.
            setupFormWithoutTitle();

            //Setup custom back color.
            setupCustomBackColor();

            //Setup context menu cho form.
            setupContextMenu();
        }

        private void setupContextMenu()
        {
            cm.Items.Add("Minimize");
            cm.Items.Add("Close");
            cm.Show();

            //Event handler cho Context Menu.
            cm.ItemClicked += new ToolStripItemClickedEventHandler(
                contextMenu_ItemClicked);
            this.ContextMenuStrip = cm;
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (item.Text.Equals("Minimize"))
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (item.Text.Equals("Close"))
            {
                this.Close();
            }
            cm.Hide();
        }

        private void setupFormWithoutTitle()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private void setupCustomBackColor()
        {
            //Setup for time count down control.
            this.txtTimer.BackColor = Color.FromArgb(82, 121, 219);

            //Setup for Start Button.
            this.btnStart.TabStop = false;
            this.btnStart.FlatStyle = FlatStyle.Flat;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtTimer.Text == "00:00")
            {
                MessageBox.Show("Please enter the time to start!", "Enter the Time", MessageBoxButtons.OK);
            }
            else
            {
                // Convert text to seconds as int for timer
                string[] totalSeconds = txtTimer.Text.Split(':');
                int minutes = Convert.ToInt32(totalSeconds[0]);
                int seconds = Convert.ToInt32(totalSeconds[1]);
                timeLeft = (minutes * 60) + seconds;

                // Lock Start and Clear buttons and text box
                btnStart.Enabled = false;
                btnClear.Enabled = false;
                txtTimer.ReadOnly = true;

                // Define Tick eventhandler and start timer
                //timer1.Tick += new EventHandler(Timer1_Tick);
                timer1.Start();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timeLeft = 0;
            btnStart.Enabled = true;
            btnClear.Enabled = true;
            txtTimer.ReadOnly = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtTimer.Text = "00:00";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
                // Display time remaining as mm:ss
                var timespan = TimeSpan.FromSeconds(timeLeft);
                txtTimer.Text = timespan.ToString(@"mm\:ss");
                //txtTimer.ForeColor = Color.DodgerBlue;
            }
            else
            {
                timer1.Stop();
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Thời gian đã hết, các bạn giải xong chưa?", "BHT", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.ToString() == "Hide")
            {
                this.Size = new Size(this.Size.Width, 153);
                button1.Text = "Show";
            }
            else
            {
                this.Size = new Size(this.Size.Width, 247);
                button1.Text = "Hide";
            }
        }

        private void txtTimer_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTimer_SelectionChanged(object sender, EventArgs e)
        {
            this.txtTimer.SelectionAlignment = HorizontalAlignment.Center;
        }

        //Xử lý move form dễ dàng.
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = true;
            movX = e.X;
            movY = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = false;
        }
    }
}