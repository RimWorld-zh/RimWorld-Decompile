using System;

namespace RimWorld
{
	// Token: 0x02000495 RID: 1173
	public static class PawnPostureUtility
	{
		// Token: 0x060014CB RID: 5323 RVA: 0x000B7778 File Offset: 0x000B5B78
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
