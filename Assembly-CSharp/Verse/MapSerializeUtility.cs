using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class MapSerializeUtility
	{
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		[CompilerGenerated]
		private sealed class <SerializeUshort>c__AnonStorey0
		{
			internal Func<IntVec3, ushort> shortReader;

			internal Map map;

			public <SerializeUshort>c__AnonStorey0()
			{
			}

			internal ushort <>m__0(int idx)
			{
				return this.shortReader(this.map.cellIndices.IndexToCell(idx));
			}
		}

		[CompilerGenerated]
		private sealed class <LoadUshort>c__AnonStorey1
		{
			internal Action<IntVec3, ushort> shortWriter;

			internal Map map;

			public <LoadUshort>c__AnonStorey1()
			{
			}

			internal void <>m__0(int idx, ushort data)
			{
				this.shortWriter(this.map.cellIndices.IndexToCell(idx), data);
			}
		}
	}
}
