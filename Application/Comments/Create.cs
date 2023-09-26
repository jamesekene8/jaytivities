using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments
{
	public class Create
	{
		public class Command : IRequest<Result<CommentDto>> 
		{
			public string Body { get; set; }

			public Guid ActivityId { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.Body).NotEmpty();
			}
		}


		public class Handler : IRequestHandler<Command, Result<CommentDto>>
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
            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
			{
				var activity = await _ctx.Activities.FindAsync(request.ActivityId);

				if (activity == null) return null;

				var user = await _ctx.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

				var comment = new Comment
				{
					Author = user,
					Activity = activity,
					Body = request.Body
				};

				activity.Comments.Add(comment);
				var success = await _ctx.SaveChangesAsync() > 0;
				if (success) return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
				return Result<CommentDto>.Failure("Failed to add comment");

			}
		}

	}
}
