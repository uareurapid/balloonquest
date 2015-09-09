using System;

namespace Batch.Internal
{
	// Interface meant to match Java Batch's AdDisplayListener
	// Careful about case sensitivity !
	internal interface IAdDisplayListener
	{
		void FireNoAdDisplayed();

		void FireAdDisplayed();
		
		void FireAdClosed();
		
		void FireAdCancelled();
		
		void FireAdClicked();
	}
}

