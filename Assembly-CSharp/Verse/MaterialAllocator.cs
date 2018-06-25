using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6E RID: 3438
	[HasDebugOutput]
	internal static class MaterialAllocator
	{
		// Token: 0x0400336D RID: 13165
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x0400336E RID: 13166
		public static int nextWarningThreshold;

		// Token: 0x0400336F RID: 13167
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x06004D10 RID: 19728 RVA: 0x00282F6C File Offset: 0x0028136C
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

		// Token: 0x06004D11 RID: 19729 RVA: 0x00282FC4 File Offset: 0x002813C4
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

		// Token: 0x06004D12 RID: 19730 RVA: 0x0028301A File Offset: 0x0028141A
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material), false);
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x00283050 File Offset: 0x00281450
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

		// Token: 0x06004D14 RID: 19732 RVA: 0x002830CC File Offset: 0x002814CC
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x002830E8 File Offset: 0x002814E8
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

		// Token: 0x06004D16 RID: 19734 RVA: 0x002831B4 File Offset: 0x002815B4
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

		// Token: 0x06004D17 RID: 19735 RVA: 0x0028324C File Offset: 0x0028164C
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

		// Token: 0x02000D6F RID: 3439
		private struct MaterialInfo
		{
			// Token: 0x04003378 RID: 13176
			public string stackTrace;
		}
	}
}
