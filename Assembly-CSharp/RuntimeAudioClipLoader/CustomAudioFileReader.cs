using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using System.IO;
using System.Threading;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		private WaveStream readerStream;

		private readonly SampleChannel sampleChannel;

		private readonly int destBytesPerSample;

		private readonly int sourceBytesPerSample;

		private readonly long length;

		private readonly object lockObject;

		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				Monitor.Enter(obj);
				try
				{
					this.readerStream.Position = this.DestToSource(value);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
		}

		public float Volume
		{
			get
			{
				return this.sampleChannel.Volume;
			}
			set
			{
				this.sampleChannel.Volume = value;
			}
		}

		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			switch (format)
			{
			case AudioFormat.wav:
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
				}
				break;
			case AudioFormat.mp3:
				this.readerStream = new Mp3FileReader(stream);
				break;
			case AudioFormat.aiff:
				this.readerStream = new AiffFileReader(stream);
				break;
			case AudioFormat.ogg:
				this.readerStream = new VorbisWaveReader(stream);
				break;
			default:
				Debug.LogWarning("Audio format " + format + " is not supported");
				break;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			int num = this.Read(waveBuffer.FloatBuffer, offset / 4, count2);
			return num * 4;
		}

		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			Monitor.Enter(obj);
			try
			{
				return this.sampleChannel.Read(buffer, offset, count);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		private long SourceToDest(long sourceBytes)
		{
			return this.destBytesPerSample * (sourceBytes / this.sourceBytesPerSample);
		}

		private long DestToSource(long destBytes)
		{
			return this.sourceBytesPerSample * (destBytes / this.destBytesPerSample);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.readerStream != null)
			{
				this.readerStream.Dispose();
				this.readerStream = null;
			}
			base.Dispose(disposing);
		}
	}
}
