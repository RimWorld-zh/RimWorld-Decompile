using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public class Apparel : ThingWithComps
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x001415CC File Offset: 0x0013F9CC
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				return (pawn_ApparelTracker == null) ? null : pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x00141600 File Offset: 0x0013FA00
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x0014161C File Offset: 0x0013FA1C
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

		// Token: 0x06002580 RID: 9600 RVA: 0x0014165A File Offset: 0x0013FA5A
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00141679 File Offset: 0x0013FA79
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00141683 File Offset: 0x0013FA83
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0014169E File Offset: 0x0013FA9E
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x001416A4 File Offset: 0x0013FAA4
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x001416BC File Offset: 0x0013FABC
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x001416D4 File Offset: 0x0013FAD4
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x001416F8 File Offset: 0x0013FAF8
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

		// Token: 0x06002588 RID: 9608 RVA: 0x0014174C File Offset: 0x0013FB4C
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x040014E3 RID: 5347
		private bool wornByCorpseInt;
	}
}
