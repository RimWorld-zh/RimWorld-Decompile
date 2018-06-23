using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public class AlertsReadout
	{
		// Token: 0x0400173F RID: 5951
		private List<Alert> activeAlerts = new List<Alert>(16);

		// Token: 0x04001740 RID: 5952
		private int curAlertIndex = 0;

		// Token: 0x04001741 RID: 5953
		private float lastFinalY = 0f;

		// Token: 0x04001742 RID: 5954
		private int mouseoverAlertIndex = -1;

		// Token: 0x04001743 RID: 5955
		private readonly List<Alert> AllAlerts = new List<Alert>();

		// Token: 0x04001744 RID: 5956
		private const int StartTickDelay = 600;

		// Token: 0x04001745 RID: 5957
		public const float AlertListWidth = 164f;

		// Token: 0x04001746 RID: 5958
		private static int AlertCycleLength = 20;

		// Token: 0x04001747 RID: 5959
		private readonly List<AlertPriority> PriosInDrawOrder = null;

		// Token: 0x06002B70 RID: 11120 RVA: 0x0016F9D0 File Offset: 0x0016DDD0
		public AlertsReadout()
		{
			this.AllAlerts.Clear();
			foreach (Type type in typeof(Alert).AllLeafSubclasses())
			{
				this.AllAlerts.Add((Alert)Activator.CreateInstance(type));
			}
			if (this.PriosInDrawOrder == null)
			{
				this.PriosInDrawOrder = new List<AlertPriority>();
				IEnumerator enumerator2 = Enum.GetValues(typeof(AlertPriority)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj = enumerator2.Current;
						AlertPriority item = (AlertPriority)obj;
						this.PriosInDrawOrder.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.PriosInDrawOrder.Reverse();
			}
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x0016FB18 File Offset: 0x0016DF18
		public void AlertsReadoutUpdate()
		{
			if (Mathf.Max(Find.TickManager.TicksGame, Find.TutorialState.endTick) >= 600)
			{
				if (Find.Storyteller.def.disableAlerts)
				{
					this.activeAlerts.Clear();
				}
				else
				{
					this.curAlertIndex++;
					if (this.curAlertIndex >= AlertsReadout.AlertCycleLength)
					{
						this.curAlertIndex = 0;
					}
					for (int i = this.curAlertIndex; i < this.AllAlerts.Count; i += AlertsReadout.AlertCycleLength)
					{
						Alert alert = this.AllAlerts[i];
						try
						{
							if (alert.Active)
							{
								if (!this.activeAlerts.Contains(alert))
								{
									this.activeAlerts.Add(alert);
									alert.Notify_Started();
								}
							}
							else
							{
								for (int j = 0; j < this.activeAlerts.Count; j++)
								{
									if (this.activeAlerts[j] == alert)
									{
										this.activeAlerts.RemoveAt(j);
										break;
									}
								}
							}
						}
						catch (Exception ex)
						{
							Log.ErrorOnce("Exception processing alert " + alert.ToString() + ": " + ex.ToString(), 743575, false);
							if (this.activeAlerts.Contains(alert))
							{
								this.activeAlerts.Remove(alert);
							}
						}
					}
					for (int k = this.activeAlerts.Count - 1; k >= 0; k--)
					{
						Alert alert2 = this.activeAlerts[k];
						try
						{
							this.activeAlerts[k].AlertActiveUpdate();
						}
						catch (Exception ex2)
						{
							Log.ErrorOnce("Exception updating alert " + alert2.ToString() + ": " + ex2.ToString(), 743575, false);
							this.activeAlerts.RemoveAt(k);
						}
					}
					if (this.mouseoverAlertIndex >= 0 && this.mouseoverAlertIndex < this.activeAlerts.Count)
					{
						IEnumerable<GlobalTargetInfo> culprits = this.activeAlerts[this.mouseoverAlertIndex].GetReport().culprits;
						if (culprits != null)
						{
							foreach (GlobalTargetInfo target in culprits)
							{
								TargetHighlighter.Highlight(target, true, true, false);
							}
						}
					}
					this.mouseoverAlertIndex = -1;
				}
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0016FDE8 File Offset: 0x0016E1E8
		public void AlertsReadoutOnGUI()
		{
			if (Event.current.type != EventType.Layout && Event.current.type != EventType.MouseDrag)
			{
				if (this.activeAlerts.Count != 0)
				{
					Alert alert = null;
					AlertPriority alertPriority = AlertPriority.Critical;
					bool flag = false;
					float num = Find.LetterStack.LastTopY - (float)this.activeAlerts.Count * 28f;
					Rect rect = new Rect((float)UI.screenWidth - 154f, num, 154f, this.lastFinalY - num);
					float num2 = GenUI.BackgroundDarkAlphaForText();
					if (num2 > 0.001f)
					{
						GUI.color = new Color(1f, 1f, 1f, num2);
						Widgets.DrawShadowAround(rect);
						GUI.color = Color.white;
					}
					float num3 = num;
					if (num3 < 0f)
					{
						num3 = 0f;
					}
					for (int i = 0; i < this.PriosInDrawOrder.Count; i++)
					{
						AlertPriority alertPriority2 = this.PriosInDrawOrder[i];
						for (int j = 0; j < this.activeAlerts.Count; j++)
						{
							Alert alert2 = this.activeAlerts[j];
							if (alert2.Priority == alertPriority2)
							{
								if (!flag)
								{
									alertPriority = alertPriority2;
									flag = true;
								}
								Rect rect2 = alert2.DrawAt(num3, alertPriority2 != alertPriority);
								if (Mouse.IsOver(rect2))
								{
									alert = alert2;
									this.mouseoverAlertIndex = j;
								}
								num3 += rect2.height;
							}
						}
					}
					this.lastFinalY = num3;
					UIHighlighter.HighlightOpportunity(rect, "Alerts");
					if (alert != null)
					{
						alert.DrawInfoPane();
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Alerts, KnowledgeAmount.FrameDisplayed);
					}
				}
			}
		}
	}
}
