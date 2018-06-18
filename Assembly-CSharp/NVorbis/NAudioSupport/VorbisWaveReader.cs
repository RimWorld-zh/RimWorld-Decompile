using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x020009E1 RID: 2529
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x060038A7 RID: 14503 RVA: 0x001E40A9 File Offset: 0x001E24A9
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x001E40DF File Offset: 0x001E24DF
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x001E4116 File Offset: 0x001E2516
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x001E4148 File Offset: 0x001E2548
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060038AB RID: 14507 RVA: 0x001E4164 File Offset: 0x001E2564
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x001E41B4 File Offset: 0x001E25B4
		// (set) Token: 0x060038AD RID: 14509 RVA: 0x001E4204 File Offset: 0x001E2604
		public override long Position
		{
			get
			{
				return (long)(this._reader.DecodedTime.TotalMilliseconds * (double)this._reader.SampleRate * (double)this._reader.Channels * 4.0);
			}
			set
			{
				if (value < 0L || value > this.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._reader.DecodedTime = TimeSpan.FromSeconds((double)value / (double)this._reader.SampleRate / (double)this._reader.Channels / 4.0);
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x001E4268 File Offset: 0x001E2668
		public override int Read(byte[] buffer, int offset, int count)
		{
			count /= 4;
			count -= count % this._reader.Channels;
			float[] array;
			if ((array = VorbisWaveReader._conversionBuffer) == null)
			{
				array = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			float[] array2 = array;
			if (array2.Length < count)
			{
				array2 = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			int num = this.Read(array2, 0, count) * 4;
			Buffer.BlockCopy(array2, 0, buffer, offset, num);
			return num;
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x001E42DC File Offset: 0x001E26DC
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x001E4300 File Offset: 0x001E2700
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x001E4320 File Offset: 0x001E2720
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x001E4330 File Offset: 0x001E2730
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x001E4350 File Offset: 0x001E2750
		// (set) Token: 0x060038B4 RID: 14516 RVA: 0x001E436A File Offset: 0x001E276A
		public int? NextStreamIndex { get; set; }

		// Token: 0x060038B5 RID: 14517 RVA: 0x001E4374 File Offset: 0x001E2774
		public bool GetNextStreamIndex()
		{
			if (this.NextStreamIndex == null)
			{
				int streamCount = this._reader.StreamCount;
				if (this._reader.FindNextStream())
				{
					this.NextStreamIndex = new int?(streamCount);
					return true;
				}
			}
			return false;
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x001E43D0 File Offset: 0x001E27D0
		// (set) Token: 0x060038B7 RID: 14519 RVA: 0x001E43F0 File Offset: 0x001E27F0
		public int CurrentStream
		{
			get
			{
				return this._reader.StreamIndex;
			}
			set
			{
				if (!this._reader.SwitchStreams(value))
				{
					throw new InvalidDataException("The selected stream is not a valid Vorbis stream!");
				}
				if (this.NextStreamIndex != null && value == this.NextStreamIndex.Value)
				{
					this.NextStreamIndex = null;
				}
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x001E4454 File Offset: 0x001E2854
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060038B9 RID: 14521 RVA: 0x001E4474 File Offset: 0x001E2874
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x001E4494 File Offset: 0x001E2894
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x001E44B4 File Offset: 0x001E28B4
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060038BC RID: 14524 RVA: 0x001E44D4 File Offset: 0x001E28D4
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060038BD RID: 14525 RVA: 0x001E44F4 File Offset: 0x001E28F4
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060038BE RID: 14526 RVA: 0x001E4514 File Offset: 0x001E2914
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Token: 0x0400242E RID: 9262
		private VorbisReader _reader;

		// Token: 0x0400242F RID: 9263
		private WaveFormat _waveFormat;

		// Token: 0x04002430 RID: 9264
		[ThreadStatic]
		private static float[] _conversionBuffer = null;
	}
}
