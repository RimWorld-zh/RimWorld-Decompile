using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C41 RID: 3137
	public class Section
	{
		// Token: 0x04002F49 RID: 12105
		public IntVec3 botLeft;

		// Token: 0x04002F4A RID: 12106
		public Map map;

		// Token: 0x04002F4B RID: 12107
		public MapMeshFlag dirtyFlags = MapMeshFlag.None;

		// Token: 0x04002F4C RID: 12108
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04002F4D RID: 12109
		private bool foundRect = false;

		// Token: 0x04002F4E RID: 12110
		private CellRect calculatedRect;

		// Token: 0x04002F4F RID: 12111
		public const int Size = 17;

		// Token: 0x06004521 RID: 17697 RVA: 0x002467C8 File Offset: 0x00244BC8
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

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004522 RID: 17698 RVA: 0x0024687C File Offset: 0x00244C7C
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

		// Token: 0x06004523 RID: 17699 RVA: 0x002468E4 File Offset: 0x00244CE4
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

		// Token: 0x06004524 RID: 17700 RVA: 0x002469BC File Offset: 0x00244DBC
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

		// Token: 0x06004525 RID: 17701 RVA: 0x00246A6C File Offset: 0x00244E6C
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

		// Token: 0x06004526 RID: 17702 RVA: 0x00246B48 File Offset: 0x00244F48
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}
	}
}
