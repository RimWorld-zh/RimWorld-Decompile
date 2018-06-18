using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x020009FE RID: 2558
	public class StateGraph
	{
		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600395E RID: 14686 RVA: 0x001E6DE8 File Offset: 0x001E51E8
		// (set) Token: 0x0600395F RID: 14687 RVA: 0x001E6E09 File Offset: 0x001E5209
		public LordToil StartingToil
		{
			get
			{
				return this.lordToils[0];
			}
			set
			{
				if (this.lordToils.Contains(value))
				{
					this.lordToils.Remove(value);
				}
				this.lordToils.Insert(0, value);
			}
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x001E6E37 File Offset: 0x001E5237
		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x001E6E46 File Offset: 0x001E5246
		public void AddTransition(Transition transition)
		{
			this.transitions.Add(transition);
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x001E6E58 File Offset: 0x001E5258
		public StateGraph AttachSubgraph(StateGraph subGraph)
		{
			for (int i = 0; i < subGraph.lordToils.Count; i++)
			{
				this.lordToils.Add(subGraph.lordToils[i]);
			}
			for (int j = 0; j < subGraph.transitions.Count; j++)
			{
				this.transitions.Add(subGraph.transitions[j]);
			}
			return subGraph;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x001E6ED8 File Offset: 0x001E52D8
		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.", false);
			}
			using (IEnumerator<LordToil> enumerator = this.lordToils.Distinct<LordToil>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordToil toil = enumerator.Current;
					int num = (from s in this.lordToils
					where s == toil
					select s).Count<LordToil>();
					if (num != 1)
					{
						Log.Error(string.Concat(new object[]
						{
							"Graph has lord toil ",
							toil,
							" registered ",
							num,
							" times."
						}), false);
					}
				}
			}
			using (List<Transition>.Enumerator enumerator2 = this.transitions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Transition trans = enumerator2.Current;
					int num2 = (from t in this.transitions
					where t == trans
					select t).Count<Transition>();
					if (num2 != 1)
					{
						Log.Error(string.Concat(new object[]
						{
							"Graph has transition ",
							trans,
							" registered ",
							num2,
							" times."
						}), false);
					}
				}
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x001E708C File Offset: 0x001E548C
		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + toil, false);
			}
			StateGraph.checkedToils.Add(toil);
			for (int i = 0; i < this.transitions.Count; i++)
			{
				Transition transition = this.transitions[i];
				if (transition.sources.Contains(toil) && !StateGraph.checkedToils.Contains(toil))
				{
					this.CheckForUnregisteredLinkedToilsRecursive(transition.target);
				}
			}
		}

		// Token: 0x04002482 RID: 9346
		public List<LordToil> lordToils = new List<LordToil>();

		// Token: 0x04002483 RID: 9347
		public List<Transition> transitions = new List<Transition>();

		// Token: 0x04002484 RID: 9348
		private static HashSet<LordToil> checkedToils;
	}
}
