using System;

namespace Verse.AI
{
	// Token: 0x02000AA0 RID: 2720
	public class Pawn_Thinker
	{
		// Token: 0x06003CAC RID: 15532 RVA: 0x002023A7 File Offset: 0x002007A7
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003CAD RID: 15533 RVA: 0x002023B8 File Offset: 0x002007B8
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x002023E0 File Offset: 0x002007E0
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x0020240C File Offset: 0x0020080C
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x00202434 File Offset: 0x00200834
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00202460 File Offset: 0x00200860
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

		// Token: 0x06003CB2 RID: 15538 RVA: 0x002024E8 File Offset: 0x002008E8
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

		// Token: 0x0400266C RID: 9836
		public Pawn pawn;
	}
}
