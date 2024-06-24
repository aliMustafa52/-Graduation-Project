using graduationProject.Api.Contracts.ContactUs;
using graduationProject.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace graduationProject.Api.Services
{
	public class ContactUsService(ApplicationDbContext context)  :IContactUsService
	{
		private readonly ApplicationDbContext _context = context;


		public async Task<IEnumerable<ContactUsResponse>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var messages = await _context.Contacts.AsNoTracking().ToListAsync(cancellationToken);
			return messages.Adapt<IEnumerable<ContactUsResponse>>();
		}


		public async Task<Result<ContactUsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			var message = await _context.Contacts.FindAsync(new object[] { id }, cancellationToken);

			return (Result<ContactUsResponse>)(message is null
				? Result.Failure<ContactUsResponse>(new Error("MessageNotFound", "Message not found", 404))
				: Result.Success(message.Adapt<ContactUsResponse>()));
		}

		public async Task<Result<ContactUsResponse>> AddAsync(ContactUsRequest request, CancellationToken cancellationToken = default)
		{
			var message = request.Adapt<Contact>();

			await _context.Contacts.AddAsync(message, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return (Result<ContactUsResponse>)Result.Success(message.Adapt<ContactUsResponse>());
		}

		public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
		{
			var message = await _context.Contacts.FindAsync(new object[] { id }, cancellationToken);
			if (message == null)
				return Result.Failure(new Error("MessageNotFound", "Message not found", 404));

			_context.Contacts.Remove(message);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}

	}
}
