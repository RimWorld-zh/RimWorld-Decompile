using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020001EE RID: 494
	public struct ThoughtState
	{
		// Token: 0x040003E5 RID: 997
		private int stageIndex;

		// Token: 0x040003E6 RID: 998
		private string reason;

		// Token: 0x040003E7 RID: 999
		private const int InactiveIndex = -99999;

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00057044 File Offset: 0x00055444
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0005706C File Offset: 0x0005546C
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00057088 File Offset: 0x00055488
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x000570A4 File Offset: 0x000554A4
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x000570C0 File Offset: 0x000554C0
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

		// Token: 0x0600099B RID: 2459 RVA: 0x000570EC File Offset: 0x000554EC
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00057114 File Offset: 0x00055514
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00057144 File Offset: 0x00055544
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00057168 File Offset: 0x00055568
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

		// Token: 0x0600099F RID: 2463 RVA: 0x00057194 File Offset: 0x00055594
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

		// Token: 0x060009A0 RID: 2464 RVA: 0x000571E0 File Offset: 0x000555E0
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}
	}
}
