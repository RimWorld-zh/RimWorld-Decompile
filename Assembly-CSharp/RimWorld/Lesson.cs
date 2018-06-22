using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CA RID: 2250
	public abstract class Lesson : IExposable
	{
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x001B7094 File Offset: 0x001B5494
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x001B70D0 File Offset: 0x001B54D0
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x001B70E8 File Offset: 0x001B54E8
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x001B7100 File Offset: 0x001B5500
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x001B711A File Offset: 0x001B551A
		public virtual void ExposeData()
		{
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x001B711D File Offset: 0x001B551D
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x001B712B File Offset: 0x001B552B
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06003385 RID: 13189
		public abstract void LessonOnGUI();

		// Token: 0x06003386 RID: 13190 RVA: 0x001B712E File Offset: 0x001B552E
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001B7131 File Offset: 0x001B5531
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x001B7134 File Offset: 0x001B5534
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x001B7138 File Offset: 0x001B5538
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600338A RID: 13194 RVA: 0x001B7154 File Offset: 0x001B5554
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04001BA3 RID: 7075
		public float startRealTime = -999f;

		// Token: 0x04001BA4 RID: 7076
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
