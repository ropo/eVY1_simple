using System;
using System.Runtime.InteropServices;
using System.Threading;


namespace jp.ropo
{
	public class Win32MidiOutPort : IDisposable
	{
		private NativeMethods.MidiOutProc midiOutProc;
		private IntPtr handle = IntPtr.Zero;

		public Win32MidiOutPort()
		{
			midiOutProc = new NativeMethods.MidiOutProc(MidiProc);
			handle = IntPtr.Zero;
		}

		~Win32MidiOutPort()
		{
			Dispose();
		}

		public void Dispose() {
			Close();
		}
		public void Reset() {
			NativeMethods.midiOutReset(handle);
		}

		public static int OutputCount {
			get { return NativeMethods.midiOutGetNumDevs(); }
		}

		public bool Close() {
			if (handle == IntPtr.Zero)
				return true;
			NativeMethods.midiOutReset(handle);
			bool result = NativeMethods.midiOutClose(handle) == NativeMethods.MMSYSERR_NOERROR;
			handle = IntPtr.Zero;
			return result;
		}

		public bool Open(int id) {
			Close();
			return NativeMethods.midiOutOpen(out handle, id, midiOutProc, IntPtr.Zero, NativeMethods.CALLBACK_FUNCTION) == NativeMethods.MMSYSERR_NOERROR;
		}
		
		uint MIDIMSG( int stat, int data1, int data2 )
		{
			return (uint)( stat | (data1<<8) | (data2<<16));
		}

		public void SendShortMessage(byte status, byte data1, byte data2)
		{
			if( handle == IntPtr.Zero )
				return;
			NativeMethods.midiOutShortMsg( handle, MIDIMSG(status,data1,data2) );
		}
		public void SendLongMessage( byte[] msg )
		{
			uint hdrSize = (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIHDR));
			var midiHdr = new NativeMethods.MIDIHDR();
			byte[] hdrReserved = new byte[8];
			GCHandle alchandle = GCHandle.Alloc(msg, GCHandleType.Pinned);
			GCHandle revHandle = GCHandle.Alloc(hdrReserved, GCHandleType.Pinned);

			midiHdr.lpData = alchandle.AddrOfPinnedObject();
			midiHdr.dwBufferLength = (uint)msg.Length;
			midiHdr.dwFlags = 0;

			NativeMethods.midiOutPrepareHeader(handle, ref midiHdr, hdrSize);

			while ((midiHdr.dwFlags & NativeMethods.MidiHdrFlag.MHDR_PREPARED) != NativeMethods.MidiHdrFlag.MHDR_PREPARED) {
				Thread.Sleep(1);
			}
			NativeMethods.midiOutLongMsg(handle, ref midiHdr, hdrSize);
			while ((midiHdr.dwFlags & NativeMethods.MidiHdrFlag.MHDR_DONE) != NativeMethods.MidiHdrFlag.MHDR_DONE) {
				Thread.Sleep(1);
			}
			NativeMethods.midiOutUnprepareHeader(handle, ref midiHdr, hdrSize);

			alchandle.Free();
			revHandle.Free();
		}

		private void MidiProc(IntPtr hMidiOut, int wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2) {
			switch (wMsg) {
				case NativeMethods.MM_MIM_OPEN:
					break;
				case NativeMethods.MM_MIM_CLOSE:
					break;
				case NativeMethods.MM_MIM_DATA:
//					OnMimData(dwParam1, dwParam2);
					break;
				case NativeMethods.MM_MIM_LONGDATA:
					break;
				case NativeMethods.MM_MIM_LONGERROR:
					break;
				case NativeMethods.MM_MIM_ERROR:
					break;
				case NativeMethods.MM_MIM_MOREDATA:
					break;
			}
		}
		public class DeviceCaps {
			public string deviceName;
		}
		static public DeviceCaps GetDeviceInfo(int id) {
			var devCaps = new NativeMethods.MIDIOUTCAPS();
			if (NativeMethods.midiOutGetDevCaps((uint)id, ref devCaps, (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIOUTCAPS))) != NativeMethods.MMSYSERR_NOERROR)
				return null;
			var caps = new DeviceCaps();
			caps.deviceName = devCaps.szPname;
			return caps;
		}
	}

    public class Win32MidiInPort : IDisposable
    {
        private NativeMethods.MidiInProc midiInProc;
        private IntPtr handle = IntPtr.Zero;

        public Win32MidiInPort()
        {
            midiInProc = new NativeMethods.MidiInProc(MidiProc);
            handle = IntPtr.Zero;
        }

		~Win32MidiInPort()
		{
			Dispose();
		}

        public void Dispose()
        {
            Close();
        }

        public static int InputCount
        {
            get { return NativeMethods.midiInGetNumDevs(); }
        }

        public bool Close()
        {
            if (handle == IntPtr.Zero)
                return true;
            bool result = NativeMethods.midiInClose(handle) == NativeMethods.MMSYSERR_NOERROR;
            handle = IntPtr.Zero;
            return result;
        }
        public bool Open(int id)
        {
            return NativeMethods.midiInOpen(out handle, id, midiInProc, IntPtr.Zero, NativeMethods.CALLBACK_FUNCTION) == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Start()
        {
            return NativeMethods.midiInStart(handle) == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Stop()
        {
            return NativeMethods.midiInStop(handle) == NativeMethods.MMSYSERR_NOERROR;
        }

        private void MidiProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2)
        {
            switch (wMsg)
            {
                case NativeMethods.MM_MIM_OPEN:
                    break;
                case NativeMethods.MM_MIM_CLOSE:
                    break;
                case NativeMethods.MM_MIM_DATA:
                    OnMimData(dwParam1, dwParam2);
                    break;
                case NativeMethods.MM_MIM_LONGDATA:
                    break;
                case NativeMethods.MM_MIM_LONGERROR:
                    break;
                case NativeMethods.MM_MIM_ERROR:
                    break;
                case NativeMethods.MM_MIM_MOREDATA:
                    break;
            }
        }

        public delegate void dlgOnNoteChange(byte ch, byte note, byte velocity, bool isOn);
        public event dlgOnNoteChange OnNoteChange = null;

        private void OnMimData(uint dwData, uint dwTimeStamp)
        {
            if (OnNoteChange == null)
                return;
            byte st, dt1, dt2;

            st = (byte)((dwData) & 0xFF);	// status
            dt1 = (byte)((dwData >> 8) & 0xFF);	// data1
            dt2 = (byte)((dwData >> 16) & 0xFF);	// data2

            if (st < 0xF0)
            {
                byte ch = (byte)(st & 0xF);
                switch (st >> 4)
                {
                    case 0x8:// note off
                        OnNoteChange(ch, dt1, 0, false);
                        break;
                    case 0x9:// note on
                        OnNoteChange(ch, dt1, dt2, true);
                        break;
                    case 0xa:// polyphonic
                        break;
                    case 0xb:// control change
                        break;
                    case 0xc:// program change
                        break;
                    case 0xd:// channel pressure
                        break;
                    case 0xe:// pitch bend change
                        break;
                }
            }
        }

        public class DeviceCaps
        {
            public string deviceName;
        }
        static public DeviceCaps GetDeviceInfo(int id)
        {
            var devCaps = new NativeMethods.MIDIINCAPS();
            if (NativeMethods.midiInGetDevCaps((uint)id, ref devCaps, (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIINCAPS))) != NativeMethods.MMSYSERR_NOERROR)
                return null;
            var caps = new DeviceCaps();
            caps.deviceName = devCaps.szPname;
            return caps;
        }
    }

    internal static class NativeMethods
    {
        internal const int MMSYSERR_NOERROR = 0;

        internal const int CALLBACK_FUNCTION = 0x30000;
        internal const int MM_MIM_OPEN = 0x3C1;
        internal const int MM_MIM_CLOSE = 0x3C2;
        internal const int MM_MIM_DATA = 0x3C3;
        internal const int MM_MIM_LONGDATA = 0x3C4;
        internal const int MM_MIM_ERROR = 0x3C5;
        internal const int MM_MIM_LONGERROR = 0x3C6;
        internal const int MM_MIM_MOREDATA = 0x3CC;

		internal delegate void MidiInProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2);
		internal delegate void MidiOutProc(IntPtr hMidiOut, int wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2);

		[DllImport("winmm.dll")]
		internal static extern int midiOutGetNumDevs();
		[DllImport("winmm.dll")]
		internal static extern int midiOutClose(IntPtr hMidiOut);
		[DllImport("winmm.dll")]
		internal static extern int midiOutOpen(out IntPtr lphMidiIn, int uDeviceID, MidiOutProc dwCallback, IntPtr dwCallbackInstance, int dwFlags);
		[DllImport("winmm.dll")]
		internal static extern int midiInGetNumDevs();
		[DllImport("winmm.dll")]
		internal static extern int midiInClose(IntPtr hMidiIn);
		[DllImport("winmm.dll")]
		internal static extern int midiInOpen(out IntPtr lphMidiIn, int uDeviceID, MidiInProc dwCallback, IntPtr dwCallbackInstance, int dwFlags);
		[DllImport("winmm.dll")]
		internal static extern int midiInStart(IntPtr hMidiIn);
		[DllImport("winmm.dll")]
		internal static extern int midiInStop(IntPtr hMidiIn);

		[StructLayout(LayoutKind.Sequential)]
		internal struct MIDIHDR {
			public IntPtr lpData;
			public uint dwBufferLength;
			public uint dwBytesRecorded;
			public uint dwUser;
			public uint dwFlags;
			public IntPtr lpNext;
			public IntPtr reserved;
			public uint dwOffset;
			public IntPtr dwReserved;
		}
		internal class MidiHdrFlag {
			/// <summary>
			/// バッファの使用が完了しました。
			/// </summary>
			public const int MHDR_DONE = 1;
			/// <summary>
			/// バッファの準備が完了しました。
			/// </summary>
			public const int MHDR_PREPARED = 2;
			public const int MHDR_INQUEUE = 4;
			public const int MHDR_ISSTRM = 8;
		}
		/*
		typedef struct midihdr_tag {
//					LPSTR   lpData;                  // MIDIデータアドレス
					DWORD   dwBufferLength;          // バッファサイズ
					DWORD   dwBytesRecorded;         // 実際のデータサイズ
					DWORD_PTR dwUser;                // カスタムユーザデータ
					DWORD   dwFlags;                 // フラグ
					struct midihdr_tag FAR *lpNext;  // 予約(NULL)
					DWORD_PTR reserved;              // 予約(0)
					DWORD   dwOffset;                // バッファのオフセット
					DWORD_PTR dwReserved[8];         // 予約
				} MIDIHDR, *PMIDIHDR, NEAR *NPMIDIHDR, FAR *LPMIDIHDR;
				*/

		[DllImport("winmm.dll")]
		internal static extern uint midiOutPrepareHeader(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize);
		[DllImport("winmm.dll")]
		internal static extern uint midiOutUnprepareHeader(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize);
		[DllImport("winmm.dll")]
		internal static extern uint midiOutShortMsg(IntPtr hMidOut, uint msg);
		[DllImport("winmm.dll")]
		internal static extern uint midiOutLongMsg(IntPtr hMidiOut, ref MIDIHDR lpMidiOutHdr, uint uSize);
		[DllImport("winmm.dll")]
		internal static extern uint midiOutReset(IntPtr hMidiOut);


		[StructLayout(LayoutKind.Sequential)]
        internal struct MIDIINCAPS
        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwSupport;
        }
		[StructLayout(LayoutKind.Sequential)]
		internal struct MIDIOUTCAPS {
			public ushort wMid;
			public ushort wPid;
			public uint vDriverVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szPname;
			public ushort wTechnology;
			public ushort wVoices;
			public ushort wNotes;
			public ushort wChannelMask;
			public uint dwSupport;
		}

		[DllImport("winmm.dll")]
		internal static extern uint midiInGetDevCaps(uint uDeviceID, ref MIDIINCAPS lpCaps, uint uSize);
		[DllImport("winmm.dll")]
		internal static extern uint midiOutGetDevCaps(uint uDeviceID, ref MIDIOUTCAPS lpCaps, uint uSize);
	}
}
