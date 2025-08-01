namespace BaseNKatmanProject.Core.Commons
{
    public class PageParameters
    {
        public int PageNumber { get; set; } = 1;  // Başlangıç sayfası
        public int PageSize { get; set; } = 10;   // Sayfadaki kayıt sayısı

        // Minimum ve maksimum kontrolü opsiyonel
        public int MaxPageSize { get; set; } = 100;

        public void Validate()
        {
            if (PageNumber < 1)
                PageNumber = 1;

            if (PageSize < 1)
                PageSize = 10;

            if (PageSize > MaxPageSize)
                PageSize = MaxPageSize;
        }
    }
}
