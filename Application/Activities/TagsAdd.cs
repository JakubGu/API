using Domain;
using MediatR;
using Persistence;
using Persistence.DTOs;
using System.Net;
using System.Text.Json;

namespace Application.Activities
{
    public class TagsAdd
    {
        public class Command : IRequest {}
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            // Define the minimum number of tags to fetch
            private const int minTagsCount = 1000;

            // Define the API URL
            private const string apiUrl = "https://api.stackexchange.com//2.3/tags?order=asc&sort=name&site=stackoverflow&pagesize=100";
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                // Create a new HTTP client with automatic decompression
                using (var httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    // Initialize the count of fetched tags and the page number
                    int fetchedTagsCount = 0;
                    int page = 1;

                    // Create a temporary list to store all tags
                    var allTags = new List<AppTag>();

                    // Fetch tags until the minimum number of tags is reached
                    while (fetchedTagsCount < minTagsCount)
                    {
                        // Create the request URL
                        string requestUrl = $"{apiUrl}&page={page}";

                        // Send the HTTP request
                        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            var tagsResponse = JsonSerializer.Deserialize<TagsResponse>(jsonResponse);

                            // If there are no tags in the response, throw an exception
                            if (tagsResponse == null || tagsResponse.items == null || tagsResponse.items.Count == 0)
                                throw new Exception("No tags in the response");

                            // Calculate the sum of the counts of all tags
                            var tagsCountSum = tagsResponse.items.Sum(i => i.count);

                            // Create a list of tags to add
                            var tagsAdd = tagsResponse.items.Select(item => new AppTag
                            {
                                Name = item.name,
                                HasSynonyms = item.has_synonyms,
                                IsModeratorOnly = item.is_moderator_only,
                                IsRequired = item.is_required,
                                Count = item.count,
                                Percentage = (tagsCountSum != 0 && item.count != 0) ? Math.Round((decimal)item.count / tagsCountSum * 100, 2) : 0
                            }).ToList();

                            // Add the tags to the temporary list
                            allTags.AddRange(tagsAdd);
                            fetchedTagsCount += tagsAdd.Count;
                            page++;
                        }
                        else
                        {
                            throw new Exception($"Failed to fetch tags. Status code: {response.StatusCode}");
                        }
                    }

                    // Add all tags to the database
                    _context.Tags.AddRange(allTags);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}