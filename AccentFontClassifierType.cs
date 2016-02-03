using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AccentFontClassifier
{
	internal static class AccentFontClassifierClassificationDefinition
	{
		[Export(typeof(ClassificationTypeDefinition))]
		[Name("EditorFormatDefinition/AccentFontClassifier")]
		internal static ClassificationTypeDefinition AccentFontClassifierType = null;
	}
}