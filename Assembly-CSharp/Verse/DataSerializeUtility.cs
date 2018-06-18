using System;

namespace Verse
{
	// Token: 0x02000EDF RID: 3807
	public static class DataSerializeUtility
	{
		// Token: 0x060059FA RID: 23034 RVA: 0x002E23A0 File Offset: 0x002E07A0
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x002E23DC File Offset: 0x002E07DC
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x060059FC RID: 23036 RVA: 0x002E23F4 File Offset: 0x002E07F4
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x002E240C File Offset: 0x002E080C
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr != null && arr.Length != 0)
			{
				for (int i = 0; i < elements; i++)
				{
					writer(i, arr[i]);
				}
			}
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x002E244C File Offset: 0x002E084C
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num >> 0 & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x002E24A8 File Offset: 0x002E08A8
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x002E24E4 File Offset: 0x002E08E4
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x002E2530 File Offset: 0x002E0930
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr != null && arr.Length != 0)
			{
				for (int i = 0; i < elements; i++)
				{
					writer(i, (ushort)((int)arr[i * 2] << 0 | (int)arr[i * 2 + 1] << 8));
				}
			}
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x002E2580 File Offset: 0x002E0980
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num >> 0 & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x002E2600 File Offset: 0x002E0A00
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x002E263C File Offset: 0x002E0A3C
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x002E2688 File Offset: 0x002E0A88
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr != null && arr.Length != 0)
			{
				for (int i = 0; i < elements; i++)
				{
					writer(i, (int)arr[i * 4] << 0 | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
				}
			}
		}
	}
}
