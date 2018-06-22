using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x020009DD RID: 2525
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x060038A1 RID: 14497 RVA: 0x001E42E9 File Offset: 0x001E26E9
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x001E431F File Offset: 0x001E271F
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x001E4356 File Offset: 0x001E2756
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x001E4388 File Offset: 0x001E2788
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060038A5 RID: 14501 RVA: 0x001E43A4 File Offset: 0x001E27A4
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x001E43F4 File Offset: 0x001E27F4
		// (set) Token: 0x060038A7 RID: 14503 RVA: 0x001E4444 File Offset: 0x001E2844
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

		// Token: 0x060038A8 RID: 14504 RVA: 0x001E44A8 File Offset: 0x001E28A8
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

		// Token: 0x060038A9 RID: 14505 RVA: 0x001E451C File Offset: 0x001E291C
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x001E4540 File Offset: 0x001E2940
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x001E4560 File Offset: 0x001E2960
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x001E4570 File Offset: 0x001E2970
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x001E4590 File Offset: 0x001E2990
		// (set) Token: 0x060038AE RID: 14510 RVA: 0x001E45AA File Offset: 0x001E29AA
		public int? NextStreamIndex { get; set; }

		// Token: 0x060038AF RID: 14511 RVA: 0x001E45B4 File Offset: 0x001E29B4
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

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x001E4610 File Offset: 0x001E2A10
		// (set) Token: 0x060038B1 RID: 14513 RVA: 0x001E4630 File Offset: 0x001E2A30
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

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x001E4694 File Offset: 0x001E2A94
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x001E46B4 File Offset: 0x001E2AB4
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060038B4 RID: 14516 RVA: 0x001E46D4 File Offset: 0x001E2AD4
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x001E46F4 File Offset: 0x001E2AF4
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x001E4714 File Offset: 0x001E2B14
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x001E4734 File Offset: 0x001E2B34
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x001E4754 File Offset: 0x001E2B54
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Token: 0x04002429 RID: 9257
		private VorbisReader _reader;

		// Token: 0x0400242A RID: 9258
		private WaveFormat _waveFormat;

		// Token: 0x0400242B RID: 9259
		[ThreadStatic]
		private static float[] _conversionBuffer = null;
	}
}
