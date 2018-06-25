using System;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		private Pawn pawn;

		private const float AngerBlinkPeriod = 1.2f;

		private const float AngerBlinkLength = 0.4f;

		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");

		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

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

		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnHeadOverlays()
		{
		}
	}
}
