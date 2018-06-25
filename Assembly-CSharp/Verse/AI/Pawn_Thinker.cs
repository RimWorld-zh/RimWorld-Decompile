using System;

namespace Verse.AI
{
	public class Pawn_Thinker
	{
		public Pawn pawn;

		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

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
