using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class TagsDelete
    {
        public class Command : IRequest {}

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var tags = await _context.Tags.ToListAsync();
                _context.Tags.RemoveRange(tags);
                await _context.SaveChangesAsync();
            }
        }

    }
}