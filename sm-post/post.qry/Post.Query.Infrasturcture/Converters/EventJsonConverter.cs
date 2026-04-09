using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Post.Common.Events;

namespace Post.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }
        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //throw new NotImplementedException();
            if(!JsonDocument.TryParseValue(ref reader,out var doc))
            {
                throw new JsonException($" failed to parse {nameof(JsonDocument)}Invalid JSON");
            }
            if(!doc.RootElement.TryGetProperty("Type",out var type))
            {
                throw new JsonException($" could not detect type discriminator property {nameof(BaseEvent.Type)}");
            }
            var typeDiscriminator = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
                nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<MessageUpdatedEvent>(json, options),
                nameof(PostLikeEvent) => JsonSerializer.Deserialize<PostLikeEvent>(json, options),
                nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(json, options),
                nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
                nameof(CommentRemoveEvent) => JsonSerializer.Deserialize<CommentRemoveEvent>(json, options),
                nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(json, options),
                _ => throw new JsonException($"{typeDiscriminator} is not MulticastNotSupportedException yet")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
