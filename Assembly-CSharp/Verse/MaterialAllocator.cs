using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6F RID: 3439
	[HasDebugOutput]
	internal static class MaterialAllocator
	{
		// Token: 0x06004CF9 RID: 19705 RVA: 0x002815D0 File Offset: 0x0027F9D0
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

		// Token: 0x06004CFA RID: 19706 RVA: 0x00281628 File Offset: 0x0027FA28
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

		// Token: 0x06004CFB RID: 19707 RVA: 0x0028167E File Offset: 0x0027FA7E
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material), false);
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x002816B4 File Offset: 0x0027FAB4
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

		// Token: 0x06004CFD RID: 19709 RVA: 0x00281730 File Offset: 0x0027FB30
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0028174C File Offset: 0x0027FB4C
		[DebugOutput]
		[Category("System")]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text, false);
			}
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x00281818 File Offset: 0x0027FC18
		[DebugOutput]
		[Category("System")]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x002818B0 File Offset: 0x0027FCB0
		[DebugOutput]
		[Category("System")]
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

		// Token: 0x0400335D RID: 13149
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x0400335E RID: 13150
		public static int nextWarningThreshold;

		// Token: 0x0400335F RID: 13151
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x02000D70 RID: 3440
		private struct MaterialInfo
		{
			// Token: 0x04003368 RID: 13160
			public string stackTrace;
		}
	}
}
