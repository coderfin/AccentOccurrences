using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AccentOccurrences
{
	[ContentType("any")] // Allows the tagging to work with any document type
	[Export(typeof(IViewTaggerProvider))]
	[TagType(typeof(AccentBoxOccurrences))]
	internal class AccentBoxTaggerProvider : IViewTaggerProvider
	{
		[Import]
		internal ITextSearchService TextSearchService { get; set; }

		[Import]
		internal IClassificationTypeRegistryService TypeRegistryService { get; set; }

		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
		{
			if (textView.TextBuffer != buffer) // Provide accenting only on the top buffer 
			{
				return null;
			}

			return new AccentBoxTagger(textView, buffer, TextSearchService, TypeRegistryService) as ITagger<T>;
		}
	}
}