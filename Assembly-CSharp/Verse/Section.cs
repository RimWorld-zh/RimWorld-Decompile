using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C42 RID: 3138
	public class Section
	{
		// Token: 0x06004517 RID: 17687 RVA: 0x00245064 File Offset: 0x00243464
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
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x00245118 File Offset: 0x00243518
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

		// Token: 0x06004519 RID: 17689 RVA: 0x00245180 File Offset: 0x00243580
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

		// Token: 0x0600451A RID: 17690 RVA: 0x00245258 File Offset: 0x00243658
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

		// Token: 0x0600451B RID: 17691 RVA: 0x00245308 File Offset: 0x00243708
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

		// Token: 0x0600451C RID: 17692 RVA: 0x002453E4 File Offset: 0x002437E4
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}

		// Token: 0x04002F3A RID: 12090
		public IntVec3 botLeft;

		// Token: 0x04002F3B RID: 12091
		public Map map;

		// Token: 0x04002F3C RID: 12092
		public MapMeshFlag dirtyFlags = MapMeshFlag.None;

		// Token: 0x04002F3D RID: 12093
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04002F3E RID: 12094
		private bool foundRect = false;

		// Token: 0x04002F3F RID: 12095
		private CellRect calculatedRect;

		// Token: 0x04002F40 RID: 12096
		public const int Size = 17;
	}
}
