using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020001EE RID: 494
	public struct ThoughtState
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x00057004 File Offset: 0x00055404
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0005702C File Offset: 0x0005542C
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00057048 File Offset: 0x00055448
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00057064 File Offset: 0x00055464
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x00057080 File Offset: 0x00055480
		public static ThoughtState Inactive
		{
			get
			{
				return new ThoughtState
				{
					stageIndex = -99999
				};
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000570AC File Offset: 0x000554AC
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000570D4 File Offset: 0x000554D4
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00057104 File Offset: 0x00055504
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00057128 File Offset: 0x00055528
		public static implicit operator ThoughtState(bool value)
		{
			ThoughtState result;
			if (value)
			{
				result = ThoughtState.ActiveDefault;
			}
			else
			{
				result = ThoughtState.Inactive;
			}
			return result;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00057154 File Offset: 0x00055554
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			bool result;
			if (!this.Active)
			{
				result = false;
			}
			else
			{
				int num = this.StageIndexFor(thoughtDef);
				result = (num >= 0 && thoughtDef.stages[num] != null);
			}
			return result;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000571A0 File Offset: 0x000555A0
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x040003E6 RID: 998
		private int stageIndex;

		// Token: 0x040003E7 RID: 999
		private string reason;

		// Token: 0x040003E8 RID: 1000
		private const int InactiveIndex = -99999;
	}
}
