using System;
using System.Collections.Generic;

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
			Scribe_Values.Look<string>(ref this.compressedString, "compressedThingMap", (string)null, false);
		}

		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedString = GridSaveUtility.CompressedStringForShortGrid(new Func<IntVec3, ushort>(this.HashValueForSquare), this.map);
		}

		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = (ushort)0;
			foreach (Thing item in this.map.thingGrid.ThingsAt(curSq))
			{
				if (item.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error("Found two compressible things in " + curSq + ". The last was " + item);
					}
					num = item.def.shortHash;
				}
			}
			return num;
		}

		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDefsByShortHash.ContainsKey(allDef.shortHash))
				{
					Log.Error("Hash collision between " + allDef + " and  " + thingDefsByShortHash[allDef.shortHash] + ": both have short hash " + allDef.shortHash);
				}
				else
				{
					thingDefsByShortHash.Add(allDef.shortHash, allDef);
				}
			}
			foreach (GridSaveUtility.LoadedGridShort item in GridSaveUtility.LoadedUShortGrid(this.compressedString, this.map))
			{
				GridSaveUtility.LoadedGridShort gridThing = item;
				if (gridThing.val != 0)
				{
					ThingDef def = null;
					try
					{
						def = thingDefsByShortHash[gridThing.val];
					}
					catch (KeyNotFoundException)
					{
						Log.Error("Map compressor decompression error: No thingDef with short hash " + gridThing.val + ". Adding as null to dictionary.");
						thingDefsByShortHash.Add(gridThing.val, null);
					}
					Thing th = ThingMaker.MakeThing(def, null);
					th.SetPositionDirect(gridThing.cell);
					yield return th;
				}
			}
		}
	}
}
