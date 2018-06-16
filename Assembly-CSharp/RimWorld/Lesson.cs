using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CE RID: 2254
	public abstract class Lesson : IExposable
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001B6DE4 File Offset: 0x001B51E4
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

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x001B6E20 File Offset: 0x001B5220
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x001B6E38 File Offset: 0x001B5238
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003386 RID: 13190 RVA: 0x001B6E50 File Offset: 0x001B5250
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001B6E6A File Offset: 0x001B526A
		public virtual void ExposeData()
		{
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x001B6E6D File Offset: 0x001B526D
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x001B6E7B File Offset: 0x001B527B
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x0600338A RID: 13194
		public abstract void LessonOnGUI();

		// Token: 0x0600338B RID: 13195 RVA: 0x001B6E7E File Offset: 0x001B527E
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x001B6E81 File Offset: 0x001B5281
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001B6E84 File Offset: 0x001B5284
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x001B6E88 File Offset: 0x001B5288
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x001B6EA4 File Offset: 0x001B52A4
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04001BA5 RID: 7077
		public float startRealTime = -999f;

		// Token: 0x04001BA6 RID: 7078
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
