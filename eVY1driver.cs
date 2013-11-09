
using System.Collections.Generic;
using System;
using System.Threading;
using System.Text;
namespace jp.ropo {
	class eVY1driver {
		string[] phoneticSymbols = new string[]{
				"a", "i", "M", "e", "o",				// あいうえお0-4
				"k a", "k' i", "k M", "k e", "k o",		// かきくけこ5-9
				"s a", "S i", "s M", "s e", "s o",		// さしすえそ10-14
				"t a", "tS i", "ts M", "t e", "t o",	// たちつてと15-19
				"n a", "J i", "n M", "n e", "n o",		//なにぬねの20-24
				"h a", "C i", "p\\ M", "h e", "h o",	// はひふへほ25-29
				"m a", "m' i", "m M", "m e", "m o",		// まみむめも30-34
				"j a","i", "j M","e","j o",				//やいゆえよ35-39
				"4 a", "4' i", "4 M", "4 e", "4 o",		// らりるれろ40-44
				"w a","w o","N\\","","",				// わをん 45-49
				"g a", "g' i", "g M", "g e", "g o",		//がぎぐげご　50-54
				"dz a", "dZ i", "dz M", "dz e", "dz o",	//ざじずぜぞ55-59
				"d a", "dZ i", "dz M", "d e", "d o",	//だじづでど60-64
				"b a", "b' i", "b M", "b e", "b o",		//ばびぶべぼ	65-69
				"p a", "p' i", "p M", "p e", "p o"		//ぱぴぷぺぽ70-74
		};
		const string phonetics = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやいゆえよらりるれろわをん　　がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽ";

		Win32MidiOutPort dev = new Win32MidiOutPort();

		public bool Open(int id) {
			bool result = dev.Open(id);
			dev.Reset();
			return result;
		}

		public void SendLylic(string lylic) {
			if (dev == null)
				return;

			var ary = new List<byte>();
			ary.Add(0xf0);
			ary.Add(0x43);
			ary.Add(0x79);
			ary.Add(0x09);
			ary.Add(0x00);
			ary.Add(0x50);
			ary.Add(0x10);
			int index;
			int count = 0;
			for (int i = 0; i < lylic.Length; i++) {
				index = phonetics.IndexOf( lylic[i] );
				if( index >= 0 ) {
					if( count != 0 )	ary.Add(0x2c);
					var asciis = Encoding.ASCII.GetBytes(phoneticSymbols[index]);
					foreach( byte ascii in asciis )
						ary.Add( ascii );
					count++;
				}
			}
			ary.Add(0x00);
			ary.Add(0xf7);

			dev.SendLongMessage(ary.ToArray());
		}

		private Thread trdPlay;
		private string mml;

		public void Play(string mml) {
			if (dev == null)
				return;
			if (trdPlay != null && trdPlay.IsAlive )
				trdPlay.Abort();
			this.mml =mml;
			trdPlay = new Thread( new ThreadStart(playing) );
			trdPlay.IsBackground = true;
			trdPlay.Start();
		}

		public void Stop() {
			if (dev == null)
				return;
			if (trdPlay != null && trdPlay.IsAlive)
				trdPlay.Abort();
		}

		protected void playing() {
			byte note = 0x3c;
			foreach (var ascii in mml)
			{
				note = 0x3c;
				switch (ascii) {
					case 'C':	note+=0;	break;
					case 'D':	note+=2;	break;
					case 'E':	note+=4;	break;
					case 'F':	note+=5;	break;
					case 'G':	note+=7;	break;
					case 'A':	note+=9;	break;
					case 'B':	note+=11;	break;
					default:	note=0;		break;
				}
				if( note != 0 ) 
					SendMessage(new byte[] { 0x90, note, 0x40 });
				System.Threading.Thread.Sleep(300);
			}
			SendMessage(new byte[] { 0x90, note, 0x00 });
		}

		private void SendMessage(byte[] msg) {
			if (dev == null)
				return;
			dev.SendShortMessage(msg[0], msg[1],msg[2]);
		}
	}
}
