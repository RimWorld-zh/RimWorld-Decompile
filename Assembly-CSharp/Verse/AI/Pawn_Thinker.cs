using System;

namespace Verse.AI
{
	// Token: 0x02000AA3 RID: 2723
	public class Pawn_Thinker
	{
		// Token: 0x04002674 RID: 9844
		public Pawn pawn;

		// Token: 0x06003CB0 RID: 15536 RVA: 0x002027B1 File Offset: 0x00200BB1
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x002027C4 File Offset: 0x00200BC4
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x002027EC File Offset: 0x00200BEC
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x00202818 File Offset: 0x00200C18
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x00202840 File Offset: 0x00200C40
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x0020286C File Offset: 0x00200C6C
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

		// Token: 0x06003CB6 RID: 15542 RVA: 0x002028F4 File Offset: 0x00200CF4
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
	}
}
