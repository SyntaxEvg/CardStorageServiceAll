using ClientServiceProtos;
using Grpc.Net.Client;
using static ClientServiceProtos.CardService;

internal class Program
{
    static void Main(string[] args)
    {
        AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);
        using var channel = GrpcChannel.ForAddress("http://localhost:5001");

        CardServiceClient clientService = new CardServiceClient(channel);
        var createClientResponse = clientService.Create(new CreateCardRequest
        {
            ClientId = 1000,
            CardNo = "43343-2323-232231-32232",
            Name = "Syntax By",
            CVV2 = "011",
            ExpDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Now)
    });

        Console.WriteLine($"Client {createClientResponse.CardId} created successfully.");

        CardServiceClient cardService = new CardServiceClient(channel);

        var getByClientIdResponse = cardService.GetByClientId(new GetByClientIdRequest
        {
            ClientId = createClientResponse.CardId
        });
        if (getByClientIdResponse is not null && getByClientIdResponse.CardDto.Count > 0) 
        {

            foreach (var card in getByClientIdResponse.CardDto)
            {
                Console.WriteLine($"{card.CardNo}; {card.Name}; {card.CVV2}; {card.ExpDate}");
            }
        }
       


        Console.ReadKey();

    }
}
