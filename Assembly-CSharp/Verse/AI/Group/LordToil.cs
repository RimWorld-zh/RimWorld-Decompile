using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009ED RID: 2541
	public abstract class LordToil
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600391A RID: 14618 RVA: 0x0004DC8C File Offset: 0x0004C08C
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x0004DCB4 File Offset: 0x0004C0B4
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600391C RID: 14620 RVA: 0x0004DCD0 File Offset: 0x0004C0D0
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x0004DCE8 File Offset: 0x0004C0E8
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x0004DD08 File Offset: 0x0004C108
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x0004DD20 File Offset: 0x0004C120
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x0004DD38 File Offset: 0x0004C138
		public virtual bool ShouldFail
		{
			get
			{
				for (int i = 0; i < this.failConditions.Count; i++)
				{
					if (this.failConditions[i]())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x0004DD8C File Offset: 0x0004C18C
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0004DDA2 File Offset: 0x0004C1A2
		public virtual void Init()
		{
		}

		// Token: 0x06003923 RID: 14627
		public abstract void UpdateAllDuties();

		// Token: 0x06003924 RID: 14628 RVA: 0x0004DDA5 File Offset: 0x0004C1A5
		public virtual void LordToilTick()
		{
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x0004DDA8 File Offset: 0x0004C1A8
		public virtual void Cleanup()
		{
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x0004DDAC File Offset: 0x0004C1AC
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x0004DDC2 File Offset: 0x0004C1C2
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x0004DDC5 File Offset: 0x0004C1C5
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0004DDC8 File Offset: 0x0004C1C8
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0004DDCB File Offset: 0x0004C1CB
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0004DDDC File Offset: 0x0004C1DC
		public override string ToString()
		{
			string text = base.GetType().ToString();
			if (text.Contains('.'))
			{
				text = text.Substring(text.LastIndexOf('.') + 1);
			}
			if (text.Contains('_'))
			{
				text = text.Substring(text.LastIndexOf('_') + 1);
			}
			return text;
		}

		// Token: 0x0400246E RID: 9326
		public Lord lord;

		// Token: 0x0400246F RID: 9327
		public LordToilData data;

		// Token: 0x04002470 RID: 9328
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04002471 RID: 9329
		public AvoidGridMode avoidGridMode = AvoidGridMode.Basic;
	}
}
