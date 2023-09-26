using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public string Id { get; set; }
		}

		public class handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext _ctx;
			private readonly IPhotoAccessor _photoAccessor;
			private readonly IUserAccessor _userAccessor;

			public handler(DataContext ctx, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
				_ctx = ctx;
				_photoAccessor = photoAccessor;
				_userAccessor = userAccessor;
			}
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _ctx.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

				if (user == null) return null;

				var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

				if (photo == null) return null;

				if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

				var result = await _photoAccessor.DeletePhoto(photo.Id);

				if (result == null) return Result<Unit>.Failure("Problem delting photo from Cloudinary");

				user.Photos.Remove(photo);

				var success = await _ctx.SaveChangesAsync() > 0;

				if (success) return Result<Unit>.Success(Unit.Value);

				return Result<Unit>.Failure("Problem delting photo from API");
			}
		}
	}
}
