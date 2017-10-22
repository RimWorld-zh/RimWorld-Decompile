namespace Verse.AI
{
	public class Pawn_Thinker
	{
		public Pawn pawn;

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

		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			foreach (ThinkNode item in this.MainThinkNodeRoot.ChildrenRecursive)
			{
				T val = (T)(item as T);
				if (val != null)
				{
					return val;
				}
			}
			return (T)null;
		}

		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T val = this.TryGetMainTreeThinkNode<T>();
			if (val == null)
			{
				Log.Warning(this.pawn + " looked for ThinkNode of type " + typeof(T) + " and didn't find it.");
			}
			return val;
		}
	}
}
