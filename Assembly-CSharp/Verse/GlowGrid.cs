#define ENABLE_PROFILER
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
			float result;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1.0)
				{
					result = num;
					goto IL_00d7;
				}
			}
			Color32[] array = (!ignoreCavePlants) ? this.glowGrid : this.glowGridNoCavePlants;
			Color32 color = array[this.map.cellIndices.CellToIndex(c)];
			if (color.a == 1)
			{
				result = 1f;
			}
			else
			{
				float b = (float)((float)(color.r + color.g + color.b) / 3.0 / 255.0 * 3.5999999046325684);
				b = Mathf.Min(0.5f, b);
				result = Mathf.Max(num, b);
			}
			goto IL_00d7;
			IL_00d7:
			return result;
		}

		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c, false);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			return (PsychGlow)((!(glow > 0.89999997615814209)) ? ((glow > 0.30000001192092896) ? 1 : 0) : 2);
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
					foreach (IntVec3 initialGlowerLoc in this.initialGlowerLocs)
					{
						this.MarkGlowGridDirty(initialGlowerLoc);
					}
					this.initialGlowerLocs = null;
				}
				int numGridCells = this.map.cellIndices.NumGridCells;
				for (int num = 0; num < numGridCells; num++)
				{
					this.glowGrid[num] = new Color32((byte)0, (byte)0, (byte)0, (byte)0);
					this.glowGridNoCavePlants[num] = new Color32((byte)0, (byte)0, (byte)0, (byte)0);
				}
				foreach (CompGlower litGlower in this.litGlowers)
				{
					this.map.glowFlooder.AddFloodGlowFor(litGlower, this.glowGrid);
					if (litGlower.parent.def.category != ThingCategory.Plant || !litGlower.parent.def.plant.cavePlant)
					{
						this.map.glowFlooder.AddFloodGlowFor(litGlower, this.glowGridNoCavePlants);
					}
				}
				Profiler.EndSample();
			}
		}
	}
}
