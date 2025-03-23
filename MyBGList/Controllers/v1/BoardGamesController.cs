using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO.v1;
using MyBGList.Models;

namespace MyBGList.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BoardGamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<BoardGamesController> _logger;

        public BoardGamesController(ApplicationDbContext context, ILogger<BoardGamesController> logger)
        {
            _context = context; 
            _logger = logger;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<BoardGame[]>> Get(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortcolumn = "Name",
            string? sortOrder = "ASC")
        {
            var query = _context.BoardGames
                        .OrderBy($"{sortcolumn} {sortOrder}")   
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize);

            return new RestDTO<BoardGame[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = await _context.BoardGames.CountAsync(),
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "BoardGames",
                            new { pageIndex, pageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }
    }
}
