using System;

namespace CardStorageServiceAll.Models.DTO
{
    public class CardDto
    {
        public Guid CardId { get; set; }


        public string CardNo { get; set; }


        public string? Name { get; set; }


        public string? CVV2 { get; set; }

        public string ExpDate { get; set; }
    }
}
