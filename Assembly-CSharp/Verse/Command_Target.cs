using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E69 RID: 3689
	public class Command_Target : Command
	{
		// Token: 0x0400398F RID: 14735
		public Action<Thing> action;

		// Token: 0x04003990 RID: 14736
		public TargetingParameters targetingParams;

		// Token: 0x060056EA RID: 22250 RVA: 0x002CC658 File Offset: 0x002CAA58
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x002CC68C File Offset: 0x002CAA8C
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}
	}
}
