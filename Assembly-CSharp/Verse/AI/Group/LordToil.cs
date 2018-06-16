using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F1 RID: 2545
	public abstract class LordToil
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x0004DCA0 File Offset: 0x0004C0A0
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x0004DCC8 File Offset: 0x0004C0C8
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x0004DCE4 File Offset: 0x0004C0E4
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x0004DCFC File Offset: 0x0004C0FC
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x0004DD1C File Offset: 0x0004C11C
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x0004DD34 File Offset: 0x0004C134
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x0004DD4C File Offset: 0x0004C14C
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

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x0004DDA0 File Offset: 0x0004C1A0
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x0004DDB6 File Offset: 0x0004C1B6
		public virtual void Init()
		{
		}

		// Token: 0x06003927 RID: 14631
		public abstract void UpdateAllDuties();

		// Token: 0x06003928 RID: 14632 RVA: 0x0004DDB9 File Offset: 0x0004C1B9
		public virtual void LordToilTick()
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0004DDBC File Offset: 0x0004C1BC
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0004DDC0 File Offset: 0x0004C1C0
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0004DDD6 File Offset: 0x0004C1D6
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x0004DDD9 File Offset: 0x0004C1D9
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0004DDDC File Offset: 0x0004C1DC
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x0004DDDF File Offset: 0x0004C1DF
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0004DDF0 File Offset: 0x0004C1F0
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

		// Token: 0x04002473 RID: 9331
		public Lord lord;

		// Token: 0x04002474 RID: 9332
		public LordToilData data;

		// Token: 0x04002475 RID: 9333
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04002476 RID: 9334
		public AvoidGridMode avoidGridMode = AvoidGridMode.Basic;
	}
}
