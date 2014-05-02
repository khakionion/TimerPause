using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimerPause
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			//This sets the event handler
			timer1.Tick += new EventHandler(OnTimedEvent);
			//init the timer to something sane (this relies on the ValueChanged handler being fired)
			dateTimePicker1.Value = dateTimePicker1.Value.AddMinutes (1);
		}

		[DllImport("user32.dll", SetLastError = true)]
		static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			int minutes = TimerMinutes();
			SetAlarm(minutes);
			UpdateLabel();
		}

		private int TimerMinutes() {
			return dateTimePicker1.Value.Minute + dateTimePicker1.Value.Hour * 60;
		}

		private void SetAlarm(int minutesUntilAlarm) {
			//bail out early if there's nothing to set
			if (dateTimePicker1.Checked == false) {
				Console.WriteLine("Won't set a timer.");
				timer1.Stop();
				return;
			}

			Console.WriteLine("Setting timer for " + minutesUntilAlarm + " minutes");
			timer1.Interval = minutesUntilAlarm * 60 * 1000;
			timer1.Start();
		}

		private void UpdateLabel()
		{
			if (dateTimePicker1.Checked)
			{
				DateTime target = DateTime.Now.Add(dateTimePicker1.Value.TimeOfDay);
				label2.Text = "The sleep timer will fire at " + target;
			}
			else
			{
				label2.Text = "The sleep timer is currently not set.";
			}
		}
		private void OnTimedEvent(object source, EventArgs e)
		{
			Console.WriteLine("TIMER!!!");
			timer1.Stop();
			dateTimePicker1.Checked = false;
			UpdateLabel();
			Byte vkMediaPlayPause = 0xB3;
			const int keyUp = 0x2;
			const int keyDown = 0x0;
			keybd_event(vkMediaPlayPause, 0, keyUp, 0);
			keybd_event(vkMediaPlayPause, 0, keyDown, 0);
		}
	}
}
