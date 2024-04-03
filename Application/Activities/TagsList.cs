using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class TagsList
    {
        // Define the command with the required parameters
        public class Command : IRequest<(List<AppTag>, int tagsNumber)>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public required string SortBy { get; set; }
            public required string OrderBy { get; set; }
        }

        // Define the handler for the command
        public class Handler : IRequestHandler<Command, (List<AppTag>, int tagsNumber)>
        {
            private readonly DataContext _context;
            const int MaxPageSize = 50; // Define the maximum page size

            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle the command
            public async Task<(List<AppTag>, int tagsNumber)> Handle(Command request, CancellationToken cancellationToken)
            {
                // Determine the page size
                int pageSize = (request.PageSize > MaxPageSize) ? MaxPageSize : request.PageSize;

                // Start with a query that includes all tags
                var query = _context.Tags.AsQueryable();

                // If the sort by parameter is "Name", order by name
                if (request.SortBy == "Name")
                {
                    query = request.OrderBy == "asc" ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name);
                }
                // If the sort by parameter is "Participation", order by count
                else if (request.SortBy == "Participation")
                {
                    query = request.OrderBy == "asc" ? query.OrderBy(t => t.Count) : query.OrderByDescending(t => t.Count);
                }

                // Get the tags for the current page
                var tags = await query
                    .Skip((request.PageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Get the total number of tags
                var tagsNumber = await query.CountAsync();

                // Return the tags and the total number of tags
                return (tags, tagsNumber);
            }
        } 
    }
}