using YamlDotNet.Serialization;

namespace Gapotchenko.Shields.Canonical.Snap.Resolution.Utils;

static class YamlUtil
{
    public static IReadOnlyDictionary<object, object>? ToDictionary(TextReader input)
    {
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<Dictionary<object, object>?>(input);
    }
}
