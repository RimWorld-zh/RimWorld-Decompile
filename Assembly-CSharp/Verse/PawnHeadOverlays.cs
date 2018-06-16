using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE3 RID: 3299
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x0600489A RID: 18586 RVA: 0x0026119A File Offset: 0x0025F59A
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x002611AC File Offset: 0x0025F5AC
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

		// Token: 0x0600489C RID: 18588 RVA: 0x002612A1 File Offset: 0x0025F6A1
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x0400311B RID: 12571
		private Pawn pawn;

		// Token: 0x0400311C RID: 12572
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x0400311D RID: 12573
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x0400311E RID: 12574
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x0400311F RID: 12575
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
