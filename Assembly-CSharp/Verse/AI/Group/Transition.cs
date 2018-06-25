using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FE RID: 2558
	public class Transition
	{
		// Token: 0x04002491 RID: 9361
		public List<LordToil> sources;

		// Token: 0x04002492 RID: 9362
		public LordToil target;

		// Token: 0x04002493 RID: 9363
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04002494 RID: 9364
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04002495 RID: 9365
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x04002496 RID: 9366
		public bool canMoveToSameState;

		// Token: 0x04002497 RID: 9367
		public bool updateDutiesIfMovedToSameState = true;

		// Token: 0x06003964 RID: 14692 RVA: 0x001E780C File Offset: 0x001E5C0C
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x001E7870 File Offset: 0x001E5C70
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x001E7890 File Offset: 0x001E5C90
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + source, false);
			}
			else
			{
				if (!this.canMoveToSameState && this.target == source)
				{
					Log.Error("Transition !canMoveToSameState and target is source: " + source, false);
				}
				this.sources.Add(source);
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x001E78FC File Offset: 0x001E5CFC
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x001E7954 File Offset: 0x001E5D54
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x001E7981 File Offset: 0x001E5D81
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x001E7990 File Offset: 0x001E5D90
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x001E799F File Offset: 0x001E5D9F
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x001E79B0 File Offset: 0x001E5DB0
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x001E79F0 File Offset: 0x001E5DF0
		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				if (this.triggers[i].ActivateOn(lord, signal))
				{
					if (this.triggers[i].filters != null)
					{
						bool flag = true;
						for (int j = 0; j < this.triggers[i].filters.Count; j++)
						{
							if (!this.triggers[i].filters[j].AllowActivation(lord, signal))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							goto IL_10F;
						}
					}
					if (DebugViewSettings.logLordToilTransitions)
					{
						Log.Message(string.Concat(new object[]
						{
							"Transitioning ",
							this.sources,
							" to ",
							this.target,
							" by trigger ",
							this.triggers[i],
							" on signal ",
							signal
						}), false);
					}
					this.Execute(lord);
					return true;
				}
				IL_10F:;
			}
			return false;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x001E7B2C File Offset: 0x001E5F2C
		public void Execute(Lord lord)
		{
			if (this.canMoveToSameState || this.target != lord.CurLordToil)
			{
				for (int i = 0; i < this.preActions.Count; i++)
				{
					this.preActions[i].DoAction(this);
				}
				if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
				{
					lord.GotoToil(this.target);
				}
				for (int j = 0; j < this.postActions.Count; j++)
				{
					this.postActions[j].DoAction(this);
				}
			}
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x001E7BE4 File Offset: 0x001E5FE4
		public override string ToString()
		{
			string text = (!this.sources.NullOrEmpty<LordToil>()) ? this.sources[0].ToString() : "null";
			int num = (this.sources != null) ? this.sources.Count : 0;
			string text2 = (this.target != null) ? this.target.ToString() : "null";
			return string.Concat(new object[]
			{
				text,
				"(",
				num,
				")->",
				text2
			});
		}
	}
}
