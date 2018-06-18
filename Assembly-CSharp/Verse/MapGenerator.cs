using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C62 RID: 3170
	public static class MapGenerator
	{
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x0024BB10 File Offset: 0x00249F10
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x0024BB30 File Offset: 0x00249F30
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060045A6 RID: 17830 RVA: 0x0024BB50 File Offset: 0x00249F50
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060045A7 RID: 17831 RVA: 0x0024BB70 File Offset: 0x00249F70
		// (set) Token: 0x060045A8 RID: 17832 RVA: 0x0024BBB0 File Offset: 0x00249FB0
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				IntVec3 zero;
				if (!MapGenerator.playerStartSpotInt.IsValid)
				{
					Log.Error("Accessing player start spot before setting it.", false);
					zero = IntVec3.Zero;
				}
				else
				{
					zero = MapGenerator.playerStartSpotInt;
				}
				return zero;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0024BBBC File Offset: 0x00249FBC
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepDef> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.data.Clear();
			MapGenerator.mapBeingGenerated = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, parent.Tile);
			Rand.Seed = seed;
			Map result;
			try
			{
				if (parent != null && parent.HasMap)
				{
					Log.Error("Tried to generate a new map and set " + parent + " as its parent, but this world object already has a map. One world object can't have more than 1 map.", false);
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				map.ConstructComponents();
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map", false);
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				IEnumerable<GenStepDef> enumerable = mapGenerator.genSteps;
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				result = map;
			}
			finally
			{
				DeepProfiler.End();
				MapGenerator.mapBeingGenerated = null;
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0024BD98 File Offset: 0x0024A198
		public static void GenerateContentsIntoMap(IEnumerable<GenStepDef> genStepDefs, Map map, int seed)
		{
			MapGenerator.data.Clear();
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.order, x.index
				select x);
				for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("GenStep - " + MapGenerator.tmpGenSteps[i]);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
						MapGenerator.tmpGenSteps[i].genStep.Generate(map);
					}
					catch (Exception arg)
					{
						Log.Error("Error in GenStep: " + arg, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
			finally
			{
				Rand.PopState();
				RockNoises.Reset();
				MapGenerator.data.Clear();
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0024BEEC File Offset: 0x0024A2EC
		public static T GetVar<T>(string name)
		{
			object obj;
			T result;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				result = (T)((object)obj);
			}
			else
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x0024BF28 File Offset: 0x0024A328
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			bool result;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				result = true;
			}
			else
			{
				var = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x0024BF74 File Offset: 0x0024A374
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x0024BF88 File Offset: 0x0024A388
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			MapGenFloatGrid result;
			if (var != null)
			{
				result = var;
			}
			else
			{
				MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
				MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
				result = mapGenFloatGrid;
			}
			return result;
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x0024BFC8 File Offset: 0x0024A3C8
		private static int GetSeedPart(List<GenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x04002F9A RID: 12186
		public static Map mapBeingGenerated;

		// Token: 0x04002F9B RID: 12187
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04002F9C RID: 12188
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04002F9D RID: 12189
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04002F9E RID: 12190
		private static List<GenStepDef> tmpGenSteps = new List<GenStepDef>();

		// Token: 0x04002F9F RID: 12191
		public const string ElevationName = "Elevation";

		// Token: 0x04002FA0 RID: 12192
		public const string FertilityName = "Fertility";

		// Token: 0x04002FA1 RID: 12193
		public const string CavesName = "Caves";

		// Token: 0x04002FA2 RID: 12194
		public const string RectOfInterestName = "RectOfInterest";
	}
}
