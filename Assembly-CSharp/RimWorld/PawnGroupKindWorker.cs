using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B5 RID: 693
	public abstract class PawnGroupKindWorker
	{
		// Token: 0x040006B0 RID: 1712
		public PawnGroupKindDef def;

		// Token: 0x040006B1 RID: 1713
		public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>();

		// Token: 0x06000B9D RID: 2973
		public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker);

		// Token: 0x06000B9E RID: 2974 RVA: 0x00068AF4 File Offset: 0x00066EF4
		public List<Pawn> GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, bool errorOnZeroResults = true)
		{
			List<Pawn> list = new List<Pawn>();
			PawnGroupKindWorker.pawnsBeingGeneratedNow.Add(list);
			try
			{
				this.GeneratePawns(parms, groupMaker, list, errorOnZeroResults);
			}
			catch (Exception arg)
			{
				Log.Error("Exception while generating pawn group: " + arg, false);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
				list.Clear();
			}
			finally
			{
				PawnGroupKindWorker.pawnsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06000B9F RID: 2975
		protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true);

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00068B9C File Offset: 0x00066F9C
		public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return true;
		}
	}
}
