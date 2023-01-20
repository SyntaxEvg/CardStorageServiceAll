using CardStorageService.Data.Entity;
using CardStorageService.Services;
using ClientServiceProtos;
using Grpc.Core;
using static ClientServiceProtos.CardService;

namespace Web.TestGrpc.Services
{
    public class CardServices : CardServiceBase
    {
        private readonly ICardRepositoryService _cardRepositoryService;

        public CardServices(ICardRepositoryService cardRepositoryService)
        {
            _cardRepositoryService = cardRepositoryService;
        }
        public override Task<CreateCardResponse> Create(CreateCardRequest request, ServerCallContext context)
        {
            var cardId = _cardRepositoryService.Create(new Card
            {
                ClientId = request.ClientId,
                CardNo = request.CardNo,
                ExpDate =request.ExpDate.ToDateTime(),
                CVV2 = request.CVV2
            });
            var res = new CreateCardResponse
            {
                CardId = cardId.ToString()
            };
            return Task.FromResult(res);
        }
        public override Task<GetByClientIdResponse> GetByClientId(GetByClientIdRequest request, ServerCallContext context)
        {
            GetByClientIdResponse getByClientIdResponse = new GetByClientIdResponse();

            getByClientIdResponse.CardDto.AddRange(_cardRepositoryService.GetByClientId(request.ClientId).Select(x =>
            new CardDtos
            {
                CardId = x.CardId.ToString(),
                CardNo = x.CardNo,
                Name = x.Name,
                CVV2 = x.CVV2,
                ExpDate = x.ExpDate.ToString() 
            }).ToList());
            return Task.FromResult(getByClientIdResponse);
        
        }
    }
}
