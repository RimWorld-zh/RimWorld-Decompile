using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE1 RID: 3297
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x04003124 RID: 12580
		private Pawn pawn;

		// Token: 0x04003125 RID: 12581
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04003126 RID: 12582
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04003127 RID: 12583
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04003128 RID: 12584
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");

		// Token: 0x060048AC RID: 18604 RVA: 0x00262666 File Offset: 0x00260A66
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x00262678 File Offset: 0x00260A78
		public void RenderStatusOverlays(Vector3 bodyLoc, Quaternion quat, Mesh headMesh)
		{
			if (this.pawn.IsColonistPlayerControlled)
			{
				Vector3 headLoc = bodyLoc + new Vector3(0f, 0f, 0.32f);
				if (this.pawn.needs.mood != null && !this.pawn.Downed && this.pawn.HitPoints > 0)
				{
					if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						if (Time.time % 1.2f < 0.4f)
						{
							this.DrawHeadGlow(headLoc, PawnHeadOverlays.MentalStateImminentMat);
						}
					}
					else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching)
					{
						if (Time.time % 1.2f < 0.4f)
						{
							this.DrawHeadGlow(headLoc, PawnHeadOverlays.UnhappyMat);
						}
					}
				}
			}
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0026276D File Offset: 0x00260B6D
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}
	}
}
