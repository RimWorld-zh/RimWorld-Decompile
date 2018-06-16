using System;

namespace Verse.AI
{
	// Token: 0x02000AA4 RID: 2724
	public class Pawn_Thinker
	{
		// Token: 0x06003CAF RID: 15535 RVA: 0x00201FAF File Offset: 0x002003AF
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x00201FC0 File Offset: 0x002003C0
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x00201FE8 File Offset: 0x002003E8
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x00202014 File Offset: 0x00200414
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x0020203C File Offset: 0x0020043C
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x00202068 File Offset: 0x00200468
		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			foreach (ThinkNode thinkNode in this.MainThinkNodeRoot.ChildrenRecursive)
			{
				T t = thinkNode as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x002020F0 File Offset: 0x002004F0
		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T t = this.TryGetMainTreeThinkNode<T>();
			if (t == null)
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" looked for ThinkNode of type ",
					typeof(T),
					" and didn't find it."
				}), false);
			}
			return t;
		}

		// Token: 0x04002671 RID: 9841
		public Pawn pawn;
	}
}
