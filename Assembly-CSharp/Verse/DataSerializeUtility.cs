using System;

namespace Verse
{
	// Token: 0x02000EE1 RID: 3809
	public static class DataSerializeUtility
	{
		// Token: 0x06005A1E RID: 23070 RVA: 0x002E44F4 File Offset: 0x002E28F4
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x002E4530 File Offset: 0x002E2930
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x002E4548 File Offset: 0x002E2948
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x002E4560 File Offset: 0x002E2960
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

		// Token: 0x06005A22 RID: 23074 RVA: 0x002E45A0 File Offset: 0x002E29A0
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

		// Token: 0x06005A23 RID: 23075 RVA: 0x002E45FC File Offset: 0x002E29FC
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x002E4638 File Offset: 0x002E2A38
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x002E4684 File Offset: 0x002E2A84
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

		// Token: 0x06005A26 RID: 23078 RVA: 0x002E46D4 File Offset: 0x002E2AD4
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

		// Token: 0x06005A27 RID: 23079 RVA: 0x002E4754 File Offset: 0x002E2B54
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x002E4790 File Offset: 0x002E2B90
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x002E47DC File Offset: 0x002E2BDC
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
