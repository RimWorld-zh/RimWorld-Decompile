using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F82 RID: 3970
	public class Noise2D : IDisposable
	{
		// Token: 0x06005FBC RID: 24508 RVA: 0x0030A24C File Offset: 0x0030864C
		protected Noise2D()
		{
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x0030A2AA File Offset: 0x003086AA
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x0030A2B6 File Offset: 0x003086B6
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x0030A2C2 File Offset: 0x003086C2
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0030A2D0 File Offset: 0x003086D0
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

		// Token: 0x17000F67 RID: 3943
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

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005FC3 RID: 24515 RVA: 0x0030A504 File Offset: 0x00308904
		// (set) Token: 0x06005FC4 RID: 24516 RVA: 0x0030A51F File Offset: 0x0030891F
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

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005FC5 RID: 24517 RVA: 0x0030A52C File Offset: 0x0030892C
		// (set) Token: 0x06005FC6 RID: 24518 RVA: 0x0030A547 File Offset: 0x00308947
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

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005FC7 RID: 24519 RVA: 0x0030A554 File Offset: 0x00308954
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005FC8 RID: 24520 RVA: 0x0030A570 File Offset: 0x00308970
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x0030A58C File Offset: 0x0030898C
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x0030A5AC File Offset: 0x003089AC
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

		// Token: 0x06005FCB RID: 24523 RVA: 0x0030A67F File Offset: 0x00308A7F
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x0030A690 File Offset: 0x00308A90
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

		// Token: 0x06005FCD RID: 24525 RVA: 0x0030A6E0 File Offset: 0x00308AE0
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x0030A70B File Offset: 0x00308B0B
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x0030A71C File Offset: 0x00308B1C
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

		// Token: 0x06005FD0 RID: 24528 RVA: 0x0030A8F4 File Offset: 0x00308CF4
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x0030A93C File Offset: 0x00308D3C
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

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0030AA80 File Offset: 0x00308E80
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0030AAE8 File Offset: 0x00308EE8
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

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0030AC2C File Offset: 0x0030902C
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x0030AC4C File Offset: 0x0030904C
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

		// Token: 0x06005FD6 RID: 24534 RVA: 0x0030AD6C File Offset: 0x0030916C
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

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005FD7 RID: 24535 RVA: 0x0030AF9C File Offset: 0x0030939C
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06005FD8 RID: 24536 RVA: 0x0030AFB7 File Offset: 0x003093B7
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x0030AFDC File Offset: 0x003093DC
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

		// Token: 0x04003EE7 RID: 16103
		public static readonly double South = -90.0;

		// Token: 0x04003EE8 RID: 16104
		public static readonly double North = 90.0;

		// Token: 0x04003EE9 RID: 16105
		public static readonly double West = -180.0;

		// Token: 0x04003EEA RID: 16106
		public static readonly double East = 180.0;

		// Token: 0x04003EEB RID: 16107
		public static readonly double AngleMin = -180.0;

		// Token: 0x04003EEC RID: 16108
		public static readonly double AngleMax = 180.0;

		// Token: 0x04003EED RID: 16109
		public static readonly double Left = -1.0;

		// Token: 0x04003EEE RID: 16110
		public static readonly double Right = 1.0;

		// Token: 0x04003EEF RID: 16111
		public static readonly double Top = -1.0;

		// Token: 0x04003EF0 RID: 16112
		public static readonly double Bottom = 1.0;

		// Token: 0x04003EF1 RID: 16113
		private int m_width = 0;

		// Token: 0x04003EF2 RID: 16114
		private int m_height = 0;

		// Token: 0x04003EF3 RID: 16115
		private float[,] m_data = null;

		// Token: 0x04003EF4 RID: 16116
		private int m_ucWidth = 0;

		// Token: 0x04003EF5 RID: 16117
		private int m_ucHeight = 0;

		// Token: 0x04003EF6 RID: 16118
		private int m_ucBorder = 1;

		// Token: 0x04003EF7 RID: 16119
		private float[,] m_ucData = null;

		// Token: 0x04003EF8 RID: 16120
		private float m_borderValue = float.NaN;

		// Token: 0x04003EF9 RID: 16121
		private ModuleBase m_generator = null;

		// Token: 0x04003EFA RID: 16122
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed = false;
	}
}
