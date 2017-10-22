using System;
using System.Text;

namespace Verse
{
	public static class ArrayExposeUtility
	{
		private const int NewlineInterval = 100;

		public static void ExposeByteArray(ref byte[] arr, string label)
		{
			string text = (string)null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				text = Convert.ToBase64String(arr);
				text = ArrayExposeUtility.AddLineBreaksToLongString(text);
			}
			Scribe_Values.Look(ref text, label, (string)null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				text = ArrayExposeUtility.RemoveLineBreaks(text);
				arr = Convert.FromBase64String(text);
			}
		}

		public static void ExposeBoolArray(ref bool[] arr, int mapSizeX, int mapSizeZ, string label)
		{
			int num = mapSizeX * mapSizeZ;
			int num2 = (int)Math.Ceiling((float)num / 6.0);
			byte[] array = new byte[num2];
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				int num3 = 0;
				byte b = (byte)1;
				for (int num4 = 0; num4 < num; num4++)
				{
					if (arr[num4])
					{
						ref byte val = ref array[num3];
						val = (byte)(val | b);
					}
					b = (byte)(b * 2);
					if (b > 32)
					{
						b = (byte)1;
						num3++;
					}
				}
			}
			ArrayExposeUtility.ExposeByteArray(ref array, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num5 = 0;
				byte b2 = (byte)1;
				for (int num6 = 0; num6 < num; num6++)
				{
					if (arr == null)
					{
						arr = new bool[num];
					}
					arr[num6] = ((array[num5] & b2) != 0);
					b2 = (byte)(b2 * 2);
					if (b2 > 32)
					{
						b2 = (byte)1;
						num5++;
					}
				}
			}
		}

		public static string AddLineBreaksToLongString(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length + (str.Length / 100 + 3) * 2 + 1);
			stringBuilder.AppendLine();
			for (int i = 0; i < str.Length; i++)
			{
				stringBuilder.Append(str[i]);
				if (i % 100 == 0)
				{
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		public static string RemoveLineBreaks(string str)
		{
			return str.Replace("\n", string.Empty);
		}
	}
}
