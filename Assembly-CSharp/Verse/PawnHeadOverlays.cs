using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CDF RID: 3295
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x060048A9 RID: 18601 RVA: 0x0026258A File Offset: 0x0026098A
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0026259C File Offset: 0x0026099C
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

		// Token: 0x060048AB RID: 18603 RVA: 0x00262691 File Offset: 0x00260A91
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

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
	}
}
