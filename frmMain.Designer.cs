namespace eVY_simple {
	partial class frmMain {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.txtLylic = new System.Windows.Forms.TextBox();
			this.cmbOutDevice = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdPlay = new System.Windows.Forms.CheckBox();
			this.txtMML = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtLylic
			// 
			this.txtLylic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLylic.Location = new System.Drawing.Point(12, 41);
			this.txtLylic.Multiline = true;
			this.txtLylic.Name = "txtLylic";
			this.txtLylic.Size = new System.Drawing.Size(444, 88);
			this.txtLylic.TabIndex = 0;
			this.txtLylic.Text = "かえるのうたがきこえてくるよ";
			// 
			// cmbOutDevice
			// 
			this.cmbOutDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbOutDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOutDevice.FormattingEnabled = true;
			this.cmbOutDevice.Location = new System.Drawing.Point(114, 14);
			this.cmbOutDevice.Name = "cmbOutDevice";
			this.cmbOutDevice.Size = new System.Drawing.Size(274, 20);
			this.cmbOutDevice.TabIndex = 2;
			this.cmbOutDevice.SelectedIndexChanged += new System.EventHandler(this.cmbOutDevice_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "MIDI-OUT Device";
			// 
			// cmdPlay
			// 
			this.cmdPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdPlay.Appearance = System.Windows.Forms.Appearance.Button;
			this.cmdPlay.AutoSize = true;
			this.cmdPlay.Location = new System.Drawing.Point(394, 12);
			this.cmdPlay.Name = "cmdPlay";
			this.cmdPlay.Size = new System.Drawing.Size(62, 22);
			this.cmdPlay.TabIndex = 4;
			this.cmdPlay.Text = "Play (&F5)";
			this.cmdPlay.UseVisualStyleBackColor = true;
			this.cmdPlay.CheckedChanged += new System.EventHandler(this.cmdPlay_CheckedChanged);
			// 
			// txtMML
			// 
			this.txtMML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMML.Location = new System.Drawing.Point(14, 135);
			this.txtMML.Multiline = true;
			this.txtMML.Name = "txtMML";
			this.txtMML.Size = new System.Drawing.Size(444, 73);
			this.txtMML.TabIndex = 5;
			this.txtMML.Text = "CDEFEDC EFGAGFE";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 221);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(356, 24);
			this.label2.TabIndex = 6;
			this.label2.Text = "ひらがなのみ\r\nMMLは「CDEFGAB」と半角スペースのみ。オクターブとか音長とか(ﾟ⊿ﾟ)ｼﾗﾈ";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(468, 258);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtMML);
			this.Controls.Add(this.cmdPlay);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbOutDevice);
			this.Controls.Add(this.txtLylic);
			this.MinimumSize = new System.Drawing.Size(484, 296);
			this.Name = "frmMain";
			this.Text = "eVY1 minimum";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtLylic;
		private System.Windows.Forms.ComboBox cmbOutDevice;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cmdPlay;
		private System.Windows.Forms.TextBox txtMML;
		private System.Windows.Forms.Label label2;
	}
}

