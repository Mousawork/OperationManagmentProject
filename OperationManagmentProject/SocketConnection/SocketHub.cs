namespace OperationManagmentProject.SocketConnection
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using OperationManagmentProject.Data;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class SocketHub : Hub
    {
        private readonly AppDbContext _context;

        public SocketHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task JoinToRoom(string jsonData)
        {
            var data = JsonSerializer.Deserialize<JoinToRoomModel>(jsonData);
            if (data != null)
            {
                Debug.WriteLine($"{data.UserId} is joined");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{data.UserId}");
            }
        }

        public async Task JoinCall(string data)
        {
            var newData = JsonSerializer.Deserialize<DataModel>(data);
            if (newData != null)
            {
                var senderId = newData.SenderId;
                var receiverId = newData.ReceiverId;
                Debug.WriteLine($"sender: {senderId}, receiver: {receiverId}");
                var g = Groups;
                await Clients.Group($"{receiverId}").SendAsync("JoinMeeting", JsonSerializer.Serialize(newData));
                //await Clients.Group($"1").SendAsync("JoinMeeting", JsonSerializer.Serialize(newData));
                Debug.WriteLine($"new connect to {receiverId}");
            }
        }

        public async Task AllJoinCall()
        {
            var users = await GetUserIdsAsync(); // Implement a method to get user IDs
            foreach (var user in users)
            {
                var newData = new { ReceiverId = user };
                await Clients.Group($"{user}").SendAsync("AllJoinMeeting", JsonSerializer.Serialize(newData));
                Debug.WriteLine($"new connect to {user}");
            }
        }

        private async Task<List<string>> GetUserIdsAsync()
        {
            return await _context.UserProfile.Select(u => u.Id.ToString()).ToListAsync();
        }
    }

    public class JoinToRoomModel
    {
        [JsonPropertyName("UserId")]
        public string UserId { get; set; }
    }

    public class DataModel
    {
        [JsonPropertyName("SenderId")]
        public string SenderId { get; set; }

        [JsonPropertyName("ReceiverId")]
        public string ReceiverId { get; set; }
    }

}
