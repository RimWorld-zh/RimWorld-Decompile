using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F0 RID: 2544
	public abstract class LordToil
	{
		// Token: 0x0400247F RID: 9343
		public Lord lord;

		// Token: 0x04002480 RID: 9344
		public LordToilData data;

		// Token: 0x04002481 RID: 9345
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04002482 RID: 9346
		public AvoidGridMode avoidGridMode = AvoidGridMode.Basic;

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x0004DC88 File Offset: 0x0004C088
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x0004DCB0 File Offset: 0x0004C0B0
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x0004DCCC File Offset: 0x0004C0CC
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x0004DCE4 File Offset: 0x0004C0E4
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x0004DD04 File Offset: 0x0004C104
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x0004DD1C File Offset: 0x0004C11C
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x0004DD34 File Offset: 0x0004C134
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
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x0004DD88 File Offset: 0x0004C188
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x0004DD9E File Offset: 0x0004C19E
		public virtual void Init()
		{
		}

		// Token: 0x06003928 RID: 14632
		public abstract void UpdateAllDuties();

		// Token: 0x06003929 RID: 14633 RVA: 0x0004DDA1 File Offset: 0x0004C1A1
		public virtual void LordToilTick()
		{
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0004DDA4 File Offset: 0x0004C1A4
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0004DDA8 File Offset: 0x0004C1A8
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x0004DDBE File Offset: 0x0004C1BE
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0004DDC1 File Offset: 0x0004C1C1
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x0004DDC4 File Offset: 0x0004C1C4
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0004DDC7 File Offset: 0x0004C1C7
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0004DDD8 File Offset: 0x0004C1D8
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
	}
}
