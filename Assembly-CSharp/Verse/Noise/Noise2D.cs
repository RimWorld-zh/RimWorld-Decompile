using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F87 RID: 3975
	public class Noise2D : IDisposable
	{
		// Token: 0x04003F04 RID: 16132
		public static readonly double South = -90.0;

		// Token: 0x04003F05 RID: 16133
		public static readonly double North = 90.0;

		// Token: 0x04003F06 RID: 16134
		public static readonly double West = -180.0;

		// Token: 0x04003F07 RID: 16135
		public static readonly double East = 180.0;

		// Token: 0x04003F08 RID: 16136
		public static readonly double AngleMin = -180.0;

		// Token: 0x04003F09 RID: 16137
		public static readonly double AngleMax = 180.0;

		// Token: 0x04003F0A RID: 16138
		public static readonly double Left = -1.0;

		// Token: 0x04003F0B RID: 16139
		public static readonly double Right = 1.0;

		// Token: 0x04003F0C RID: 16140
		public static readonly double Top = -1.0;

		// Token: 0x04003F0D RID: 16141
		public static readonly double Bottom = 1.0;

		// Token: 0x04003F0E RID: 16142
		private int m_width = 0;

		// Token: 0x04003F0F RID: 16143
		private int m_height = 0;

		// Token: 0x04003F10 RID: 16144
		private float[,] m_data = null;

		// Token: 0x04003F11 RID: 16145
		private int m_ucWidth = 0;

		// Token: 0x04003F12 RID: 16146
		private int m_ucHeight = 0;

		// Token: 0x04003F13 RID: 16147
		private int m_ucBorder = 1;

		// Token: 0x04003F14 RID: 16148
		private float[,] m_ucData = null;

		// Token: 0x04003F15 RID: 16149
		private float m_borderValue = float.NaN;

		// Token: 0x04003F16 RID: 16150
		private ModuleBase m_generator = null;

		// Token: 0x04003F17 RID: 16151
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;

		// Token: 0x06005FEF RID: 24559 RVA: 0x0030CBB4 File Offset: 0x0030AFB4
		protected Noise2D()
		{
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x0030CC12 File Offset: 0x0030B012
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x0030CC1E File Offset: 0x0030B01E
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x0030CC2A File Offset: 0x0030B02A
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x0030CC38 File Offset: 0x0030B038
		public Noise2D(int width, int height, ModuleBase generator)
		{
			this.m_generator = generator;
			this.m_width = width;
			this.m_height = height;
			this.m_data = new float[width, height];
			this.m_ucWidth = width + this.m_ucBorder * 2;
			this.m_ucHeight = height + this.m_ucBorder * 2;
			this.m_ucData = new float[width + this.m_ucBorder * 2, height + this.m_ucBorder * 2];
		}

		// Token: 0x17000F6A RID: 3946
		public float this[int x, int y, bool isCropped = true]
		{
			get
			{
				float result;
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					result = this.m_data[x, y];
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					result = this.m_ucData[x, y];
				}
				return result;
			}
			set
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Invalid y position");
					}
					this.m_data[x, y] = value;
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					this.m_ucData[x, y] = value;
				}
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005FF6 RID: 24566 RVA: 0x0030CE6C File Offset: 0x0030B26C
		// (set) Token: 0x06005FF7 RID: 24567 RVA: 0x0030CE87 File Offset: 0x0030B287
		public float Border
		{
			get
			{
				return this.m_borderValue;
			}
			set
			{
				this.m_borderValue = value;
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005FF8 RID: 24568 RVA: 0x0030CE94 File Offset: 0x0030B294
		// (set) Token: 0x06005FF9 RID: 24569 RVA: 0x0030CEAF File Offset: 0x0030B2AF
		public ModuleBase Generator
		{
			get
			{
				return this.m_generator;
			}
			set
			{
				this.m_generator = value;
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06005FFA RID: 24570 RVA: 0x0030CEBC File Offset: 0x0030B2BC
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06005FFB RID: 24571 RVA: 0x0030CED8 File Offset: 0x0030B2D8
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x0030CEF4 File Offset: 0x0030B2F4
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x0030CF14 File Offset: 0x0030B314
		public float[,] GetData(bool isCropped = true, int xCrop = 0, int yCrop = 0, bool isNormalized = false)
		{
			int num;
			int num2;
			float[,] array;
			if (isCropped)
			{
				num = this.m_width;
				num2 = this.m_height;
				array = this.m_data;
			}
			else
			{
				num = this.m_ucWidth;
				num2 = this.m_ucHeight;
				array = this.m_ucData;
			}
			num -= xCrop;
			num2 -= yCrop;
			float[,] array2 = new float[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num3;
					if (isNormalized)
					{
						num3 = (array[i, j] + 1f) / 2f;
					}
					else
					{
						num3 = array[i, j];
					}
					array2[i, j] = num3;
				}
			}
			return array2;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x0030CFE7 File Offset: 0x0030B3E7
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x0030CFF8 File Offset: 0x0030B3F8
		public void Clear(float value)
		{
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					this.m_data[i, j] = value;
				}
			}
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x0030D048 File Offset: 0x0030B448
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x0030D073 File Offset: 0x0030B473
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x0030D084 File Offset: 0x0030B484
		public void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless)
		{
			if (right <= left || bottom <= top)
			{
				throw new ArgumentException("Invalid right/left or bottom/top combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = right - left;
			double num2 = bottom - top;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = left;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = top;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num7;
					if (isSeamless)
					{
						num7 = (float)this.GeneratePlanar(num5, num6);
					}
					else
					{
						double a = this.GeneratePlanar(num5, num6);
						double b = this.GeneratePlanar(num5 + num, num6);
						double a2 = this.GeneratePlanar(num5, num6 + num2);
						double b2 = this.GeneratePlanar(num5 + num, num6 + num2);
						double position = 1.0 - (num5 - left) / num;
						double position2 = 1.0 - (num6 - top) / num2;
						double a3 = Utils.InterpolateLinear(a, b, position);
						double b3 = Utils.InterpolateLinear(a2, b2, position);
						num7 = (float)Utils.InterpolateLinear(a3, b3, position2);
					}
					this.m_ucData[i, j] = num7;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = num7;
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x0030D25C File Offset: 0x0030B65C
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x0030D2A4 File Offset: 0x0030B6A4
		public void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
		{
			if (angleMax <= angleMin || heightMax <= heightMin)
			{
				throw new ArgumentException("Invalid angle or height parameters");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = angleMax - angleMin;
			double num2 = heightMax - heightMin;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = angleMin;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = heightMin;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateCylindrical(num5, num6);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateCylindrical(num5, num6);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x0030D3E8 File Offset: 0x0030B7E8
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x0030D450 File Offset: 0x0030B850
		public void GenerateSpherical(double south, double north, double west, double east)
		{
			if (east <= west || north <= south)
			{
				throw new ArgumentException("Invalid east/west or north/south combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = east - west;
			double num2 = north - south;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = west;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = south;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateSpherical(num6, num5);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateSpherical(num6, num5);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x0030D594 File Offset: 0x0030B994
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x0030D5B4 File Offset: 0x0030B9B4
		public Texture2D GetTexture(Gradient gradient)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					float num;
					if (!float.IsNaN(this.m_borderValue) && (i == 0 || i == this.m_width - this.m_ucBorder || j == 0 || j == this.m_height - this.m_ucBorder))
					{
						num = this.m_borderValue;
					}
					else
					{
						num = this.m_data[i, j];
					}
					array[i + j * this.m_width] = gradient.Evaluate((num + 1f) / 2f);
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x0030D6D4 File Offset: 0x0030BAD4
		public Texture2D GetNormalMap(float intensity)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num = (this.m_ucData[Mathf.Max(0, i - this.m_ucBorder), j] - this.m_ucData[Mathf.Min(i + this.m_ucBorder, this.m_height + this.m_ucBorder), j]) / 2f;
					float num2 = (this.m_ucData[i, Mathf.Max(0, j - this.m_ucBorder)] - this.m_ucData[i, Mathf.Min(j + this.m_ucBorder, this.m_width + this.m_ucBorder)]) / 2f;
					Vector3 a = new Vector3(num * intensity, 0f, 1f);
					Vector3 b = new Vector3(0f, num2 * intensity, 1f);
					Vector3 vector = a + b;
					vector.Normalize();
					Vector3 zero = Vector3.zero;
					zero.x = (vector.x + 1f) / 2f;
					zero.y = (vector.y + 1f) / 2f;
					zero.z = (vector.z + 1f) / 2f;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						array[i - this.m_ucBorder + (j - this.m_ucBorder) * this.m_width] = new Color(zero.x, zero.y, zero.z);
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x0600600A RID: 24586 RVA: 0x0030D904 File Offset: 0x0030BD04
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x0030D91F File Offset: 0x0030BD1F
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x0030D944 File Offset: 0x0030BD44
		protected virtual bool Disposing()
		{
			if (this.m_data != null)
			{
				this.m_data = null;
			}
			this.m_width = 0;
			this.m_height = 0;
			return true;
		}
	}
}
