using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E6A RID: 3690
	public class Command_Target : Command
	{
		// Token: 0x04003997 RID: 14743
		public Action<Thing> action;

		// Token: 0x04003998 RID: 14744
		public TargetingParameters targetingParams;

		// Token: 0x060056EA RID: 22250 RVA: 0x002CC844 File Offset: 0x002CAC44
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x002CC878 File Offset: 0x002CAC78
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}
	}
}
