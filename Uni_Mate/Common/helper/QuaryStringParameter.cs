namespace Uni_Mate.Common.Helper
{
    public abstract  class QueryStringParamater
    {
 
            private const int MaxPageSize = 30;
            private int _pageSize = 10;
            private int _pageNumber = 1;

            public int PageNumber
            {
                get => _pageNumber;
                set => _pageNumber = (value < 1) ? 1 : value;
            }
            public int PageSize
            {
                get => _pageSize;
                set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
          
        
    }
}
