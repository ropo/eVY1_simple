using System;
using System.Windows.Forms;
using jp.ropo;
using System.Collections.Generic;

namespace eVY_simple {
	public partial class frmMain : Form {
		eVY1driver eVY1 = new eVY1driver();
		List<int> devs = new List<int>();

		public frmMain() {
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e) {
			var count = Win32MidiOutPort.OutputCount;
			devs.Clear();
			for(int i=0; i<count; i++ ) {
				var caps = Win32MidiOutPort.GetDeviceInfo(i);
				if( caps != null && caps.deviceName=="eVY1 MIDI" ) {
					cmbOutDevice.Items.Add(string.Format("ID:{0} {1}", i+1, caps.deviceName ));
					devs.Add(i);
				}
			}
			if( cmbOutDevice.Items.Count > 0 )
				cmbOutDevice.SelectedIndex = 0;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			// F5 キーは[Play]
			if ((int)keyData == (int)Keys.F5) {
				cmdPlay.Checked = !cmdPlay.Checked;
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void cmdPlay_CheckedChanged(object sender, EventArgs e) {
			if( cmdPlay.Checked ) {
				eVY1.SendLylic(txtLylic.Text);
				eVY1.Play(txtMML.Text);
			}else{
				eVY1.Stop();
			}
		}

		private void cmbOutDevice_SelectedIndexChanged(object sender, EventArgs e) {
			cmdPlay.Enabled = eVY1.Open(devs[cmbOutDevice.SelectedIndex]);
		}
	}
}
