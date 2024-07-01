namespace BusinessLayer
{
    public class CollectionParameters
    {
        const int maxPageSize = 100000;
        public int PageNumber {get;set;}=1;
        private int _pageSize = 10;
        public int PageSize {
            get {
                return _pageSize;
            }
            set {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
        public string Fields { get; set; }
        public string OrderBy { get; set; } = "Id";
    }
}