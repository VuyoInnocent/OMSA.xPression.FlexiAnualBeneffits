using System;
using FileHelpers;

namespace SaGE.Correspondence.Domain
{
	[DelimitedRecord("|")]

	public class Template
	{
		public string KeyId;
		public string TemplateName;
		public string Format;
		public string EffectiveDate;
		public string Enabled;
	}
}