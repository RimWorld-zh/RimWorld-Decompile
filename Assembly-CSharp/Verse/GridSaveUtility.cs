using System;
using System.Collections.Generic;

namespace Verse
{
	public static class GridSaveUtility
	{
		public struct LoadedGridShort
		{
			public ushort val;

			public IntVec3 cell;
		}

		public static string CompressedStringForShortGrid(Func<IntVec3, ushort> shortGetter, Map map)
		{
			int numCells = map.info.NumCells;
			byte[] array = new byte[numCells * 2];
			IntVec3 arg = new IntVec3(0, 0, 0);
			int num = 0;
			while (true)
			{
				ushort num2 = shortGetter(arg);
				byte b = (byte)((int)num2 % 256);
				byte b2 = (byte)((int)num2 / 256);
				array[num] = b;
				array[num + 1] = b2;
				num += 2;
				arg.x++;
				int x = arg.x;
				IntVec3 size = map.Size;
				if (x >= size.x)
				{
					arg.x = 0;
					arg.z++;
					int z = arg.z;
					IntVec3 size2 = map.Size;
					if (z >= size2.z)
						break;
				}
			}
			string str = Convert.ToBase64String(array);
			return ArrayExposeUtility.AddLineBreaksToLongString(str);
		}

		public static IEnumerable<LoadedGridShort> LoadedUShortGrid(string compressedString, Map map)
		{
			compressedString = ArrayExposeUtility.RemoveLineBreaks(compressedString);
			byte[] byteGrid = Convert.FromBase64String(compressedString);
			IntVec3 curSq = new IntVec3(0, 0, 0);
			int byteInd = 0;
			while (true)
			{
				LoadedGridShort loadedElement = new LoadedGridShort
				{
					cell = curSq,
					val = (ushort)(byteGrid[byteInd] + byteGrid[byteInd + 1] * 256)
				};
				byteInd += 2;
				yield return loadedElement;
				curSq.x++;
				int x = curSq.x;
				IntVec3 size = map.Size;
				if (x >= size.x)
				{
					curSq.x = 0;
					curSq.z++;
					int z = curSq.z;
					IntVec3 size2 = map.Size;
					if (z >= size2.z)
						break;
				}
			}
		}
	}
}
