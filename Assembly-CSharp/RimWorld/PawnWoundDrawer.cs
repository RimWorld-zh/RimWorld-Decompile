using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000461 RID: 1121
	[StaticConstructorOnStartup]
	public class PawnWoundDrawer
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x000A9596 File Offset: 0x000A7996
		public PawnWoundDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000A95B8 File Offset: 0x000A79B8
		public void RenderOverBody(Vector3 drawLoc, Mesh bodyMesh, Quaternion quat, bool forPortrait)
		{
			int num = 0;
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.displayWound)
				{
					Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
					if (hediff_Injury == null || !hediff_Injury.IsPermanent())
					{
						num++;
					}
				}
			}
			int num2 = Mathf.CeilToInt((float)num / 2f);
			if (num2 > this.MaxDisplayWounds)
			{
				num2 = this.MaxDisplayWounds;
			}
			while (this.wounds.Count < num2)
			{
				this.wounds.Add(new PawnWoundDrawer.Wound(this.pawn));
				PortraitsCache.SetDirty(this.pawn);
			}
			while (this.wounds.Count > num2)
			{
				this.wounds.Remove(this.wounds.RandomElement<PawnWoundDrawer.Wound>());
				PortraitsCache.SetDirty(this.pawn);
			}
			for (int j = 0; j < this.wounds.Count; j++)
			{
				this.wounds[j].DrawWound(drawLoc, quat, this.pawn.Rotation, forPortrait);
			}
		}

		// Token: 0x04000BEC RID: 3052
		protected Pawn pawn;

		// Token: 0x04000BED RID: 3053
		private List<PawnWoundDrawer.Wound> wounds = new List<PawnWoundDrawer.Wound>();

		// Token: 0x04000BEE RID: 3054
		private int MaxDisplayWounds = 3;

		// Token: 0x02000462 RID: 1122
		private class Wound
		{
			// Token: 0x060013A6 RID: 5030 RVA: 0x000A9710 File Offset: 0x000A7B10
			public Wound(Pawn pawn)
			{
				this.mat = pawn.RaceProps.FleshType.ChooseWoundOverlay();
				if (this.mat == null)
				{
					Log.ErrorOnce(string.Format("No wound graphics data available for flesh type {0}", pawn.RaceProps.FleshType), 76591733, false);
					this.mat = FleshTypeDefOf.Normal.ChooseWoundOverlay();
				}
				this.quat = Quaternion.AngleAxis((float)Rand.Range(0, 360), Vector3.up);
				for (int i = 0; i < 4; i++)
				{
					this.locsPerSide.Add(new Vector2(Rand.Value, Rand.Value));
				}
			}

			// Token: 0x060013A7 RID: 5031 RVA: 0x000A97D4 File Offset: 0x000A7BD4
			public void DrawWound(Vector3 drawLoc, Quaternion bodyQuat, Rot4 bodyRot, bool forPortrait)
			{
				Vector2 vector = this.locsPerSide[bodyRot.AsInt];
				drawLoc += new Vector3((vector.x - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.x, 0f, (vector.y - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.y);
				drawLoc.z -= 0.3f;
				GenDraw.DrawMeshNowOrLater(MeshPool.plane025, drawLoc, this.quat, this.mat, forPortrait);
			}

			// Token: 0x04000BEF RID: 3055
			private List<Vector2> locsPerSide = new List<Vector2>();

			// Token: 0x04000BF0 RID: 3056
			private Material mat;

			// Token: 0x04000BF1 RID: 3057
			private Quaternion quat;

			// Token: 0x04000BF2 RID: 3058
			private static readonly Vector2 WoundSpan = new Vector2(0.18f, 0.3f);
		}
	}
}
