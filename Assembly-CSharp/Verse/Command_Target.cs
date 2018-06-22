using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E67 RID: 3687
	public class Command_Target : Command
	{
		// Token: 0x060056E6 RID: 22246 RVA: 0x002CC52C File Offset: 0x002CA92C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x002CC560 File Offset: 0x002CA960
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x0400398F RID: 14735
		public Action<Thing> action;

		// Token: 0x04003990 RID: 14736
		public TargetingParameters targetingParams;
	}
}
