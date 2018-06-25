using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public sealed class GlowGrid
	{
		private Map map;

		public Color32[] glowGrid;

		public Color32[] glowGridNoCavePlants;

		private bool glowGridDirty = false;

		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		public const int AlphaOfNotOverlit = 0;

		public const int AlphaOfOverlit = 1;

		private const float GameGlowLitThreshold = 0.3f;

		private const float GameGlowOverlitThreshold = 0.9f;

		private const float GroundGameGlowFactor = 3.6f;

		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;

		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

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

		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c, false);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

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

		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

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
