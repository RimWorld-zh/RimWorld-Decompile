using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C4 RID: 1732
	public class Apparel : ThingWithComps
	{
		// Token: 0x040014E1 RID: 5345
		private bool wornByCorpseInt;

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x00141790 File Offset: 0x0013FB90
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				return (pawn_ApparelTracker == null) ? null : pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x001417C4 File Offset: 0x0013FBC4
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x001417E0 File Offset: 0x0013FBE0
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

		// Token: 0x0600257A RID: 9594 RVA: 0x0014181E File Offset: 0x0013FC1E
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0014183D File Offset: 0x0013FC3D
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00141847 File Offset: 0x0013FC47
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00141862 File Offset: 0x0013FC62
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00141868 File Offset: 0x0013FC68
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00141880 File Offset: 0x0013FC80
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00141898 File Offset: 0x0013FC98
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x001418BC File Offset: 0x0013FCBC
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

		// Token: 0x06002582 RID: 9602 RVA: 0x00141910 File Offset: 0x0013FD10
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}
	}
}
