using AccentOccurrences;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

// Registers the classifier so that we can tag spans
namespace AccentFontClassifier
{
	[ContentType("any")]
	[Export(typeof(IClassifierProvider))]
	internal class AccentFontClassifierProvider : IClassifierProvider
	{
		[Import]
		internal IClassificationTypeRegistryService ClassificationRegistry = null;

		public IClassifier GetClassifier(ITextBuffer buffer)
		{
			return buffer.Properties.GetOrCreateSingletonProperty<AccentFontClassifier>(delegate { return new AccentFontClassifier(ClassificationRegistry); });
		}
	}

	class AccentFontClassifier : IClassifier
	{
		IClassificationType _classificationType;

		internal AccentFontClassifier(IClassificationTypeRegistryService registry)
		{
			_classificationType = registry.GetClassificationType(Constants.FONT_FORMAT_NAME);
		}

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			return new List<ClassificationSpan>();
		}

#pragma warning disable 67
		// This event gets raised if a non-text change would affect the classification in some way,
		// for example typing /* would cause the classification to change in C# without directly
		// affecting the span.
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67
	}
}