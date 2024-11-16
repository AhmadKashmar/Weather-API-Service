namespace WeatherAPI.Models
{
    public class PaginatedResponse<T>
    {
        public int PageNumber { get; set; }       // Current page number
        public int PageSize { get; set; }         // Number of items per page
        public int TotalPages { get; set; }       // Total number of pages
        public int TotalRecords { get; set; }     // Total number of records
        public IEnumerable<T>? Data { get; set; }  // Data for the current page
    }
}
