using System;

namespace RimWorld
{
	// Token: 0x02000497 RID: 1175
	public static class PawnPostureUtility
	{
		// Token: 0x060014CE RID: 5326 RVA: 0x000B7AC8 File Offset: 0x000B5EC8
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
