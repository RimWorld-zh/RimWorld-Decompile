using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	internal static class MaterialAllocator
	{
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		public static int nextWarningThreshold;

		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		[CompilerGenerated]
		private static Func<IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>>, int> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<KeyValuePair<Material, MaterialAllocator.MaterialInfo>, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>>, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<KeyValuePair<Material, MaterialAllocator.MaterialInfo>, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<MaterialAllocator.MaterialInfo, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<KeyValuePair<Material, MaterialAllocator.MaterialInfo>, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, int> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, string> <>f__am$cache6;

		public static Material Create(Material material)
		{
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = ((!Prefs.DevMode) ? "(unavailable)" : Environment.StackTrace)
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		public static Material Create(Shader shader)
		{
			Material material = new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = ((!Prefs.DevMode) ? "(unavailable)" : Environment.StackTrace)
			};
			MaterialAllocator.TryReport();
			return material;
		}

		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material), false);
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		public static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count), false);
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		[Category("System")]
		[DebugOutput]
		public static void MaterialReport()
		{
			IEnumerable<IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>>> source = from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace;
			if (MaterialAllocator.<>f__mg$cache0 == null)
			{
				MaterialAllocator.<>f__mg$cache0 = new Func<IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>>, int>(Enumerable.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>);
			}
			foreach (string text in (from g in source.OrderByDescending(MaterialAllocator.<>f__mg$cache0)
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text, false);
			}
		}

		[Category("System")]
		[DebugOutput]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		[Category("System")]
		[DebugOutput]
		public static void MaterialDelta()
		{
			IEnumerable<string> source = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			IEnumerable<KeyValuePair<string, int>> source2 = from k in source
			select new KeyValuePair<string, int>(k, currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0));
			foreach (string text in (from kvp in source2
			orderby kvp.Value descending
			select kvp into g
			select string.Format("{0}: {1}", g.Value, g.Key)).Take(20))
			{
				Log.Error(text, false);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MaterialAllocator()
		{
		}

		[CompilerGenerated]
		private static string <MaterialReport>m__0(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
		{
			return kvp.Value.stackTrace;
		}

		[CompilerGenerated]
		private static string <MaterialReport>m__1(IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> g)
		{
			return string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace);
		}

		[CompilerGenerated]
		private static string <MaterialSnapshot>m__2(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
		{
			return kvp.Value.stackTrace;
		}

		[CompilerGenerated]
		private static string <MaterialDelta>m__3(MaterialAllocator.MaterialInfo v)
		{
			return v.stackTrace;
		}

		[CompilerGenerated]
		private static string <MaterialDelta>m__4(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
		{
			return kvp.Value.stackTrace;
		}

		[CompilerGenerated]
		private static int <MaterialDelta>m__5(KeyValuePair<string, int> kvp)
		{
			return kvp.Value;
		}

		[CompilerGenerated]
		private static string <MaterialDelta>m__6(KeyValuePair<string, int> g)
		{
			return string.Format("{0}: {1}", g.Value, g.Key);
		}

		private struct MaterialInfo
		{
			public string stackTrace;
		}

		[CompilerGenerated]
		private sealed class <MaterialDelta>c__AnonStorey0
		{
			internal Dictionary<string, int> currentSnapshot;

			public <MaterialDelta>c__AnonStorey0()
			{
			}

			internal KeyValuePair<string, int> <>m__0(string k)
			{
				return new KeyValuePair<string, int>(k, this.currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0));
			}
		}
	}
}
