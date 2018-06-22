using System;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F9 RID: 249
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00039918 File Offset: 0x00037D18
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00039930 File Offset: 0x00037D30
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0003994C File Offset: 0x00037D4C
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00039970 File Offset: 0x00037D70
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.CurJob == null || !pawn.InBed() || !pawn.Awake() || pawn.needs.joy == null)
			{
				result = null;
			}
			else
			{
				float curLevel = pawn.needs.joy.CurLevel;
				if (curLevel > 0.5f)
				{
					result = null;
				}
				else
				{
					Profiler.BeginSample("GetFunWhileInBed");
					Job job = base.TryGiveJob(pawn);
					Profiler.EndSample();
					result = job;
				}
			}
			return result;
		}

		// Token: 0x040002D0 RID: 720
		private const float MaxJoyLevel = 0.5f;
	}
}
