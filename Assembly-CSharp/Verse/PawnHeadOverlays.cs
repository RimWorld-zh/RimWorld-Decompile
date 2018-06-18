using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE2 RID: 3298
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06004898 RID: 18584 RVA: 0x00261172 File Offset: 0x0025F572
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00261184 File Offset: 0x0025F584
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

		// Token: 0x0600489A RID: 18586 RVA: 0x00261279 File Offset: 0x0025F679
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04003119 RID: 12569
		private Pawn pawn;

		// Token: 0x0400311A RID: 12570
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x0400311B RID: 12571
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x0400311C RID: 12572
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x0400311D RID: 12573
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
