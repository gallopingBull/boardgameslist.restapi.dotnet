using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO.v1;
using MyBGList.DTO.v1.MyBGList.DTO;
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
            string? sortOrder = "ASC",
            string? filterQuery = null)
        {
            var query = _context.BoardGames.AsQueryable();

            if (!string.IsNullOrEmpty(filterQuery))
                query = query.Where(b => b.Name.StartsWith(filterQuery));

            var recordCount = await query.CountAsync();

            query = query
                        .OrderBy($"{sortcolumn} {sortOrder}")   
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize);

            return new RestDTO<BoardGame[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = recordCount,
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

        [HttpPost(Name = "UpdateBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)
        {
            var boardgame = await _context.BoardGames
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();
            if (boardgame != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    boardgame.Name = model.Name;
                if (model.Year.HasValue && model.Year.Value > 0)
                    boardgame.Year = model.Year.Value;
                if (model.MinPlayers.HasValue && model.MinPlayers.Value > 0)
                    boardgame.MinPlayers = model.MinPlayers.Value;
                if (model.MaxPlayers.HasValue && model.MaxPlayers.Value > 0)
                    boardgame.MaxPlayers = model.MaxPlayers.Value;
                if (model.Playtime.HasValue && model.Playtime.Value > 0)
                    boardgame.PlayTime = model.Playtime.Value;
                if (model.MinAge.HasValue && model.MinAge.Value > 0)
                    boardgame.MinAge = model.MinAge.Value;

                boardgame.LastModifiedDate = DateTime.Now;
                _context.BoardGames.Update(boardgame);
                await _context.SaveChangesAsync();
            };

            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "BoardGames",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [HttpDelete(Name = "DeleteBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame[]?>> Delete(string ids)
        {
            var idArray = ids.Split(',').Select(x => int.Parse(x));
            var deletedBGList = new List<BoardGame>();

            foreach (int id in idArray)
            {
                var boardgame = await _context.BoardGames
                    .Where(b => b.Id == id)
                    .FirstOrDefaultAsync();
                if (boardgame != null)
                {
                    deletedBGList.Add(boardgame);
                    _context.BoardGames.Remove(boardgame);
                    await _context.SaveChangesAsync();
                };
            }

            return new RestDTO<BoardGame[]?>()
            {
                Data = deletedBGList.Count > 0 ? deletedBGList.ToArray() : null,
                Links = new List<LinkDTO>
                    {
                        new LinkDTO(
                                Url.Action(
                                    null,
                                    "BoardGames",
                                    ids,
                                    Request.Scheme)!,
                                "self",
                                "DELETE"),
                    }
            };
        }
    }
}
