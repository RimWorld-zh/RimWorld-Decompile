using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C3E RID: 3134
	public class Section
	{
		// Token: 0x04002F42 RID: 12098
		public IntVec3 botLeft;

		// Token: 0x04002F43 RID: 12099
		public Map map;

		// Token: 0x04002F44 RID: 12100
		public MapMeshFlag dirtyFlags = MapMeshFlag.None;

		// Token: 0x04002F45 RID: 12101
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04002F46 RID: 12102
		private bool foundRect = false;

		// Token: 0x04002F47 RID: 12103
		private CellRect calculatedRect;

		// Token: 0x04002F48 RID: 12104
		public const int Size = 17;

		// Token: 0x0600451E RID: 17694 RVA: 0x0024640C File Offset: 0x0024480C
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				this.layers.Add((SectionLayer)Activator.CreateInstance(type, new object[]
				{
					this
				}));
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x0600451F RID: 17695 RVA: 0x002464C0 File Offset: 0x002448C0
		public CellRect CellRect
		{
			get
			{
				if (!this.foundRect)
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
					this.foundRect = true;
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00246528 File Offset: 0x00244928
		public void DrawSection(bool drawSunShadowsOnly)
		{
			int count = this.layers.Count;
			for (int i = 0; i < count; i++)
			{
				if (!drawSunShadowsOnly || this.layers[i] is SectionLayer_SunShadows)
				{
					this.layers[i].DrawLayer();
				}
			}
			if (!drawSunShadowsOnly && DebugViewSettings.drawSectionEdges)
			{
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			}
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00246600 File Offset: 0x00244A00
		public void RegenerateAllLayers()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							this.layers[i].ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x002466B0 File Offset: 0x00244AB0
		public void RegenerateLayers(MapMeshFlag changeType)
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				if ((sectionLayer.relevantChangeTypes & changeType) != MapMeshFlag.None)
				{
					Profiler.BeginSample(string.Concat(new object[]
					{
						"Regen ",
						sectionLayer.GetType().Name,
						" ",
						this.botLeft
					}));
					try
					{
						sectionLayer.Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							sectionLayer.ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
					Profiler.EndSample();
				}
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0024678C File Offset: 0x00244B8C
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}
	}
}
