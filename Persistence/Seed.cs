using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence.DTOs;
using System.Net;
using System.Text.Json;

namespace Persistence
{
    public class Seed
    {
        // Set the API URL and the minimum number of tags to fetch
        private const string apiUrl = "https://api.stackexchange.com//2.3/tags?order=asc&sort=name&site=stackoverflow&pagesize=100";
        private const int minTagsCount = 1000;
        
        public static async Task SeedData(UserManager<AppUser> userManager, DataContext context)
        {
            // If there are no users, create some test users
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{UserName = "Test", Email = "Test"},
                    new AppUser{UserName = "Jakub", Email = "user01@gmail.com"},
                    new AppUser{UserName = "Mateusz", Email = "user02@gmail.com"},
                    new AppUser{UserName = "Marcin", Email = "user03@gmail.pl"},
                    new AppUser{UserName = "Bartek", Email = "user04@gmail.pl"}
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Test");
                };
            }

            // If there are any tags, remove them
            if(context.Tags.Any())
            {
                var tags = context.Tags;
                context.Tags.RemoveRange(tags);
                await context.SaveChangesAsync();
            }

            // If there are no tags, fetch them from the API
            if (!context.Tags.Any())
            {
                // Create a new HttpClient with automatic decompression
                using (var httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    int fetchedTagsCount = 0;
                    int page = 1;
                    var allTags = new List<AppTag>(); // Temporary list to store all tags

                    // Keep fetching tags until we reach the minimum required tags count
                    while (fetchedTagsCount < minTagsCount)
                    {
                        string requestUrl = $"{apiUrl}&page={page}";

                        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();

                            // Deserialize the JSON response to a TagsResponse object
                            var tagsResponse = JsonSerializer.Deserialize<TagsResponse>(jsonResponse);

                            if (tagsResponse == null || tagsResponse.items == null || tagsResponse.items.Count == 0)
                                throw new Exception("No tags in the response");

                            var tagsCountSum = tagsResponse.items.Sum(i => i.count);

                            // Map the response items to AppTag objects and add them to a list
                            var tagsAdd = tagsResponse.items.Select(item => new AppTag
                            {
                                Name = item.name,
                                HasSynonyms = item.has_synonyms,
                                IsModeratorOnly = item.is_moderator_only,
                                IsRequired = item.is_required,
                                Count = item.count,
                                Percentage = (tagsCountSum != 0 && item.count != 0) ? Math.Round((decimal)item.count / tagsCountSum * 100, 2) : 0
                            }).ToList();

                            allTags.AddRange(tagsAdd); // Add tags to the temporary list

                            fetchedTagsCount += tagsAdd.Count;
                            page++; // Go to the next page
                        }
                        else
                        {
                            throw new Exception($"Failed to fetch tags. Status code: {response.StatusCode}");
                        }
                    }

                    // Add all fetched tags to the database
                    context.Tags.AddRange(allTags);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}