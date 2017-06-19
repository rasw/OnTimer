using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace OnTimer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        long hours = 0;
        long minutes = 0;
        long seconds = 0;
        private EventLog eventLog1;

        private void Form1_Load(object sender, EventArgs e)
        {
            txtStartedAt.Text = DateTime.Now.ToString();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            //Label1.Text = ("Resolution: " + screenWidth + "x" + screenHeight);

            try
            {
                if(!Directory.Exists(@"C:\AkvaData"))
                {
                    Directory.CreateDirectory(@"C:\AkvaData");
                }

                File.AppendAllText(@"C:\AkvaData\OnTimer.txt","Started @ " + DateTime.Now.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }

            Left = (screenWidth - Width) - 5;
            Top = (screenHeight - Height) - 35;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                txtCurrentTime.Text = DateTime.Now.ToString();
                seconds++;

                if (seconds == 60)
                {
                    minutes++;
                    seconds = 0;

                    if (minutes == 60)
                    {
                        hours++;
                        minutes = 0;

                        try
                        {
                            File.AppendAllText(@"C:\AkvaData\OnTimer.txt", "On Hours: " + hours.ToString() + " @ " + DateTime.Now.ToString() + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex);
                        }
                    }
                }

                txtCounter.Text = "h:" + hours.ToString() + " m:" + minutes.ToString() + " s:" + seconds.ToString();
            }
            catch 
            {
                throw new System.Exception("Timer application screwed up big time.");
            }
        }

        private void WriteLog(Exception ex)
        {
            eventLog1 = new EventLog();

            if (!EventLog.SourceExists("OnTimer"))
                EventLog.CreateEventSource("OnTimer", "OnTimerLog");

            eventLog1.Source = "OnTimer";
            eventLog1.Log = "OnTimerLog";
            eventLog1.WriteEntry("Hours file write error. -> " + ex.Message);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Opacity = 1;
            tmrOpacity.Enabled = true;
            tmrOpacity.Start();
            Refresh();
        }

        private void tmrOpacity_Tick(object sender, EventArgs e)
        {
            Opacity = 0.4;
            Refresh();
            tmrOpacity.Stop();
            tmrOpacity.Enabled = false;
            Application.DoEvents();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA0) // WM_NCMOUSEMOVE
            {
                Opacity = 1;
                tmrOpacity.Stop();
                tmrOpacity.Start();
                Refresh();

            }
            else if (m.Msg == 0x2A2) // WM_NCMOUSELEAVE
            {
                
            }
            base.WndProc(ref m);
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Opacity = 1;
            tmrOpacity.Stop();
            tmrOpacity.Start();
            Refresh();
        }
    }
}
