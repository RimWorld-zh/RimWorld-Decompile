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
		// Token: 0x04002FAB RID: 12203
		public static Map mapBeingGenerated;

		// Token: 0x04002FAC RID: 12204
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04002FAD RID: 12205
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04002FAE RID: 12206
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04002FAF RID: 12207
		private static List<GenStepDef> tmpGenSteps = new List<GenStepDef>();

		// Token: 0x04002FB0 RID: 12208
		public const string ElevationName = "Elevation";

		// Token: 0x04002FB1 RID: 12209
		public const string FertilityName = "Fertility";

		// Token: 0x04002FB2 RID: 12210
		public const string CavesName = "Caves";

		// Token: 0x04002FB3 RID: 12211
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x0024D29C File Offset: 0x0024B69C
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060045B1 RID: 17841 RVA: 0x0024D2BC File Offset: 0x0024B6BC
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060045B2 RID: 17842 RVA: 0x0024D2DC File Offset: 0x0024B6DC
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060045B3 RID: 17843 RVA: 0x0024D2FC File Offset: 0x0024B6FC
		// (set) Token: 0x060045B4 RID: 17844 RVA: 0x0024D33C File Offset: 0x0024B73C
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

		// Token: 0x060045B5 RID: 17845 RVA: 0x0024D348 File Offset: 0x0024B748
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

		// Token: 0x060045B6 RID: 17846 RVA: 0x0024D524 File Offset: 0x0024B924
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

		// Token: 0x060045B7 RID: 17847 RVA: 0x0024D678 File Offset: 0x0024BA78
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

		// Token: 0x060045B8 RID: 17848 RVA: 0x0024D6B4 File Offset: 0x0024BAB4
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

		// Token: 0x060045B9 RID: 17849 RVA: 0x0024D700 File Offset: 0x0024BB00
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0024D714 File Offset: 0x0024BB14
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

		// Token: 0x060045BB RID: 17851 RVA: 0x0024D754 File Offset: 0x0024BB54
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
	}
}
