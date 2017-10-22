using System;

namespace Verse
{
	public static class DataSerializeUtility
	{
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int num = 0; num < elements; num++)
			{
				array[num] = reader(num);
			}
			return array;
		}

		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (((arr != null) ? arr.Length : 0) != 0)
			{
				for (int num = 0; num < elements; num++)
				{
					writer(num, arr[num]);
				}
			}
		}

		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int num = 0; num < elements; num++)
			{
				ushort num2 = reader(num);
				array[num * 2] = (byte)(num2 >> 0 & 255);
				array[num * 2 + 1] = (byte)(num2 >> 8 & 255);
			}
			return array;
		}

		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (Func<int, ushort>)((int i) => data[i]));
		}

		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, (Action<int, ushort>)delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (((arr != null) ? arr.Length : 0) != 0)
			{
				for (int num = 0; num < elements; num++)
				{
					writer(num, (ushort)(arr[num * 2] << 0 | arr[num * 2 + 1] << 8));
				}
			}
		}

		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int num = 0; num < elements; num++)
			{
				int num2 = reader(num);
				array[num * 4] = (byte)(num2 >> 0 & 255);
				array[num * 4 + 1] = (byte)(num2 >> 8 & 255);
				array[num * 4 + 2] = (byte)(num2 >> 16 & 255);
				array[num * 4 + 3] = (byte)(num2 >> 24 & 255);
			}
			return array;
		}

		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (Func<int, int>)((int i) => data[i]));
		}

		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, (Action<int, int>)delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (((arr != null) ? arr.Length : 0) != 0)
			{
				for (int num = 0; num < elements; num++)
				{
					writer(num, arr[num * 4] << 0 | arr[num * 4 + 1] << 8 | arr[num * 4 + 2] << 16 | arr[num * 4 + 3] << 24);
				}
			}
		}
	}
}
