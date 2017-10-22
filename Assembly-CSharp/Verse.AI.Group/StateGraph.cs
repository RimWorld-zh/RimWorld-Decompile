using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	public class StateGraph
	{
		public List<LordToil> lordToils = new List<LordToil>();

		public List<Transition> transitions = new List<Transition>();

		private static HashSet<LordToil> checkedToils;

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

		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		public void AddTransition(Transition transition)
		{
			this.transitions.Add(transition);
		}

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

		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.");
			}
			using (IEnumerator<LordToil> enumerator = this.lordToils.Distinct().GetEnumerator())
			{
				LordToil toil;
				while (enumerator.MoveNext())
				{
					toil = enumerator.Current;
					int num = (from s in this.lordToils
					where s == toil
					select s).Count();
					if (num != 1)
					{
						Log.Error("Graph has lord toil " + toil + " registered " + num + " times.");
					}
				}
			}
			List<Transition>.Enumerator enumerator2 = this.transitions.GetEnumerator();
			try
			{
				Transition trans;
				while (enumerator2.MoveNext())
				{
					trans = enumerator2.Current;
					int num2 = (from t in this.transitions
					where t == trans
					select t).Count();
					if (num2 != 1)
					{
						Log.Error("Graph has transition " + trans + " registered " + num2 + " times.");
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + toil);
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
	}
}
