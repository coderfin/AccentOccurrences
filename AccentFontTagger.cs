using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AccentOccurrences
{
	class AccentFontTagger : ITagger<ClassificationTag>
	{
		ITextView View { get; set; }
		ITextBuffer SourceBuffer { get; set; }
		ITextSearchService TextSearchService { get; set; }
		NormalizedSnapshotSpanCollection WordSpans { get; set; }
		SnapshotSpan? CurrentWord { get; set; }
		object updateLock = new object();
		bool HasAdornments { get; set; }
		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
		IClassificationTypeRegistryService TypeService { get; set; }

		public AccentFontTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService, IClassificationTypeRegistryService typeService)
		{
			this.View = view;
			this.SourceBuffer = sourceBuffer;
			this.TextSearchService = textSearchService;
			this.TypeService = typeService;
			this.WordSpans = new NormalizedSnapshotSpanCollection();
			this.CurrentWord = null;
			this.HasAdornments = false;
			this.View.Selection.SelectionChanged += ViewSelectionChanged;
		}

		void ViewSelectionChanged(object sender, EventArgs e)
		{
			if (!View.Selection.IsEmpty)
			{
				HasAdornments = true;
				UpdateWordAdornments();
			}
			else if (HasAdornments)
			{
				ClearWordAdornments();
				HasAdornments = false;
			}
		}

		void ClearWordAdornments()
		{
			SynchronousUpdate(new NormalizedSnapshotSpanCollection(new List<SnapshotSpan>()), new SnapshotSpan());
		}

		void UpdateWordAdornments()
		{
			List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
			SnapshotSpan currentWord = this.View.Selection.StreamSelectionSpan.SnapshotSpan;

			if (CurrentWord.HasValue && currentWord == CurrentWord)
			{
				return;
			}

			// Find the new spans
			FindData findData = new FindData(currentWord.GetText(), currentWord.Snapshot);

			bool ctrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
			if (ctrlDown)
			{
				findData.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;
			}
			else
			{
				findData.FindOptions = FindOptions.WholeWord;
			}

			wordSpans.AddRange(TextSearchService.FindAll(findData));

			SynchronousUpdate(new NormalizedSnapshotSpanCollection(wordSpans), currentWord);
		}

		void SynchronousUpdate(NormalizedSnapshotSpanCollection newSpans, SnapshotSpan? newCurrentWord)
		{
			lock (updateLock)
			{
				WordSpans = newSpans;
				CurrentWord = newCurrentWord;

				var tempEvent = TagsChanged;
				if (tempEvent != null)
				{
					tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(SourceBuffer.CurrentSnapshot, 0, SourceBuffer.CurrentSnapshot.Length)));
				}
			}
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			if (CurrentWord == null)
			{
				yield break;
			}

			// Hold on to a "snapshot" of the word spans and current word, so that the same collection is maintained throughout
			SnapshotSpan currentWord = CurrentWord.Value;
			NormalizedSnapshotSpanCollection wordSpans = WordSpans;

			if (spans.Count == 0 || WordSpans.Count == 0)
			{
				yield break;
			}

			// If the requested snapshot isn't the same as the one the words are on, translate the spans to the expected snapshot 
			if (spans[0].Snapshot != wordSpans[0].Snapshot)
			{
				wordSpans = new NormalizedSnapshotSpanCollection(wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

				currentWord = currentWord.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
			}

			// First, yield back the word the cursor is under (if it overlaps) 
			// Note that this will yield back the same word again in the wordspans collection; the duplication here is expected. 
			if (spans.OverlapsWith(new NormalizedSnapshotSpanCollection(currentWord)))
			{
				yield return new TagSpan<ClassificationTag>(currentWord, new ClassificationTag(TypeService.GetClassificationType(Constants.FONT_FORMAT_NAME)));
			}

			// Second, yield all the other words in the file 
			foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
			{
				yield return new TagSpan<ClassificationTag>(span, new ClassificationTag(TypeService.GetClassificationType(Constants.FONT_FORMAT_NAME)));
			}
		}
	}
}