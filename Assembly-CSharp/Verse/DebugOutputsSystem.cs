using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000E21 RID: 3617
	[HasDebugOutput]
	internal static class DebugOutputsSystem
	{
		// Token: 0x060054A1 RID: 21665 RVA: 0x002B625C File Offset: 0x002B465C
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

		// Token: 0x060054A2 RID: 21666 RVA: 0x002B646C File Offset: 0x002B486C
		private static long TotalBytes(UnityEngine.Object[] arr)
		{
			long num = 0L;
			foreach (UnityEngine.Object o in arr)
			{
				num += Profiler.GetRuntimeMemorySizeLong(o);
			}
			return num;
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x002B64AD File Offset: 0x002B48AD
		[DebugOutput]
		[ModeRestrictionPlay]
		[Category("System")]
		public static void DynamicDrawThingsList()
		{
			Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x002B64C0 File Offset: 0x002B48C0
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
