using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000E1E RID: 3614
	[HasDebugOutput]
	internal static class DebugOutputsSystem
	{
		// Token: 0x060054BD RID: 21693 RVA: 0x002B7E14 File Offset: 0x002B6214
		[DebugOutput]
		[Category("System")]
		public static void LoadedAssets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Meshes: ",
				array.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array).ToStringBytes("F2"),
				")"
			}));
			UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Materials: ",
				array2.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array2).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   Faded: ",
				FadedMaterialPool.TotalMaterialCount,
				" (",
				FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount);
			UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Textures: ",
				array3.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array3).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Texture list:");
			foreach (UnityEngine.Object @object in array3)
			{
				string text = ((Texture)@object).name;
				if (text.NullOrEmpty())
				{
					text = "-";
				}
				stringBuilder.AppendLine(text);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x002B8024 File Offset: 0x002B6424
		private static long TotalBytes(UnityEngine.Object[] arr)
		{
			long num = 0L;
			foreach (UnityEngine.Object o in arr)
			{
				num += Profiler.GetRuntimeMemorySizeLong(o);
			}
			return num;
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x002B8065 File Offset: 0x002B6465
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("System")]
		public static void DynamicDrawThingsList()
		{
			Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x002B8078 File Offset: 0x002B6478
		[DebugOutput]
		[Category("System")]
		public static void RandByCurveTests()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(0, 30)
			select (float)x).ToArray<float>());
			SimpleCurve curve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(10f, 1f),
					true
				},
				{
					new CurvePoint(15f, 2f),
					true
				},
				{
					new CurvePoint(20f, 2f),
					true
				},
				{
					new CurvePoint(21f, 0.5f),
					true
				},
				{
					new CurvePoint(30f, 0f),
					true
				}
			};
			float num = 0f;
			for (int i = 0; i < 1000000; i++)
			{
				float num2 = Rand.ByCurve(curve);
				num += num2;
				debugHistogram.Add(num2);
			}
			debugHistogram.Display();
			Log.Message(string.Format("Average {0}, calculated as {1}", num / 1000000f, Rand.ByCurveAverage(curve)), false);
		}
	}
}
