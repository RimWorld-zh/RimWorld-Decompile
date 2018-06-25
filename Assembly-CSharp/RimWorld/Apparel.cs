using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C6 RID: 1734
	public class Apparel : ThingWithComps
	{
		// Token: 0x040014E1 RID: 5345
		private bool wornByCorpseInt;

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x001418E0 File Offset: 0x0013FCE0
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				return (pawn_ApparelTracker == null) ? null : pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x00141914 File Offset: 0x0013FD14
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x00141930 File Offset: 0x0013FD30
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text = text + "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x0014196E File Offset: 0x0013FD6E
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x0014198D File Offset: 0x0013FD8D
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00141997 File Offset: 0x0013FD97
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x001419B2 File Offset: 0x0013FDB2
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x001419B8 File Offset: 0x0013FDB8
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x001419D0 File Offset: 0x0013FDD0
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x001419E8 File Offset: 0x0013FDE8
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00141A0C File Offset: 0x0013FE0C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00141A60 File Offset: 0x0013FE60
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}
	}
}
