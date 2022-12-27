using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Xml;

namespace Instrument;

public class WritableJsonConfigurationSource : JsonConfigurationSource
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        this.EnsureDefaults(builder);
        return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
    }
}

public class WritableJsonConfigurationProvider : JsonConfigurationProvider
{
    public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
    {
    }

    public override void Set(string key, string value)
    {
        base.Set(key,value);

        //Get Whole json file and change only passed key with passed value. It requires modification if you need to support change multi level json structure
        var fileFullPath = base.Source.FileProvider.GetFileInfo(base.Source.Path).PhysicalPath;
        string json = File.ReadAllText(fileFullPath);
        JsonNode jsonObj = JsonNode.Parse(json);
        jsonObj[key] = value;
        string output = jsonObj.ToJsonString(JsonOption.Options);
        File.WriteAllText(fileFullPath, output);
    }
}
