using System;
using System.Text;

namespace Verse
{
	public static class DataExposeUtility
	{
		private const int NewlineInterval = 100;

		public static void ByteArray(ref byte[] arr, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving && arr != null)
			{
				byte[] array = CompressUtility.Compress(arr);
				if (array.Length < arr.Length)
				{
					string text = DataExposeUtility.AddLineBreaksToLongString(Convert.ToBase64String(array));
					Scribe_Values.Look(ref text, label + "Deflate", (string)null, false);
				}
				else
				{
					string text2 = DataExposeUtility.AddLineBreaksToLongString(Convert.ToBase64String(arr));
					Scribe_Values.Look(ref text2, label, (string)null, false);
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string text3 = (string)null;
				Scribe_Values.Look(ref text3, label + "Deflate", (string)null, false);
				if (text3 != null)
				{
					arr = CompressUtility.Decompress(Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(text3)));
				}
				else
				{
					Scribe_Values.Look(ref text3, label, (string)null, false);
					if (text3 != null)
					{
						arr = Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(text3));
					}
					else
					{
						arr = null;
					}
				}
			}
		}

		public static void BoolArray(ref bool[] arr, int elements, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (arr.Length != elements)
				{
					Log.ErrorOnce(string.Format("Bool array length mismatch for {0}", label), 74135877);
				}
				elements = arr.Length;
			}
			int num = (elements + 7) / 8;
			byte[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = new byte[num];
				int num2 = 0;
				byte b = (byte)1;
				for (int num3 = 0; num3 < elements; num3++)
				{
					if (arr[num3])
					{
						ref byte val = ref array[num2];
						val = (byte)(val | b);
					}
					b = (byte)(b * 2);
					if (b == 0)
					{
						b = (byte)1;
						num2++;
					}
				}
			}
			DataExposeUtility.ByteArray(ref array, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (arr == null)
				{
					arr = new bool[elements];
				}
				if (((array != null) ? array.Length : 0) != 0)
				{
					if (array.Length != num)
					{
						int num4 = 0;
						byte b2 = (byte)1;
						for (int num5 = 0; num5 < elements; num5++)
						{
							arr[num5] = ((array[num4] & b2) != 0);
							b2 = (byte)(b2 * 2);
							if (b2 > 32)
							{
								b2 = (byte)1;
								num4++;
							}
						}
					}
					else
					{
						int num6 = 0;
						byte b3 = (byte)1;
						for (int num7 = 0; num7 < elements; num7++)
						{
							arr[num7] = ((array[num6] & b3) != 0);
							b3 = (byte)(b3 * 2);
							if (b3 == 0)
							{
								b3 = (byte)1;
								num6++;
							}
						}
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
				if (((i % 100 == 0) ? i : 0) != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(str[i]);
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		public static string RemoveLineBreaks(string str)
		{
			return str.Replace("\n", "").Replace("\r", "");
		}
	}
}
