using System;
using System.Collections.Generic;

namespace Verse
{
	public class MapFileCompressor : IExposable
	{
		private Map map;

		private byte[] compressedData;

		public CompressibilityDecider compressibilityDecider;

		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
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
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, (Action<IntVec3, ushort>)delegate(IntVec3 c, ushort val)
			{
				if (val != 0)
				{
					ThingDef def = null;
					try
					{
						def = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.");
						thingDefsByShortHash.Add(val, null);
					}
					Thing thing = ThingMaker.MakeThing(def, null);
					thing.SetPositionDirect(c);
					loadables.Add(thing);
				}
			});
			return loadables;
		}
	}
}
