using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C17 RID: 3095
	public sealed class GlowGrid
	{
		// Token: 0x04002E3F RID: 11839
		private Map map;

		// Token: 0x04002E40 RID: 11840
		public Color32[] glowGrid;

		// Token: 0x04002E41 RID: 11841
		public Color32[] glowGridNoCavePlants;

		// Token: 0x04002E42 RID: 11842
		private bool glowGridDirty = false;

		// Token: 0x04002E43 RID: 11843
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04002E44 RID: 11844
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04002E45 RID: 11845
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04002E46 RID: 11846
		public const int AlphaOfOverlit = 1;

		// Token: 0x04002E47 RID: 11847
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04002E48 RID: 11848
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04002E49 RID: 11849
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04002E4A RID: 11850
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;

		// Token: 0x060043A5 RID: 17317 RVA: 0x0023C1CC File Offset: 0x0023A5CC
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0023C230 File Offset: 0x0023A630
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0023C268 File Offset: 0x0023A668
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

		// Token: 0x060043A8 RID: 17320 RVA: 0x0023C350 File Offset: 0x0023A750
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c, false);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0023C374 File Offset: 0x0023A774
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

		// Token: 0x060043AA RID: 17322 RVA: 0x0023C3B0 File Offset: 0x0023A7B0
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0023C3FD File Offset: 0x0023A7FD
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0023C41E File Offset: 0x0023A81E
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0023C43A File Offset: 0x0023A83A
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0023C458 File Offset: 0x0023A858
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
	}
}
