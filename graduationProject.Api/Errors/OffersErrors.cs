namespace graduationProject.Api.Errors
{
	public static class OffersErrors
	{
		public static readonly Error OfferNotFound =
			new("Offer.NotFount", "No Offer was found with this Id", StatusCodes.Status404NotFound);

		public static readonly Error DuplicateOffer =
			new("Offer.DuplicateOffer", "Cannot submit two offers for the same project", StatusCodes.Status400BadRequest);
	}
}
