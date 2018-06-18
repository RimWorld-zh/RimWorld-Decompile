using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C05 RID: 3077
	public class MapFileCompressor : IExposable
	{
		// Token: 0x0600433A RID: 17210 RVA: 0x00237CAA File Offset: 0x002360AA
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x00237CBA File Offset: 0x002360BA
		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x00237CCD File Offset: 0x002360CD
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x00237D0C File Offset: 0x0023610C
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error(string.Concat(new object[]
						{
							"Found two compressible things in ",
							curSq,
							". The last was ",
							thing
						}), false);
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x00237DC4 File Offset: 0x002361C4
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDefsByShortHash.ContainsKey(thingDef.shortHash))
				{
					Log.Error(string.Concat(new object[]
					{
						"Hash collision between ",
						thingDef,
						" and  ",
						thingDefsByShortHash[thingDef.shortHash],
						": both have short hash ",
						thingDef.shortHash
					}), false);
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val != 0)
				{
					ThingDef thingDef2 = null;
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.", false);
							thingDefsByShortHash.Add(val, null);
						}
					}
					if (thingDef2 != null)
					{
						try
						{
							Thing thing = ThingMaker.MakeThing(thingDef2, null);
							thing.SetPositionDirect(c);
							loadables.Add(thing);
						}
						catch (Exception arg)
						{
							Log.Error("Could not instantiate compressed thing: " + arg, false);
						}
					}
				}
			});
			return loadables;
		}

		// Token: 0x04002DF3 RID: 11763
		private Map map;

		// Token: 0x04002DF4 RID: 11764
		private byte[] compressedData;

		// Token: 0x04002DF5 RID: 11765
		public CompressibilityDecider compressibilityDecider;
	}
}
