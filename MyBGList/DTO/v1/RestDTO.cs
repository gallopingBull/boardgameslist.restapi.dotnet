namespace MyBGList.DTO.v1
{
    public class RestDTO<T>
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
        public T Data { get; set; } = default!;
        public int? PageIndex { get; internal set; }
        public int? PageSize { get; internal set; }
        public int? RecordCount { get; internal set; }
    }
}
