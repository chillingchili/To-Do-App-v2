using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MauiApp1.Services;

public class ApiService
{
    private static readonly HttpClient _http = SetupHttpClient();

    private static HttpClient SetupHttpClient()
    {
#if ANDROID
        // HttpClient.Timeout is ignored by AndroidMessageHandler (Java socket layer).
        // Set ConnectTimeout directly on the handler to get actual fast failures.
        var handler = new Xamarin.Android.Net.AndroidMessageHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(15),
            ReadTimeout     = TimeSpan.FromSeconds(30),
        };
        var client = new HttpClient(handler);
#else
        var client = new HttpClient(new SocketsHttpHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(15),
        });
#endif
        client.BaseAddress = new Uri("https://todo-list.dcism.org/");
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("User-Agent", "MauiApp-MingotreesRef");
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    private const string ROOT_URL = "https://todo-list.dcism.org";

    private static string Encode(string s) => WebUtility.UrlEncode(s);

    public class ApiResponse
    {
        public int status { get; set; }
        public string? message { get; set; }
    }

    public class UserResponse
    {
        public int id { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? email { get; set; }
        public string? timemodified { get; set; }
    }

    public class ToDoItemResponse
    {
        public int item_id { get; set; }
        public string? item_name { get; set; }
        public string? item_description { get; set; }
        public string? status { get; set; }
        public int user_id { get; set; }
        public string? timemodified { get; set; }
    }

    public class ToDoListResponse
    {
        public int status { get; set; }
        public string? message { get; set; }
        public JsonElement data { get; set; }
        public int? count { get; set; }

        public List<ToDoItemResponse> GetItems()
        {
            var list = new List<ToDoItemResponse>();
            if (data.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in data.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Object)
                    {
                        var item = JsonSerializer.Deserialize<ToDoItemResponse>(prop.Value.GetRawText());
                        if (item != null) list.Add(item);
                    }
                }
            }
            else if (data.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in data.EnumerateArray())
                {
                    var parsed = JsonSerializer.Deserialize<ToDoItemResponse>(item.GetRawText());
                    if (parsed != null) list.Add(parsed);
                }
            }
            return list;
        }
    }

    public class UserApiResponse
    {
        public int status { get; set; }
        public string? message { get; set; }
        public UserResponse? data { get; set; }
    }



    // Auth

    public async Task<UserApiResponse> SignUpAsync(string firstName, string lastName, string email, string password)
    {
        try
        {
            var content = new Dictionary<string, string>
            {
                ["first_name"] = firstName,
                ["last_name"] = lastName,
                ["email"] = email,
                ["password"] = password,
                ["confirm_password"] = password
            };
            var jsonString = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            
            System.Diagnostics.Debug.WriteLine($"SIGNUP REQUEST: {jsonString}");
            var response = await _http.PostAsync($"{ROOT_URL}/signup_action.php", httpContent);
            var text = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"SIGNUP RESPONSE: {text}");

            if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(text))
            {
                return new UserApiResponse { status = (int)response.StatusCode, message = $"Server error: {response.ReasonPhrase}" };
            }

            try 
            {
                return JsonSerializer.Deserialize<UserApiResponse>(text) ?? new UserApiResponse { status = 500, message = "Empty response." };
            }
            catch (JsonException)
            {
                // If it's not JSON, it might be a WAF HTML page or raw error string
                return new UserApiResponse { status = (int)response.StatusCode, message = text.Length > 100 ? text.Substring(0, 100) + "..." : text };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SIGNUP FATAL ERROR: {ex}");
            return new UserApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    public async Task<UserApiResponse> SignInAsync(string email, string password)
    {
        try
        {
            var url = $"{ROOT_URL}/signin_action.php?email={Encode(email)}&password={Encode(password)}";
            System.Diagnostics.Debug.WriteLine($"SIGNIN REQUEST: {url}");
            
            var response = await _http.GetAsync(url);
            var text = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"SIGNIN RESPONSE: {text}");

            try
            {
                return JsonSerializer.Deserialize<UserApiResponse>(text) ?? new UserApiResponse { status = 500, message = "Empty response." };
            }
            catch (JsonException)
            {
                return new UserApiResponse { status = (int)response.StatusCode, message = text.Length > 100 ? text.Substring(0, 100) + "..." : text };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SIGNIN FATAL ERROR: {ex}");
            return new UserApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    // ToDo CRUD

    public async Task<ToDoListResponse> GetToDoItemsAsync(string status, int userId)
    {
        try
        {
            var url = $"{ROOT_URL}/getItems_action.php?status={Encode(status)}&user_id={userId}";
            var response = await _http.GetAsync(url);
            var text = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ToDoListResponse>(text) ?? new ToDoListResponse { status = 500, message = "Empty response." };
        }
        catch (Exception ex)
        {
            return new ToDoListResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> AddToDoAsync(string itemName, string itemDescription, int userId)
    {
        try
        {
            var content = new Dictionary<string, object>
            {
                ["item_name"] = itemName,
                ["item_description"] = itemDescription,
                ["user_id"] = userId
            };
            var jsonString = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"{ROOT_URL}/addItem_action.php", httpContent);
            var text = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(text) ?? new ApiResponse { status = 500, message = "Empty response." };
        }
        catch (Exception ex)
        {
            return new ApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> UpdateToDoAsync(string itemName, string itemDescription, int itemId)
    {
        try
        {
            var content = new Dictionary<string, object>
            {
                ["item_name"] = itemName,
                ["item_description"] = itemDescription,
                ["item_id"] = itemId
            };
            var jsonString = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{ROOT_URL}/editItem_action.php")
            {
                Content = httpContent
            };
            
            var response = await _http.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(text) ?? new ApiResponse { status = 500, message = "Empty response." };
        }
        catch (Exception ex)
        {
            return new ApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> ChangeToDoStatusAsync(string status, int itemId)
    {
        try
        {
            var content = new Dictionary<string, object>
            {
                ["status"] = status,
                ["item_id"] = itemId
            };
            var jsonString = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{ROOT_URL}/statusItem_action.php")
            {
                Content = httpContent
            };

            var response = await _http.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(text) ?? new ApiResponse { status = 500, message = "Empty response." };
        }
        catch (Exception ex)
        {
            return new ApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }

    public async Task<ApiResponse> DeleteToDoAsync(int itemId)
    {
        try
        {
            var response = await _http.DeleteAsync($"{ROOT_URL}/deleteItem_action.php?item_id={itemId}");
            var text = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(text) ?? new ApiResponse { status = 500, message = "Empty response." };
        }
        catch (Exception ex)
        {
            return new ApiResponse { status = 500, message = $"Network Error: {ex.Message}" };
        }
    }
}