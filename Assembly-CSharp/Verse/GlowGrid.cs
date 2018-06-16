using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C1B RID: 3099
	public sealed class GlowGrid
	{
		// Token: 0x0600439E RID: 17310 RVA: 0x0023AE2C File Offset: 0x0023922C
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0023AE90 File Offset: 0x00239290
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0023AEC8 File Offset: 0x002392C8
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

		// Token: 0x060043A1 RID: 17313 RVA: 0x0023AFB0 File Offset: 0x002393B0
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c, false);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0023AFD4 File Offset: 0x002393D4
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

		// Token: 0x060043A3 RID: 17315 RVA: 0x0023B010 File Offset: 0x00239410
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0023B05D File Offset: 0x0023945D
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0023B07E File Offset: 0x0023947E
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0023B09A File Offset: 0x0023949A
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0023B0B8 File Offset: 0x002394B8
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

		// Token: 0x04002E37 RID: 11831
		private Map map;

		// Token: 0x04002E38 RID: 11832
		public Color32[] glowGrid;

		// Token: 0x04002E39 RID: 11833
		public Color32[] glowGridNoCavePlants;

		// Token: 0x04002E3A RID: 11834
		private bool glowGridDirty = false;

		// Token: 0x04002E3B RID: 11835
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04002E3C RID: 11836
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04002E3D RID: 11837
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04002E3E RID: 11838
		public const int AlphaOfOverlit = 1;

		// Token: 0x04002E3F RID: 11839
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04002E40 RID: 11840
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04002E41 RID: 11841
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04002E42 RID: 11842
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
