using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A02 RID: 2562
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x06003976 RID: 14710 RVA: 0x001E764F File Offset: 0x001E5A4F
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds)
		{
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x001E7660 File Offset: 0x001E5A60
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x001E7691 File Offset: 0x001E5A91
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x001E76CA File Offset: 0x001E5ACA
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x001E7704 File Offset: 0x001E5B04
		public override void DoAction(Transition trans)
		{
			if (this.repeatAvoiderTag.NullOrEmpty() || MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				TargetInfo target = (this.lookTargetGetter == null) ? this.lookTarget : this.lookTargetGetter();
				if (!target.IsValid)
				{
					target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
				}
				Messages.Message(this.message, target, this.type, true);
			}
		}

		// Token: 0x0400248E RID: 9358
		public string message;

		// Token: 0x0400248F RID: 9359
		public MessageTypeDef type;

		// Token: 0x04002490 RID: 9360
		public TargetInfo lookTarget;

		// Token: 0x04002491 RID: 9361
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04002492 RID: 9362
		public string repeatAvoiderTag;

		// Token: 0x04002493 RID: 9363
		public float repeatAvoiderSeconds;
	}
}
