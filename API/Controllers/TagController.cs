using API.DTOs;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class TagController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> GetTags([FromQuery] TagParamsDto tagParams)
        {
            try
            {
                var command = new TagsList.Command
                {
                    PageNumber = tagParams.PageNumber,
                    PageSize = tagParams.PageSize,
                    SortBy = tagParams.SortBy,
                    OrderBy = tagParams.OrderBy
                };

                var tags = await Mediator.Send(command);

                return Ok(new { Tags = tags.Item1, TagsNumber = tags.Item2 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> RefreshTags()
        {
            try
            {
                await Mediator.Send(new TagsDelete.Command());

                await Mediator.Send(new TagsAdd.Command());

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}