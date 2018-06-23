using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009FE RID: 2558
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x04002489 RID: 9353
		public string message;

		// Token: 0x0400248A RID: 9354
		public MessageTypeDef type;

		// Token: 0x0400248B RID: 9355
		public TargetInfo lookTarget;

		// Token: 0x0400248C RID: 9356
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x0400248D RID: 9357
		public string repeatAvoiderTag;

		// Token: 0x0400248E RID: 9358
		public float repeatAvoiderSeconds;

		// Token: 0x06003970 RID: 14704 RVA: 0x001E788F File Offset: 0x001E5C8F
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds)
		{
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x001E78A0 File Offset: 0x001E5CA0
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001E78D1 File Offset: 0x001E5CD1
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

		// Token: 0x06003973 RID: 14707 RVA: 0x001E790A File Offset: 0x001E5D0A
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

		// Token: 0x06003974 RID: 14708 RVA: 0x001E7944 File Offset: 0x001E5D44
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
	}
}
