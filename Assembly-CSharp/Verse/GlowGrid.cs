using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public sealed class GlowGrid
	{
		public const int AlphaOfNotOverlit = 0;

		public const int AlphaOfOverlit = 1;

		private const float GameGlowLitThreshold = 0.3f;

		private const float GameGlowOverlitThreshold = 0.9f;

		private const float GroundGameGlowFactor = 3.6f;

		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;

		private Map map;

		public Color32[] glowGrid;

		private bool glowGridDirty;

		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
		}

		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		public float GameGlowAt(IntVec3 c)
		{
			float num = 0f;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1.0)
				{
					return num;
				}
			}
			Color32 color = this.glowGrid[this.map.cellIndices.CellToIndex(c)];
			if (color.a == 1)
			{
				return 1f;
			}
			float b = (float)((float)(color.r + color.g + color.b) / 3.0 / 255.0 * 3.5999999046325684);
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			float glow = this.GameGlowAt(c);
			return GlowGrid.PsychGlowAtGlow(glow);
		}

		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.89999997615814209)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.30000001192092896)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
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
				if (this.initialGlowerLocs != null)
				{
					List<IntVec3>.Enumerator enumerator = this.initialGlowerLocs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IntVec3 current = enumerator.Current;
							this.MarkGlowGridDirty(current);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
					this.initialGlowerLocs = null;
				}
				int numGridCells = this.map.cellIndices.NumGridCells;
				for (int num = 0; num < numGridCells; num++)
				{
					this.glowGrid[num] = new Color32((byte)0, (byte)0, (byte)0, (byte)0);
				}
				HashSet<CompGlower>.Enumerator enumerator2 = this.litGlowers.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						CompGlower current2 = enumerator2.Current;
						this.map.glowFlooder.AddFloodGlowFor(current2);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
		}
	}
}
