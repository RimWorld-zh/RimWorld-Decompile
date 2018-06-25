using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x020009DF RID: 2527
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x04002431 RID: 9265
		private VorbisReader _reader;

		// Token: 0x04002432 RID: 9266
		private WaveFormat _waveFormat;

		// Token: 0x04002433 RID: 9267
		[ThreadStatic]
		private static float[] _conversionBuffer = null;

		// Token: 0x060038A5 RID: 14501 RVA: 0x001E46E9 File Offset: 0x001E2AE9
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x001E471F File Offset: 0x001E2B1F
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x001E4756 File Offset: 0x001E2B56
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
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x001E4788 File Offset: 0x001E2B88
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060038A9 RID: 14505 RVA: 0x001E47A4 File Offset: 0x001E2BA4
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x001E47F4 File Offset: 0x001E2BF4
		// (set) Token: 0x060038AB RID: 14507 RVA: 0x001E4844 File Offset: 0x001E2C44
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

		// Token: 0x060038AC RID: 14508 RVA: 0x001E48A8 File Offset: 0x001E2CA8
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

		// Token: 0x060038AD RID: 14509 RVA: 0x001E491C File Offset: 0x001E2D1C
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x001E4940 File Offset: 0x001E2D40
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x001E4960 File Offset: 0x001E2D60
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x001E4970 File Offset: 0x001E2D70
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x001E4990 File Offset: 0x001E2D90
		// (set) Token: 0x060038B2 RID: 14514 RVA: 0x001E49AA File Offset: 0x001E2DAA
		public int? NextStreamIndex { get; set; }

		// Token: 0x060038B3 RID: 14515 RVA: 0x001E49B4 File Offset: 0x001E2DB4
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
		// (get) Token: 0x060038B4 RID: 14516 RVA: 0x001E4A10 File Offset: 0x001E2E10
		// (set) Token: 0x060038B5 RID: 14517 RVA: 0x001E4A30 File Offset: 0x001E2E30
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
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x001E4A94 File Offset: 0x001E2E94
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x001E4AB4 File Offset: 0x001E2EB4
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x001E4AD4 File Offset: 0x001E2ED4
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060038B9 RID: 14521 RVA: 0x001E4AF4 File Offset: 0x001E2EF4
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x001E4B14 File Offset: 0x001E2F14
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x001E4B34 File Offset: 0x001E2F34
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060038BC RID: 14524 RVA: 0x001E4B54 File Offset: 0x001E2F54
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}
	}
}
