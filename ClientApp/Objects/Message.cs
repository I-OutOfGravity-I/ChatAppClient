using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientApp.Objects
{
    [Serializable]
    public class Message
    {
        public MessageType Type { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }

        [JsonConstructor]
        public Message(MessageType type, string username, string content)
        {
            Type = type;
            Username = username;
            Content = content;
        }

        public static Message DeserializeMessage(string jsonString)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<Message>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing object: {ex.Message}");
                return null;
            }
        }
        public static string SerializeMessage(Message obj)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing object: {ex.Message}");
                return null;
            }
        }
        public static List<string> DeserializeStringList(string jsonString)
        {
            jsonString = jsonString.TrimStart('[').TrimEnd(']');

            string[] items = jsonString.Split(',');

            List<string> deserializedList = new List<string>();
            foreach (var item in items)
            {
                string unescapedItem = UnescapeString(item.Trim('\"'));
                deserializedList.Add(unescapedItem);
            }

            return deserializedList;
        }

        public static string EscapeString(string input)
        {
            return input.Replace("\"", "\\\"");
        }

        public static string UnescapeString(string input)
        {
            return input.Replace("\\\"", "\"");
        }
    }
}
