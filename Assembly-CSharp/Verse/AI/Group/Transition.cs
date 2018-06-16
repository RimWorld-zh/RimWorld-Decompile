using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FF RID: 2559
	public class Transition
	{
		// Token: 0x06003963 RID: 14691 RVA: 0x001E70A0 File Offset: 0x001E54A0
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003964 RID: 14692 RVA: 0x001E7104 File Offset: 0x001E5504
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x001E7124 File Offset: 0x001E5524
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

		// Token: 0x06003966 RID: 14694 RVA: 0x001E7190 File Offset: 0x001E5590
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x001E71E8 File Offset: 0x001E55E8
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x001E7215 File Offset: 0x001E5615
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x001E7224 File Offset: 0x001E5624
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x001E7233 File Offset: 0x001E5633
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x001E7244 File Offset: 0x001E5644
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x001E7284 File Offset: 0x001E5684
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

		// Token: 0x0600396D RID: 14701 RVA: 0x001E73C0 File Offset: 0x001E57C0
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

		// Token: 0x0600396E RID: 14702 RVA: 0x001E7478 File Offset: 0x001E5878
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

		// Token: 0x04002485 RID: 9349
		public List<LordToil> sources;

		// Token: 0x04002486 RID: 9350
		public LordToil target;

		// Token: 0x04002487 RID: 9351
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04002488 RID: 9352
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04002489 RID: 9353
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x0400248A RID: 9354
		public bool canMoveToSameState;

		// Token: 0x0400248B RID: 9355
		public bool updateDutiesIfMovedToSameState = true;
	}
}
