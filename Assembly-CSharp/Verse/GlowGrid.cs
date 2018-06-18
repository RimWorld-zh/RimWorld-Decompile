using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C1A RID: 3098
	public sealed class GlowGrid
	{
		// Token: 0x0600439C RID: 17308 RVA: 0x0023AE04 File Offset: 0x00239204
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0023AE68 File Offset: 0x00239268
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0023AEA0 File Offset: 0x002392A0
		public float GameGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			float num = 0f;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1f)
				{
					return num;
				}
			}
			Color32[] array = (!ignoreCavePlants) ? this.glowGrid : this.glowGridNoCavePlants;
			Color32 color = array[this.map.cellIndices.CellToIndex(c)];
			float result;
			if (color.a == 1)
			{
				result = 1f;
			}
			else
			{
				float b = (float)(color.r + color.g + color.b) / 3f / 255f * 3.6f;
				b = Mathf.Min(0.5f, b);
				result = Mathf.Max(num, b);
			}
			return result;
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0023AF88 File Offset: 0x00239388
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c, false);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0023AFAC File Offset: 0x002393AC
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			PsychGlow result;
			if (glow > 0.9f)
			{
				result = PsychGlow.Overlit;
			}
			else if (glow > 0.3f)
			{
				result = PsychGlow.Lit;
			}
			else
			{
				result = PsychGlow.Dark;
			}
			return result;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0023AFE8 File Offset: 0x002393E8
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0023B035 File Offset: 0x00239435
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0023B056 File Offset: 0x00239456
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0023B072 File Offset: 0x00239472
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0023B090 File Offset: 0x00239490
		private void RecalculateAllGlow()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Profiler.BeginSample("RecalculateAllGlow");
				if (this.initialGlowerLocs != null)
				{
					foreach (IntVec3 loc in this.initialGlowerLocs)
					{
						this.MarkGlowGridDirty(loc);
					}
					this.initialGlowerLocs = null;
				}
				int numGridCells = this.map.cellIndices.NumGridCells;
				for (int i = 0; i < numGridCells; i++)
				{
					this.glowGrid[i] = new Color32(0, 0, 0, 0);
					this.glowGridNoCavePlants[i] = new Color32(0, 0, 0, 0);
				}
				foreach (CompGlower compGlower in this.litGlowers)
				{
					this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGrid);
					if (compGlower.parent.def.category != ThingCategory.Plant || !compGlower.parent.def.plant.cavePlant)
					{
						this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGridNoCavePlants);
					}
				}
				Profiler.EndSample();
			}
		}

		// Token: 0x04002E35 RID: 11829
		private Map map;

		// Token: 0x04002E36 RID: 11830
		public Color32[] glowGrid;

		// Token: 0x04002E37 RID: 11831
		public Color32[] glowGridNoCavePlants;

		// Token: 0x04002E38 RID: 11832
		private bool glowGridDirty = false;

		// Token: 0x04002E39 RID: 11833
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04002E3A RID: 11834
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04002E3B RID: 11835
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04002E3C RID: 11836
		public const int AlphaOfOverlit = 1;

		// Token: 0x04002E3D RID: 11837
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04002E3E RID: 11838
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04002E3F RID: 11839
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04002E40 RID: 11840
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
