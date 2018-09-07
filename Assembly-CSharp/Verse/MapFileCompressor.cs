using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

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
			int major = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			int minor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
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
			});
			return loadables;
		}

		[CompilerGenerated]
		private sealed class <ThingsToSpawnAfterLoad>c__AnonStorey0
		{
			internal int major;

			internal int minor;

			internal Dictionary<ushort, ThingDef> thingDefsByShortHash;

			internal List<Thing> loadables;

			public <ThingsToSpawnAfterLoad>c__AnonStorey0()
			{
			}

			internal void <>m__0(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, this.major, this.minor);
				if (thingDef == null)
				{
					try
					{
						thingDef = this.thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef2 != null)
						{
							thingDef = thingDef2;
							this.thingDefsByShortHash.Add(val, thingDef2);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.", false);
							this.thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef != null)
				{
					try
					{
						Thing thing = ThingMaker.MakeThing(thingDef, null);
						thing.SetPositionDirect(c);
						this.loadables.Add(thing);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate compressed thing: " + arg, false);
					}
				}
			}
		}
	}
}
