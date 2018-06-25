using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	// Token: 0x020009DC RID: 2524
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		// Token: 0x04002415 RID: 9237
		private WaveStream readerStream;

		// Token: 0x04002416 RID: 9238
		private readonly SampleChannel sampleChannel;

		// Token: 0x04002417 RID: 9239
		private readonly int destBytesPerSample;

		// Token: 0x04002418 RID: 9240
		private readonly int sourceBytesPerSample;

		// Token: 0x04002419 RID: 9241
		private readonly long length;

		// Token: 0x0400241A RID: 9242
		private readonly object lockObject;

		// Token: 0x06003884 RID: 14468 RVA: 0x001E387C File Offset: 0x001E1C7C
		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x001E3910 File Offset: 0x001E1D10
		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			if (format == AudioFormat.wav)
			{
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
				}
			}
			else if (format == AudioFormat.mp3)
			{
				this.readerStream = new Mp3FileReader(stream);
			}
			else if (format == AudioFormat.aiff)
			{
				this.readerStream = new AiffFileReader(stream);
			}
			else if (format == AudioFormat.ogg)
			{
				this.readerStream = new VorbisWaveReader(stream);
			}
			else
			{
				Debug.LogWarning("Audio format " + format + " is not supported");
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003886 RID: 14470 RVA: 0x001E39F4 File Offset: 0x001E1DF4
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003887 RID: 14471 RVA: 0x001E3A14 File Offset: 0x001E1E14
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003888 RID: 14472 RVA: 0x001E3A30 File Offset: 0x001E1E30
		// (set) Token: 0x06003889 RID: 14473 RVA: 0x001E3A58 File Offset: 0x001E1E58
		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.readerStream.Position = this.DestToSource(value);
				}
			}
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x001E3AA4 File Offset: 0x001E1EA4
		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			int num = this.Read(waveBuffer.FloatBuffer, offset / 4, count2);
			return num * 4;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x001E3AD8 File Offset: 0x001E1ED8
		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				result = this.sampleChannel.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600388C RID: 14476 RVA: 0x001E3B20 File Offset: 0x001E1F20
		// (set) Token: 0x0600388D RID: 14477 RVA: 0x001E3B40 File Offset: 0x001E1F40
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

		// Token: 0x0600388E RID: 14478 RVA: 0x001E3B50 File Offset: 0x001E1F50
		private long SourceToDest(long sourceBytes)
		{
			return (long)this.destBytesPerSample * (sourceBytes / (long)this.sourceBytesPerSample);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x001E3B78 File Offset: 0x001E1F78
		private long DestToSource(long destBytes)
		{
			return (long)this.sourceBytesPerSample * (destBytes / (long)this.destBytesPerSample);
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x001E3B9E File Offset: 0x001E1F9E
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.readerStream != null)
				{
					this.readerStream.Dispose();
					this.readerStream = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
