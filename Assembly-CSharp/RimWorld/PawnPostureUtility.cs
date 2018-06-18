using System;

namespace RimWorld
{
	// Token: 0x02000499 RID: 1177
	public static class PawnPostureUtility
	{
		// Token: 0x060014D4 RID: 5332 RVA: 0x000B777C File Offset: 0x000B5B7C
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
