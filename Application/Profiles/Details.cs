using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
	public class Details
	{
		public class Query : IRequest<Result<Profile>>
		{
			public string Username { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Profile>>
		{
			private readonly DataContext _ctx;
			private readonly IMapper _mapper;
			private readonly IUserAccessor _userAccessor;

			public Handler(DataContext ctx, IMapper mapper, IUserAccessor userAccessor)
            {
				_ctx = ctx;
				_mapper = mapper;
				_userAccessor = userAccessor;
			}
            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await _ctx.Users.ProjectTo<Profile>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
					.SingleOrDefaultAsync(x => x.Username == request.Username);

				if (user == null) return null;

				return Result<Profile>.Success(user);
			}
		}
	}
}
