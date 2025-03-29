using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBGList.DTO.v1
{
    namespace MyBGList.DTO
    {
        public class BoardGameDTO
        {
            [Required]
            public int Id { get; set; }

            public string? Name { get; set; }

            public int? Year { get; set; }
            
            public int? MinPlayers { get; set; }
            
            public int? MaxPlayers { get; set; }

            public int? Playtime { get; set; }

            public int? MinAge { get; set; }


        }
    }
}
