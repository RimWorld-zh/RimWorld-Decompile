using System.Collections.Generic;

namespace Verse.AI.Group
{
	public class Transition
	{
		public List<LordToil> sources;

		public LordToil target;

		public List<Trigger> triggers = new List<Trigger>();

		public List<TransitionAction> preActions = new List<TransitionAction>();

		public List<TransitionAction> postActions = new List<TransitionAction>();

		public bool canMoveToSameState = false;

		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		public Transition(LordToil firstSource, LordToil target)
		{
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
			this.target = target;
		}

		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + source);
			}
			else
			{
				if (!this.canMoveToSameState && this.target == source)
				{
					Log.Error("Transition canMoveToSameState and target is source: " + source);
				}
				this.sources.Add(source);
			}
		}

		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil item in sources)
			{
				this.AddSource(item);
			}
		}

		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.triggers.Count)
				{
					if (this.triggers[num].ActivateOn(lord, signal))
					{
						if (this.triggers[num].filters == null)
						{
							goto IL_009b;
						}
						bool flag = true;
						int num2 = 0;
						while (num2 < this.triggers[num].filters.Count)
						{
							if (this.triggers[num].filters[num2].AllowActivation(lord, signal))
							{
								num2++;
								continue;
							}
							flag = false;
							break;
						}
						if (flag)
							goto IL_009b;
					}
					num++;
					continue;
				}
				result = false;
				break;
				IL_009b:
				if (DebugViewSettings.logLordToilTransitions)
				{
					Log.Message("Transitioning " + this.sources + " to " + this.target + " by trigger " + this.triggers[num] + " on signal " + signal);
				}
				this.Execute(lord);
				result = true;
				break;
			}
			return result;
		}

		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
				return;
			for (int i = 0; i < this.preActions.Count; i++)
			{
				this.preActions[i].DoAction(this);
			}
			lord.GotoToil(this.target);
			for (int j = 0; j < this.postActions.Count; j++)
			{
				this.postActions[j].DoAction(this);
			}
		}

		public override string ToString()
		{
			string text = (!this.sources.NullOrEmpty()) ? this.sources[0].ToString() : "null";
			int num = (this.sources != null) ? this.sources.Count : 0;
			string text2 = (this.target != null) ? this.target.ToString() : "null";
			return text + "(" + num + ")->" + text2;
		}
	}
}
