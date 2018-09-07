using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		private VorbisReader _reader;

		private WaveFormat _waveFormat;

		[ThreadStatic]
		private static float[] _conversionBuffer;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int? <NextStreamIndex>k__BackingField;

		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

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

		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		public int? NextStreamIndex
		{
			[CompilerGenerated]
			get
			{
				return this.<NextStreamIndex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NextStreamIndex>k__BackingField = value;
			}
		}

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

		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static VorbisWaveReader()
		{
		}
	}
}
