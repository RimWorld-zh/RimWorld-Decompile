using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E68 RID: 3688
	public class Command_Target : Command
	{
		// Token: 0x060056C6 RID: 22214 RVA: 0x002CA91C File Offset: 0x002C8D1C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x002CA950 File Offset: 0x002C8D50
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x04003980 RID: 14720
		public Action<Thing> action;

		// Token: 0x04003981 RID: 14721
		public TargetingParameters targetingParams;
	}
}
