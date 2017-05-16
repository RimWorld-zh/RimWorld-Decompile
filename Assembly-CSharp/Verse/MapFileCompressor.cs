using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class MapFileCompressor : IExposable
	{
		private Map map;

		private string compressedString;

		public CompressibilityDecider compressibilityDecider;

		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.compressedString, "compressedThingMap", null, false);
		}

		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedString = GridSaveUtility.CompressedStringForShortGrid(new Func<IntVec3, ushort>(this.HashValueForSquare), this.map);
		}

		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing current in this.map.thingGrid.ThingsAt(curSq))
			{
				if (current.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error(string.Concat(new object[]
						{
							"Found two compressible things in ",
							curSq,
							". The last was ",
							current
						}));
					}
					num = current.def.shortHash;
				}
			}
			return num;
		}

		[DebuggerHidden]
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			MapFileCompressor.<ThingsToSpawnAfterLoad>c__Iterator1F2 <ThingsToSpawnAfterLoad>c__Iterator1F = new MapFileCompressor.<ThingsToSpawnAfterLoad>c__Iterator1F2();
			<ThingsToSpawnAfterLoad>c__Iterator1F.<>f__this = this;
			MapFileCompressor.<ThingsToSpawnAfterLoad>c__Iterator1F2 expr_0E = <ThingsToSpawnAfterLoad>c__Iterator1F;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
